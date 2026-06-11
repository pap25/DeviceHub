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

            while (TryExtractMessage(out byte[] message))   // 是否有一条完整信息，有的话完整信息返回到message
            {
                string payload = Encoding.UTF8.GetString(message);
                Logger.Info($"TCP接收完整消息: {payload}");
            }
        }

        private bool TryExtractMessage(out byte[] message)
        {
            message = Array.Empty<byte>();

            int start = IndexOfByte(Protocols.VT);
            if (start < 0) // 没有VT数据就有问题了
            {
                Logger.Error($"数据没有VT数据异常了 data:{Encoding.UTF8.GetString(buffer.ToArray())}");
                buffer.Clear();
                return false;
            }

            if (start > 0) // 移除粘包前面数据
                buffer.RemoveRange(0, start);

            int end = IndexOfByte(Protocols.EB, 1);
            if (end < 0) // 半包
            {
                return false;
            }

            // 完整一条信息
            message = buffer.GetRange(1, end).ToArray();
            // 清理掉已完整信息
            buffer.RemoveRange(0, end + 1);

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
