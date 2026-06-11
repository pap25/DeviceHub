using DeviceHub.Abstractions;
using DeviceHub.Base.Common;
using DeviceHub.Base.Constant;
using DeviceHub.Base.Transports;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using System.Text;

namespace DeviceHub.Yhlo.yhloTest
{
    public class YhloTestDriver : IDeviceDriver
    {
        private readonly ILisClient lisClient = LisClient.Instance;
        private readonly List<byte> buffer = new();
        public async Task<Resp> Start(DriverConfig config)
        {
            if (config.TcpConfig != null)
            {
                TcpServerTransport tcpServerTransport = new(config.TcpConfig.Host, config.TcpConfig.Port);
                await tcpServerTransport.StartAsync();

                tcpServerTransport.DataReceived += TcpTransport_DataReceived;

                return Resp.Ok();
            }
            else if (config.SerialPortConfig != null)
            {
                return Resp.Ok();
            }

            return Resp.Fail("无效配置");
        }

        private void TcpTransport_DataReceived(byte[] data)
        {
            try
            {
                buffer.AddRange(data);

                while (TryExtractMessage(out List<byte> message))
                {
                    Logger.Info($"TCP接收完整消息: {Encoding.UTF8.GetString(message.ToArray())}");

                    string text = Encoding.UTF8.GetString(message.GetRange(1, message.Count - 1).ToArray());
                    
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"TCP接收数据处理异常: {Encoding.UTF8.GetString(data)}", ex);
                buffer.Clear();
            }
        }

        private bool TryExtractMessage(out List<byte> message)
        {
            message = new List<byte>(0);

            int startIndex = IndexOfByte(Protocols.VT);
            if (startIndex < 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error($"接收数据没VT异常: {Encoding.UTF8.GetString(buffer.ToArray(), buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    buffer.Clear();
                }
                return false; // 半包或垃圾前缀，继续等
            }

            if (startIndex > 0)
                buffer.RemoveRange(0, startIndex); // 移除粘包前面数据

            int endIndex = IndexOfByte(Protocols.EB, 1);
            if (endIndex < 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error($"接收数据没EB异常: {Encoding.UTF8.GetString(buffer.ToArray(), buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    buffer.Clear();
                }
                return false;  // 半包，继续等
            }

            // 完整消息：VT + 正文 + EB
            message = buffer.GetRange(0, endIndex + 1);

            int consumed = endIndex + 1;
            if (consumed < buffer.Count && buffer[consumed] == Protocols.CR)
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
    }
}
