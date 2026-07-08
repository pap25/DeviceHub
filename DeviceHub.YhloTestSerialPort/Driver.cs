using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Base.Common;
using DeviceHub.Base.Constant;
using DeviceHub.Base.Transports;
using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Service;
using DeviceHub.YhloTestSerialPort.Handler;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;

namespace DeviceHub.YhloTestSerialPort
{
    public class Driver : ISerialDeviceDriver
    {
        private readonly string logType = nameof(Driver);
        private IConsumeTask receiveTask;
        private ISenderTaskHandler senderTaskHandler;
        private IConsumeTask lisIssueApplication;
        private readonly ReceiveMessageService receiveMessageService = ReceiveMessageService.Instance;
        private long _instrumentId;

        private LineState lineState = LineState.Idle;
        private long lastReceiveTime;
        private SerialPortTransport transport;
        private readonly List<byte> buffer = new();
        private readonly object stateLock = new();
        private Timer? idleCheckTimer;
        private const int ReceiveIdleTimeoutSeconds = 15;

        /// <summary>
        /// buffer 中下一待解析帧的起始位置（此前完整帧均已 ACK）
        /// </summary>
        private int processedOffset;

        public async Task Start(long instrumentId, SerialPortConfig config)
        {
            _instrumentId = instrumentId;
            transport = new(
                    config.PortName,
                    config.BaudRate,
                    (Parity)config.Parity,
                    config.DataBits,
                    (StopBits)config.StopBits
                );

            transport.DataReceived += Transport_DataReceived;

            receiveTask = new BatchConsumeTask<ReceiveMessage>(new ReceiveHandler(instrumentId));
            receiveTask.StartConsume();
            lisIssueApplication = new BatchConsumeTask<GetSampleApplyListOutput>(new LisIssueApplication(instrumentId));
            lisIssueApplication.StartConsume();

            senderTaskHandler = new SendHandler(instrumentId);

            lastReceiveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            idleCheckTimer = new Timer(
                CheckReceiveIdleTimeout,
                null,
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(5));

            await Task.Run(transport.Open);
        }

        /// <summary>
        /// 串口数据接收事件
        /// </summary>
        private void Transport_DataReceived(byte[] data)
        {
            try
            {
                lastReceiveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Logger.Debug(logType, $"串口接收消息: {Decode(data)}");

                buffer.AddRange(data);

                // 提取控制字符
                while (TryExtractControlChar(out byte controlChar))
                {
                    HandleControlChar(controlChar);
                }

                // 按 Terminator Record（L 记录）切分完整消息
                while (TryExtractMessage(out List<byte> message))
                {
                    LogCompleteMessage(message);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "串口接收数据处理异常", ex);
                ResetReceiveBuffer();
            }
        }

        /// <summary>
        /// 处理控制字符
        /// </summary>
        private void HandleControlChar(byte controlChar)
        {
            lock (stateLock)
            {
                switch (controlChar)
                {
                    case ASTMProtocols.ENQ:
                        Logger.Info(logType, "收到 ENQ，回复ACK");
                        lineState = LineState.Receiving;
                        transport.Send(ASTMProtocols.ACK);
                        return;

                    case ASTMProtocols.ACK:
                        Logger.Debug(logType, "收到 ACK");
                        if (sendFrameList.Count == 0)
                            return;
                        if (lineState is not (LineState.WaitingAck or LineState.Sending))
                            return;
                        lineState = LineState.Sending;
                        SendFrameUnlocked();
                        return;

                    case ASTMProtocols.NAK:
                        Logger.Info(logType, "收到 NAK");
                        lineState = LineState.Receiving;
                        return;

                    case ASTMProtocols.EOT:
                        Logger.Info(logType, "收到 EOT，本次传输结束，清理未完成消息");
                        ResetReceiveBuffer();
                        if (lineState == LineState.WaitingAck)
                        {
                            sendFrameList = [];
                            sendFrameOffset = 0;
                        }
                        lineState = LineState.Idle;
                        SendFrameUnlocked();
                        return;
                }
            }
        }

