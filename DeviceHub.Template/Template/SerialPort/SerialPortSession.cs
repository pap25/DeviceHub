using DeviceHub.Abstractions.Dto;
using DeviceHub.Template.Constant;
using DeviceHub.Template.Transports;
using DeviceHub.Utils;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;

namespace DeviceHub.Template.Template.SerialPort
{
    public class SerialPortSession : IDisposable
    {
        private readonly string logType = nameof(SerialPortSession);
        private readonly object stateLock = new();
        private SerialPortReceiver receiver = null!;
        private SerialPortSender sender = null!;
        private IConsumeTask sendTask = null!;

        private SerialPortTransport? transport;
        private Encoding messageEncoding = null!;
        private LineState lineState = LineState.Idle;
        private long lastReceiveTime;
        private const int receiveIdleTimeoutSeconds = 20;
        private const int idleCheckIntervalMilliseconds = 5 * 1000;

        /// <summary>供外部（如下发申请）唤起发送循环。</summary>
        public IConsumeTask SendTask => sendTask;

        public async Task Start(
            long instrumentId,
            SerialPortConfig config,
            IConsumeTask receiveTask,
            ISenderSerialPortTaskHandler senderTaskHandler)
        {
            messageEncoding = TextEncodings.GetEncoding(config.Encoding);
            receiver = new SerialPortReceiver(this, instrumentId, receiveTask, messageEncoding);
            sender = new SerialPortSender(this, senderTaskHandler);
            sendTask = new NotifyTask(sender, "serial_port_send", idleCheckIntervalMilliseconds);

            transport = new SerialPortTransport(
                config.PortName,
                config.BaudRate,
                (Parity)config.Parity,
                config.DataBits,
                (StopBits)config.StopBits);

            transport.DataReceived += Transport_DataReceived;

            lastReceiveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Run(transport.Open);
            sendTask.StartConsume();
        }

        /// <summary>在 stateLock 下执行，供 Sender HandleTask 使用。</summary>
        public void ExecuteWithStateLock(Action action)
        {
            lock (stateLock)
            {
                action();
            }
        }

        private void Transport_DataReceived(byte[] data)
        {
            try
            {
                Volatile.Write(ref lastReceiveTime, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                receiver.OnDataReceived(data);
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "串口接收数据处理异常", ex);
                receiver.ResetBuffer();
            }
        }

        /// <summary>
        /// 处理控制字符
        /// </summary>
        public void HandleControlChar(byte controlChar)
        {
            lock (stateLock)
            {
                switch (controlChar)
                {
                    case ASTMProtocols.ENQ:
                        Logger.Info(logType, "收到 ENQ，回复ACK");
                        lineState = LineState.Receiving;
                        SendUnlocked(ASTMProtocols.ACK);
                        return;

                    case ASTMProtocols.ACK:
                        Logger.Debug(logType, "收到 ACK");
                        if (sender.SendFrameList.Count == 0)
                            return;
                        if (lineState is not (LineState.WaitingEnqAck or LineState.WaitingFrameAck))
                            return;
                        lineState = LineState.Sending;
                        sendTask.NotifyConsume();
                        return;

                    case ASTMProtocols.NAK:
                        Logger.Info(logType, "收到 NAK");
                        lineState = LineState.Receiving;
                        return;

                    case ASTMProtocols.EOT:
                        Logger.Info(logType, "收到 EOT，本次传输结束，清理未完成消息");
                        receiver.ResetBuffer();
                        sender.ClearSendFrames();
                        lineState = LineState.Idle;
                        sendTask.NotifyConsume();
                        return;
                }
            }
        }

        public void OnFrameReceived()
        {
            lock (stateLock)
            {
                lineState = LineState.Receiving;
                SendUnlocked(ASTMProtocols.ACK);
            }
            Logger.Debug(logType, "收到完整帧，回复ACK");
        }

        public void SendUnlocked(byte data) => transport?.Send(data);

        public void SendFrameUnlocked(byte[] frame)
        {
            transport?.Send(frame);
            Logger.Debug(logType, $"串口发送帧: {messageEncoding.GetString(frame)}");
        }

        public LineState GetLineStateUnlocked() => lineState;

        public void SetLineStateUnlocked(LineState state)
        {
            this.lineState = state;
            if (state is LineState.WaitingEnqAck or LineState.WaitingFrameAck)
            {
                Volatile.Write(ref lastReceiveTime, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
        }

        /// <summary>检查接收空闲超时。调用方须已持有 stateLock。</summary>
        public void CheckReceiveIdleTimeoutUnlocked()
        {
            if (lineState == LineState.Idle)
                return;

            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (now - Volatile.Read(ref lastReceiveTime) < receiveIdleTimeoutSeconds * 1000L)
                return;

            Logger.Info(logType, $"超过{receiveIdleTimeoutSeconds}秒未收到消息，重置为Idle并尝试发送");
            lineState = LineState.Idle;
        }

        public void Stop()
        {
            sendTask.Shutdown();

            if (transport != null)
            {
                transport.DataReceived -= Transport_DataReceived;
                transport.Close();
            }

            receiver.ResetBuffer();
            sender.ClearSendFrames();
        }

        public void Dispose()
        {
            Stop();
            transport?.Dispose();
            transport = null;
        }
    }

    public enum LineState
    {
        [Description("空闲")]
        Idle,
        [Description("已发ENQ等待ACK")]
        WaitingEnqAck,
        [Description("已发帧等待ACK")]
        WaitingFrameAck,
        [Description("发送数据中")]
        Sending,
        [Description("接收数据中")]
        Receiving
    }
}
