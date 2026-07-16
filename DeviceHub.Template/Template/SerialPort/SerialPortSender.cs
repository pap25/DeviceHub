using DeviceHub.Utils;
using DeviceHub.Template.Constant;
using System.Collections.Concurrent;

namespace DeviceHub.Template.Template.SerialPort
{
    public class SerialPortSender
    {
        private readonly string logType = nameof(SerialPortSender);
        private readonly SerialPortSession session;
        private readonly ISenderSerialPortTaskHandler senderTaskHandler;

        // 接收线程需要回复 ACK 等控制字符时，放入此队列，由发送线程统一写串口。
        private readonly ConcurrentQueue<byte[]> controlWriteQueue = new();

        // 唤醒发送线程：有控制字符要写、有新发送机会、或需要停止线程。
        private readonly AutoResetEvent wakeEvent = new(false);

        // 接收线程收到 ACK/NAK 后通知发送线程结束等待。
        private readonly AutoResetEvent ackEvent = new(false);
        private readonly object ackLock = new();
        private CancellationTokenSource? sendCts;
        private Task? sendTask;
        private SendAckResult ackResult = SendAckResult.None;

        // 对端 ENQ/EOT 或会话重置时，用于中断当前发送流程。
        private volatile bool abortRequested;

        // 当前正在发送的 ASTM 帧列表，来源仍然是数据库待发送任务。
        private List<byte[]> sendFrameList = [];
        private int sendFrameOffset;

        // ASTM 常用超时/重试参数：发送 ENQ 或数据帧后等待 ACK。
        private const int AckTimeoutSeconds = 15;
        private const int MaxRetryCount = 6;

        public SerialPortSender(SerialPortSession session, ISenderSerialPortTaskHandler senderTaskHandler)
        {
            this.session = session;
            this.senderTaskHandler = senderTaskHandler;
        }

        /// <summary>
        /// 启动唯一发送线程。所有 SerialPort.Write 都应通过该线程完成。
        /// </summary>
        public void Start()
        {
            if (sendTask is { IsCompleted: false })
            {
                return;
            }

            sendCts = new CancellationTokenSource();
            CancellationToken token = sendCts.Token;
            sendTask = Task.Factory.StartNew(
                () => SendLoop(token),
                token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        /// <summary>
        /// 停止发送线程，并唤醒可能正在等待 ACK 或发送机会的循环。
        /// </summary>
        public void Stop()
        {
            CancellationTokenSource? cts = sendCts;
            sendCts = null;
            cts?.Cancel();
            wakeEvent.Set();
            ackEvent.Set();

            try
            {
                sendTask?.Wait(TimeSpan.FromSeconds(2));
            }
            catch (AggregateException ex)
            {
                Logger.Warn(logType, $"等待串口发送循环结束异常: {ex.InnerException?.Message ?? ex.Message}");
            }
            finally
            {
                cts?.Dispose();
                sendTask = null;
                ClearSendFrames();
            }
        }

        /// <summary>
        /// 清理当前发送任务的帧缓存和发送偏移。
        /// </summary>
        public void ClearSendFrames()
        {
            sendFrameList = [];
            sendFrameOffset = 0;
            abortRequested = false;
        }

        /// <summary>
        /// 将控制字符加入写队列。常用于接收完整帧后回复 ACK。
        /// </summary>
        public void EnqueueControl(byte data)
        {
            controlWriteQueue.Enqueue([data]);
            wakeEvent.Set();
        }

        /// <summary>
        /// 接收线程收到 ACK 后调用，通知发送线程当前发送已被确认。
        /// </summary>
        public void NotifyAck()
        {
            SetAckResult(SendAckResult.Ack);
        }

        /// <summary>
        /// 接收线程收到 NAK 后调用，通知发送线程需要重发。
        /// </summary>
        public void NotifyNak()
        {
            SetAckResult(SendAckResult.Nak);
        }

        /// <summary>
        /// 唤醒发送线程重新检查线路状态和数据库任务。
        /// </summary>
        public void WakeUp()
        {
            wakeEvent.Set();
        }

        /// <summary>
        /// 中断当前发送流程。常见场景是对端抢占发送 ENQ，或会话收到 EOT。
        /// </summary>
        public void AbortCurrentTransmission()
        {
            abortRequested = true;
            SetAckResult(SendAckResult.Timeout);
            wakeEvent.Set();
        }

        /// <summary>
        /// 发送主循环：优先写控制字符，线路空闲时从数据库取待发任务并发送。
        /// </summary>
        private void SendLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // 先处理接收线程排队的 ACK 等控制字符，避免对端长时间等待响应。
                    DrainControlWrites();

                    // 只有线路空闲时才能主动发送；若正在接收，则等待下一次唤醒。
                    if (!session.TryBeginOutgoingTransmission())
                    {
                        WaitHandle.WaitAny([wakeEvent, token.WaitHandle], TimeSpan.FromSeconds(1));
                        continue;
                    }

                    // 数据库队列方式保持不变：每次从 DB 取一条待发送任务并编码成 ASTM 帧。
                    sendFrameList = senderTaskHandler.SearchEncoderTask();
                    if (sendFrameList.Count == 0)
                    {
                        // 没有任务时释放刚占用的发送状态，避免线路一直停在 WaitingAck。
                        session.ReleaseOutgoingTransmission();
                        WaitHandle.WaitAny([wakeEvent, token.WaitHandle], TimeSpan.FromSeconds(1));
                        continue;
                    }

                    Logger.Debug(logType, $"待发送帧条数: {sendFrameList.Count}");
                    sendFrameOffset = 0;
                    abortRequested = false;

                    // 发送 ENQ、等待 ACK、逐帧发送、超时/NAK 重试都在这里完成。
                    bool success = SendCurrentTask(token);
                    ClearSendFrames();
                    session.ReleaseOutgoingTransmission();

                    if (!success)
                    {
                        // 失败后稍等再抢占线路，避免和对端持续冲突或高频重试。
                        WaitHandle.WaitAny([wakeEvent, token.WaitHandle], TimeSpan.FromSeconds(ASTMProtocols.NakRetryDelaySeconds));
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Error(logType, "串口发送循环异常", ex);
                    // 异常后清理当前任务状态并释放线路，避免状态机卡死。
                    ClearSendFrames();
                    session.ReleaseOutgoingTransmission();
                    WaitHandle.WaitAny([wakeEvent, token.WaitHandle], TimeSpan.FromSeconds(1));
                }
            }
        }