        /// <summary>
        /// 按 Terminator Record（L 记录）切分完整 ASTM 消息；多帧累积至含 L 记录后一次性取出。
        /// </summary>
        private bool TryExtractMessage(out List<byte> message)
        {
            message = new List<byte>(0);

            // buffer 三帧拼接示例：
            //   <STX>1H|\^&|||^^||||||ASCII|PR|1394-97|20180120110408<CR><ETX>A6<CR><LF>
            //   <STX>4R|1|TP^TP1I|^+|ResultUnit1|||+|F||||20171019114059|<CR><ETX>46<CR><LF>
            //   <STX>7L|1|N<CR><ETB>1E<CR><LF>
            int startIndex = IndexOfByte(ASTMProtocols.STX);
            if (startIndex < 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error(logType, $"接收数据无STX异常: {Decode(buffer, buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    ResetReceiveBuffer();
                }
                return false;
            }

            if (startIndex > 0)
            {
                buffer.RemoveRange(0, startIndex); // 丢弃 STX 前的粘包前缀
                processedOffset = Math.Max(0, processedOffset - startIndex);
            }

            int consumeLength = 0; // 命中 L 后：buffer[0] 至帧尾的总字节数

            while (processedOffset < buffer.Count)
            {
                if (buffer[processedOffset] != ASTMProtocols.STX)
                    break; // 帧对齐丢失，保留 buffer 等待更多数据或后续重同步

                if (processedOffset + 2 >= buffer.Count)
                    return false; // 半包，如只收到 <STX>1

                // processedOffset+0=<STX> +1=帧号 +2=payload 首字节(H/R/L)
                int payloadStart = processedOffset + 2;
                int frameEndIndex = -1;  // <ETX/ETB>位置

                for (int i = payloadStart; i < buffer.Count; i++)
                {
                    if (buffer[i] == ASTMProtocols.STX)
                        break; // 当前帧尚无 <ETX>/<ETB> 下一帧已开始，视为半包
                    if (ASTMProtocols.IsFrameEnd(buffer[i]))
                    {
                        frameEndIndex = i;
                        break;
                    }
                }

                if (frameEndIndex < 0)
                    return false; // 半包，等待 <ETX>/<ETB>

                int frameTrailerEnd = frameEndIndex + ASTMProtocols.FrameTrailerLength;
                if (frameTrailerEnd > buffer.Count)
                    return false; // 半包，帧尾未收齐

                lock (stateLock)
                {
                    lineState = LineState.Receiving;
                    transport.Send(ASTMProtocols.ACK);
                }
                Logger.Debug(logType, "收到完整帧，回复ACK");
                processedOffset = frameTrailerEnd;

                // 在 [payloadStart, frameEndIndex) 内按 <CR> 切记录，查找 L 记录
                int recordStart = payloadStart;
                for (int i = payloadStart; i < frameEndIndex; i++)
                {
                    if (buffer[i] != ASTMProtocols.CR)
                        continue;

                    int recordLength = i - recordStart;
                    if (ASTMProtocols.IsTerminatorRecord(CollectionsMarshal.AsSpan(buffer).Slice(recordStart, recordLength)))
                    {
                        consumeLength = frameTrailerEnd;
                        break;
                    }

                    recordStart = i + 1;
                }

                if (consumeLength > 0)
                    break;
            }

            if (consumeLength <= 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error(logType, $"接收数据无完整消息(L记录结束)异常: {Decode(buffer, buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    ResetReceiveBuffer();
                }
                return false;
            }

            message = buffer.GetRange(0, consumeLength);
            buffer.RemoveRange(0, consumeLength);
            processedOffset = 0;
            return true;
        }

        private void ResetReceiveBuffer()
        {
            buffer.Clear();
            processedOffset = 0;
        }

        /// <summary>
        /// 提取控制字符
        /// </summary>
        private bool TryExtractControlChar(out byte controlChar)
        {
            controlChar = 0;

            if (buffer.Count == 0 || buffer[0] == ASTMProtocols.STX)
                return false;

            byte value = buffer[0];
            if (value is not (ASTMProtocols.ENQ or ASTMProtocols.ACK or ASTMProtocols.NAK or ASTMProtocols.EOT))
                return false;

            controlChar = value;
            buffer.RemoveAt(0);
            return true;
        }

