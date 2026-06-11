using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Vo;
using DeviceHub.Base.Common;
using DeviceHub.Base.Constant;
using DeviceHub.Base.Transports;
using DeviceHub.Base.Transports.DeviceHub.Base.Transports;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace DeviceHub.Yhlo
{
    public class HL7ProtocolYhlo : IDeviceDriver
    {
        private readonly ILisClient lisClient = new LisClient();
        private readonly List<byte> buffer = new();
        public async Task<Resp> Start(int driverId)
        {
            Logger.Info($"=====================start driverId:{driverId}======================");

            DriverConfig config;
            try
            {
                config = await lisClient.queryDriverConfig(driverId);
            }
            catch (Exception ex)
            {
                Logger.Error($"从LIS拉取配置失败 driverId:{driverId}", ex);
                return Resp.Make($"从LIS拉取配置失败 driverId:{driverId}");
            }

            Logger.Info($"从LIS领取配置成功: {JsonSerializer.Serialize(config)}");

            NetworkPortVo networkPortVo = new NetworkPortVo
            {
                Host = config.TcpConfig.Host,
                Port = config.TcpConfig.Port
            };

            TcpServerTransport tcpServerTransport = new(networkPortVo.Host, networkPortVo.Port);
            await tcpServerTransport.StartAsync();

            tcpServerTransport.DataReceived += TcpTransport_DataReceived;

            return Resp.Ok(networkPortVo);
        }

        private void TcpTransport_DataReceived(byte[] data)
        {
            // 把收到消息拼上来后面再验证
            buffer.AddRange(data);

            while (TryExtractMessage(out byte[] message))  // 是否有一条完整信息，有的话完整信息返回到message
            {
                string text = Encoding.UTF8.GetString(message);
                Logger.Info($"TCP接收完整消息: {text}");
            }
        }

        private bool TryExtractMessage(out byte[] message)
        {
            message = Array.Empty<byte>();

            int startIndex = IndexOfByte(Protocols.VT);
            if (startIndex < 0)
            {
                if (buffer.Count > Constants.MaxReceiveSize)
                {
                    Logger.Info($"接收数据没VT异常: {Encoding.UTF8.GetString(buffer.ToArray(), buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    buffer.Clear();
                }
                return false; // 半包或垃圾前缀，继续等
            }

            if (startIndex > 0)
                buffer.RemoveRange(0, startIndex); // 移除粘包前面数据

            int endIndex = IndexOfByte(Protocols.EB, 1);
            if (endIndex < 0)
            {
                return false;  // 半包，继续等
            }

            // 完整消息：VT + 正文 + EB
            message = buffer.GetRange(0, endIndex + 1).ToArray();

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