        /// <summary>
        /// 发送当前数据库任务：先发送 ENQ 建立会话，再逐帧发送，最后发送 EOT。
        /// </summary>
        private bool SendCurrentTask(CancellationToken token)
        {
            if (abortRequested)
            {
                return false;
            }

            // ASTM 发送方先发 ENQ，只有收到 ACK 后才能开始发送数据帧。
            if (!SendWithAck([ASTMProtocols.ENQ], "ENQ", token))
            {
                Logger.Warn(logType, "ENQ 等待 ACK 超过重试次数，本次发送放弃");
                session.WriteToTransport(ASTMProtocols.EOT);
                return false;
            }

            // 确认当前仍持有本端发送状态，再从 WaitingAck 切换到 Sending。
            if (!session.TryMarkOutgoingSending())
            {
                Logger.Warn(logType, "ENQ 已收到 ACK，但线路状态已切换，本次发送放弃");
                return false;
            }

            // 每一帧都必须等待 ACK；NAK 或超时会在 SendWithAck 内重试。
            while (sendFrameList.Count > sendFrameOffset && !abortRequested)
            {
                byte[] frame = sendFrameList[sendFrameOffset];
                if (!SendWithAck(frame, $"Frame[{sendFrameOffset + 1}/{sendFrameList.Count}]", token))
                {
                    Logger.Warn(logType, $"帧发送等待 ACK 超过重试次数 offset={sendFrameOffset}");
                    session.WriteToTransport(ASTMProtocols.EOT);
                    return false;
                }

                sendFrameOffset++;
            }

            // 对端中断时不能回写发送成功。
            if (abortRequested)
            {
                Logger.Warn(logType, "当前发送被对端结束或会话重置中断");
                return false;
            }

            // 全部帧 ACK 后，回写数据库发送成功并发送 EOT 结束会话。
            senderTaskHandler.Completed(sendFrameList);
            session.WriteToTransport(ASTMProtocols.EOT);
            return true;
        }

        /// <summary>
        /// 发送一段数据并等待 ACK。收到 NAK 或超时会按最大次数重发。
        /// </summary>
        private bool SendWithAck(byte[] data, string description, CancellationToken token)
        {
            for (int retry = 1; retry <= MaxRetryCount && !token.IsCancellationRequested && !abortRequested; retry++)
            {
                // 每次重发前都清空上一次 ACK/NAK 结果，避免误用旧信号。
                ResetAckResult();
                session.WriteToTransport(data);

                SendAckResult result = WaitAck(token);
                if (result == SendAckResult.Ack)
                {
                    return true;
                }

                // NAK 和 Timeout 都进入下一轮重发，直到超过最大重试次数。
                Logger.Warn(logType, $"{description} 等待结果={result}, retry={retry}/{MaxRetryCount}");
            }

            return false;
        }

        /// <summary>
        /// 等待接收线程通知 ACK/NAK，等待期间也允许优先写控制字符。
        /// </summary>
        private SendAckResult WaitAck(CancellationToken token)
        {
            DateTimeOffset deadline = DateTimeOffset.UtcNow.AddSeconds(AckTimeoutSeconds);

            while (!token.IsCancellationRequested && !abortRequested)
            {
                TimeSpan remaining = deadline - DateTimeOffset.UtcNow;
                if (remaining <= TimeSpan.Zero)
                {
                    return SendAckResult.Timeout;
                }

                // 同时等待 ACK/NAK、控制写唤醒、停止信号。
                int index = WaitHandle.WaitAny([ackEvent, wakeEvent, token.WaitHandle], remaining);
                if (index == 0)
                {
                    lock (ackLock)
                    {
                        return ackResult;
                    }
                }

                if (index == 1)
                {
                    // 等 ACK 期间如果需要回复对端控制字符，先写出去再继续等待。
                    DrainControlWrites();
                    continue;
                }

                return SendAckResult.Timeout;
            }

            return SendAckResult.Timeout;
        }

        /// <summary>
        /// 写出排队的控制字符，保证控制字符也由唯一发送线程调用 SerialPort.Write。
        /// </summary>
        private void DrainControlWrites()
        {
            while (controlWriteQueue.TryDequeue(out byte[]? data))
            {
                session.WriteToTransport(data);
            }
        }

        /// <summary>
        /// 重置本轮 ACK/NAK 等待结果。
        /// </summary>
        private void ResetAckResult()
        {
            lock (ackLock)
            {
                ackResult = SendAckResult.None;
                ackEvent.Reset();
            }
        }

        /// <summary>
        /// 设置 ACK/NAK 等待结果，并唤醒正在等待的发送线程。
        /// </summary>
        private void SetAckResult(SendAckResult result)
        {
            lock (ackLock)
            {
                ackResult = result;
            }

            ackEvent.Set();
        }
    }

    /// <summary>
    /// 发送线程等待接收线程反馈后的结果。
    /// </summary>
    internal enum SendAckResult
    {
        None,
        Ack,
        Nak,
        Timeout
    }
}