        /// <summary>
        /// 日志记录完整 ASTM 原始报文（含多帧帧尾）
        /// </summary>
        private void LogCompleteMessage(List<byte> message)
        {
            byte[] rawMessage = message.ToArray();
            Logger.Info(logType, $"串口接收完整消息{Decode(message)}");
            receiveMessageService.Save(_instrumentId, rawMessage).GetAwaiter().GetResult();
            receiveTask.NotifyConsume();
        }

        private int IndexOfByte(byte value, int startIndex = 0)
        {
            for (int i = startIndex; i < buffer.Count; i++)
            {
                if (buffer[i] == value)
                    return i;
            }

            return -1;
        }

        private static string Decode(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        private static string Decode(List<byte> data)
        {
            return Decode(data.ToArray());
        }

        private static string Decode(List<byte> data, int startIndex, int count)
        {
            return count == 0 ? string.Empty : Decode(data.GetRange(startIndex, count).ToArray());
        }

        private List<byte[]> sendFrameList = [];
        private int sendFrameOffset = 0;
        /// <summary>
        /// 发送一个完整帧 (主动向仪器下发数据（下发申请单 / 查询单）这条“发送路径”预留的，而不是接收路径)
        /// 调用方须已持有 stateLock。
        /// </summary>
        private void SendFrameUnlocked()
        {
            switch (lineState)
            {
                case LineState.Idle:
                    sendFrameList = senderTaskHandler.SearchEncoderTask();
                    if (sendFrameList.Count > 0)
                    {
                        sendFrameOffset = 0;
                        transport.Send(ASTMProtocols.ENQ);
                        lineState = LineState.WaitingAck;
                    }
                    break;
                case LineState.WaitingAck:
                    break;
                case LineState.Sending:
                    if (sendFrameList.Count > sendFrameOffset)
                    {
                        byte[] frame = sendFrameList[sendFrameOffset];
                        sendFrameOffset++;
                        transport.Send(frame);
                        Logger.Debug(logType, $"串口发送帧: {Decode(frame)}");
                    }
                    else if (sendFrameList.Count > 0)
                    {
                        senderTaskHandler.Completed(sendFrameList);
                        sendFrameList = [];
                        sendFrameOffset = 0;
                        lineState = LineState.Idle;
                        transport.Send(ASTMProtocols.EOT);
                    }
                    break;
                case LineState.Receiving:
                    break;
                default:
                    break;
            }
        }

        private void CheckReceiveIdleTimeout(object? state)
        {
            try
            {
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (now - lastReceiveTime < ReceiveIdleTimeoutSeconds * 1000L)
                    return;

                lock (stateLock)
                {
                    if (now - lastReceiveTime < ReceiveIdleTimeoutSeconds * 1000L)
                        return;

                    if (lineState != LineState.Idle)
                    {
                        Logger.Info(logType, $"超过{ReceiveIdleTimeoutSeconds}秒未收到消息，重置为Idle并尝试发送");
                        sendFrameList = [];
                        sendFrameOffset = 0;
                    }

                    lineState = LineState.Idle;
                    SendFrameUnlocked();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "空闲超时检查异常", ex);
            }
        }

        public void Stop()
        {
            idleCheckTimer?.Dispose();
            idleCheckTimer = null;
            transport.DataReceived -= Transport_DataReceived;
            receiveTask.Shutdown();
            ResetReceiveBuffer();
            transport.Close();
        }

        public void NotifyLisIssueApplication()
        {
            lisIssueApplication.NotifyConsume();
        }

        public enum LineState
        {
            Idle,
            WaitingAck,
            Sending,
            Receiving
        }
        public interface ISenderTaskHandler
        {
            List<byte[]> SearchEncoderTask();
            void Completed(List<byte[]> sendFrameList);
        }
    }
}