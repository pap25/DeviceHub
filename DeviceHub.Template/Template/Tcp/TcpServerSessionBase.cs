using DeviceHub.Abstractions.Dto;
using DeviceHub.Template.Constant;
using DeviceHub.Model.Entities;
using DeviceHub.Service;
using DeviceHub.Template.Transports;
using DeviceHub.Utils;
using System.Text;

namespace DeviceHub.Template.Template.Tcp
{
    public abstract class TcpServerSessionBase
    {
        private readonly string logType = nameof(TcpServerSessionBase);
        private IConsumeTask? receiveTask;
        private readonly ReceiveMessageService receiveMessageService = ReceiveMessageService.Instance;
        private long _instrumentId;

        private TcpServerTransport transport;
        private readonly List<byte> buffer = new();
        public void Start(long instrumentId, TcpConfig config, IBatchTaskHandler<ReceiveMessage> receiveHandler)
        {
            this._instrumentId = instrumentId;
            this.transport = new(config.Host, config.Port);
            this.transport.DataReceived += Transport_DataReceived;

            this.receiveTask = new BatchConsumeTask<ReceiveMessage>(receiveHandler);
            this.receiveTask.StartConsume();

            this.transport.StartListening();
        }

        private void Transport_DataReceived(byte[] data)
        {
            try
            {
                Logger.Debug(logType, $"TCP接收消息: {Encoding.UTF8.GetString(data)}");

                buffer.AddRange(data);

                while (TryExtractMessage(out List<byte> message))
                {
                    byte[] rawMessage = message.ToArray();
                    Logger.Info(logType, $"TCP接收完整消息: {Encoding.UTF8.GetString(rawMessage)}");

                    receiveMessageService.Save(_instrumentId, rawMessage).GetAwaiter().GetResult();

                    // 回复 ACK
                    ReplyAck(rawMessage);

                    receiveTask?.NotifyConsume();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(logType, $"TCP接收数据处理异常: {Encoding.UTF8.GetString(data)}", ex);
                buffer.Clear();
            }
        }

        protected void ReplyAck(byte[] rawMessage)
        {
            byte[]? ackMessage = GetReplyAckMessage(rawMessage);
            if (ackMessage is null)
                return;

            transport.SendAsync(ackMessage).GetAwaiter().GetResult();
            Logger.Info(logType, $"TCP回复ACK: {Encoding.UTF8.GetString(ackMessage)}");
        }

        protected abstract byte[]? GetReplyAckMessage(byte[] rawMessage);

        private bool TryExtractMessage(out List<byte> message)
        {
            message = new List<byte>(0);

            int startIndex = IndexOfByte(HL7Protocols.VT);
            if (startIndex < 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error(logType, $"接收数据没VT异常: {Encoding.UTF8.GetString(buffer.ToArray(), buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    buffer.Clear();
                }
                return false; // 半包或垃圾前缀，继续等
            }

            if (startIndex > 0)
                buffer.RemoveRange(0, startIndex); // 移除粘包前面数据

            int endIndex = IndexOfByte(HL7Protocols.EB, 1);
            if (endIndex < 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error(logType, $"接收数据没EB异常: {Encoding.UTF8.GetString(buffer.ToArray(), buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    buffer.Clear();
                }
                return false;  // 半包，继续等
            }

            // 完整消息：VT + 正文 + EB
            message = buffer.GetRange(0, endIndex + 1);

            int consumed = endIndex + 1;
            if (consumed < buffer.Count && buffer[consumed] == HL7Protocols.CR)
                consumed++;

            buffer.RemoveRange(0, consumed);

            return true;
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

        public string GetClientRemoteEndPoint()
        {
            return transport?.GetClientRemoteEndPoint() ?? string.Empty;
        }

        public async Task SendAsync(byte[] data)
        {
            await transport.SendAsync(data);
        }

        public void Stop()
        {
            if (transport != null)
            {
                transport.DataReceived -= Transport_DataReceived;
                transport.Stop();
                transport = null;
            }

            receiveTask?.Shutdown();
            buffer.Clear();

            Logger.Info(logType, "TCP会话已停止");
        }
    }
}
