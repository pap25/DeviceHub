using DeviceHub.Abstractions.Dto;
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
            sender.Start();
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
                        if (lineState is LineState.WaitingAck or LineState.Sending)
                        {
                            sender.AbortCurrentTransmission();
                        }
                        lineState = LineState.Receiving;
                        sender.EnqueueControl(ASTMProtocols.ACK);
                        return;

                    case ASTMProtocols.ACK:
                        Logger.Debug(logType, "收到 ACK");
                        if (lineState is not (LineState.WaitingAck or LineState.Sending))
                            return;
                        sender.NotifyAck();
                        return;

                    case ASTMProtocols.NAK:
                        Logger.Info(logType, "收到 NAK");
                        if (lineState is LineState.WaitingAck or LineState.Sending)
                        {
                            sender.NotifyNak();
                        }
                        return;

                    case ASTMProtocols.EOT:
                        Logger.Info(logType, "收到 EOT，本次传输结束，清理未完成消息");
                        receiver.ResetBuffer();
                        if (lineState is LineState.WaitingAck or LineState.Sending)
                        {
                            sender.AbortCurrentTransmission();
                        }
                        lineState = LineState.Idle;
                        sender.WakeUp();
                        return;
                }
            }
        }

        public void OnFrameReceived()
        {
            lock (stateLock)
            {
                lineState = LineState.Receiving;
                sender.EnqueueControl(ASTMProtocols.ACK);
            }
            Logger.Debug(logType, "收到完整帧，回复ACK");
        }

        public void WriteToTransport(byte data) => transport?.Send(data);

        public void WriteToTransport(byte[] frame)
        {
            transport?.Send(frame);
            Logger.Debug(logType, $"串口发送帧: {SerialPortReceiver.Decode(frame)}");
        }

        public bool TryBeginOutgoingTransmission()
        {
            lock (stateLock)
            {
                if (lineState != LineState.Idle)
                {
                    return false;
                }

                lineState = LineState.WaitingAck;
                lastReceiveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                return true;
            }
        }

        public bool TryMarkOutgoingSending()
        {
            lock (stateLock)
            {
                if (lineState != LineState.WaitingAck)
                {
                    return lineState == LineState.Sending;
                }

                lineState = LineState.Sending;
                return true;
            }
        }

        public void ReleaseOutgoingTransmission()
        {
            lock (stateLock)
            {
                if (lineState is LineState.WaitingAck or LineState.Sending)
                {
                    lineState = LineState.Idle;
                }
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

                    if (lineState == LineState.Receiving)
                    {
                        Logger.Info(logType, $"超过{receiveIdleTimeoutSeconds}秒未收到消息，重置为Idle并尝试发送");
                        receiver.ResetBuffer();
                        lineState = LineState.Idle;
                        sender.WakeUp();
                    }
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

            sender.Stop();

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
