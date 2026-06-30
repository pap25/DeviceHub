using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Base.Common;
using DeviceHub.Base.Constant;
using DeviceHub.Base.Transports;
using DeviceHub.Lis;
using DeviceHub.Lis.Impl;
using DeviceHub.Yhlo.Handler;
using System.Text;

namespace DeviceHub.Yhlo
{
    public class TcpDriver : ITcpDeviceDriver
    {
        private readonly string logType = nameof(TcpDriver);
        private readonly ILisClient lisClient = LisClient.Instance;
        private TcpServerTransport transport;
        private readonly List<byte> buffer = new();
        private IConsumeTask receiveTask = new BatchConsumeTask<Object>(new ReceiveHandler());
        public async Task<Resp> Start(TcpConfig config)
        {
            transport = new(config.Host, config.Port);
            await transport.StartListeningAsync();

            transport.DataReceived += Transport_DataReceived;

            // 启动消费线程
            receiveTask.StartConsume();

            return Resp.Ok();
        }

        private void Transport_DataReceived(byte[] data)
        {
            try
            {
                buffer.AddRange(data);

                while (TryExtractMessage(out List<byte> message))
                {
                    Logger.Info(logType, $"TCP接收完整消息: {Encoding.UTF8.GetString(message.ToArray())}");

                    //string text = Encoding.UTF8.GetString(message.GetRange(1, message.Count - 1).ToArray());


                    // 添加队列表 receive_message、receive_message_large

                    receiveTask.NotifyConsume();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(logType, $"TCP接收数据处理异常: {Encoding.UTF8.GetString(data)}", ex);
                buffer.Clear();
            }
        }

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
            return transport.GetClientRemoteEndPoint();
        }
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
