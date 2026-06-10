using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Vo;
using DeviceHub.Base.Common;
using DeviceHub.Base.Transports;
using DeviceHub.Base.Transports.DeviceHub.Base.Transports;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace DeviceHub.Yhlo
{
    public class HL7ProtocolYhlo : IDeviceDriver
    {
        private readonly ILisClient lisClient = new LisClient();
        public async Task<Resp> Start(int driverId)
        {
            DriverConfig config;
            try
            {
                config = await lisClient.queryDriverConfig(driverId);
            }
            catch (Exception ex)
            {
                Logger.Error($"从LIS拉取配置失败: driverId={driverId}", ex);
                return Resp.Make($"从LIS拉取配置失败: driverId={driverId}");
            }

            NetworkPortVo networkPortVo = new NetworkPortVo
            {
                Host = config.TcpConfig.Host,
                Port = config.TcpConfig.Port
            };

            TcpTransport tcpTransport = new TcpTransport(networkPortVo.Host, networkPortVo.Port);
            await tcpTransport.ConnectAsync();

            tcpTransport.DataReceived += TcpTransport_DataReceived;

            return Resp.Ok(networkPortVo);
        }

        private void TcpTransport_DataReceived(byte[] obj)
        {
            throw new NotImplementedException();
        }
    }
}
