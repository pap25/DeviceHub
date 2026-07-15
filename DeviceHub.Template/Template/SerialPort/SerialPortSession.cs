using DeviceHub.Abstractions.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Template.Constant;
using DeviceHub.Template.Transports;
using DeviceHub.Utils;
using System.IO.Ports;

namespace DeviceHub.Template.Template.SerialPort
{
    public class SerialPortSession : IDisposable
    {
        private readonly string logType = nameof(SerialPortSession);
        private readonly object stateLock = new();
        private readonly SerialPortReceiver receiver;
        private readonly SerialPortSender sender;

        private SerialPortTransport? transport;
        private LineState lineState = LineState.Idle;
        private long lastReceiveTime;
        private Timer? idleCheckTimer;
        private const int receiveIdleTimeoutSeconds = 15;

        public SerialPortSession(long instrumentId, IConsumeTask receiveTask, ISenderSerialPortTaskHandler senderTaskHandler)
        {
            receiver = new SerialPortReceiver(this, instrumentId, receiveTask);
            sender = new SerialPortSender(this, senderTaskHandler);
        }

        public async Task Start(SerialPortConfig config)
        {
            transport = new SerialPortTransport(
                config.PortName,
                config.BaudRate,
                (Parity)config.Parity,
                config.DataBits,
                (StopBits)config.StopBits);

            transport.DataReceived += Transport_DataReceived;

            lastReceiveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            idleCheckTimer = new Timer(
                CheckReceiveIdleTimeout,
                null,
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(5));

            await Task.Run(transport.Open);
        }

        private void Transport_DataReceived(byte[] data)
        {
            try
            {
                lastReceiveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
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
                        if (lineState is not (LineState.WaitingAck or LineState.Sending))
                            return;
                        lineState = LineState.Sending;
                        sender.TrySendNextUnlocked();
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
                        sender.TrySendNextUnlocked();
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
            Logger.Debug(logType, $"串口发送帧: {SerialPortReceiver.Decode(frame)}");
        }

        public LineState GetLineStateUnlocked() => lineState;

        public void SetLineStateUnlocked(LineState state)
        {
            this.lineState = state;
            if (state == LineState.WaitingAck)
            {
                lastReceiveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }

        private void CheckReceiveIdleTimeout(object? state)
        {
            try
            {
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (now - lastReceiveTime < receiveIdleTimeoutSeconds * 1000L)
                    return;

                lock (stateLock)
                {
                    if (now - lastReceiveTime < receiveIdleTimeoutSeconds * 1000L)
                        return;

                    if (lineState != LineState.Idle)
                    {
                        Logger.Info(logType, $"超过{receiveIdleTimeoutSeconds}秒未收到消息，重置为Idle并尝试发送");
                        sender.ClearSendFrames();
                    }

                    lineState = LineState.Idle;
                    sender.TrySendNextUnlocked();
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
        Idle,
        WaitingAck,
        Sending,
        Receiving
    }
}
