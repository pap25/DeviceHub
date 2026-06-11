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
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviceHub.Yhlo
{
    public class HL7ProtocolYhlo : IDeviceDriver
    {
        private readonly ILisClient lisClient = new LisClient();
        private readonly StringBuilder buffer = new();
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

            Logger.Info($"从LIS领取配置成功: {config}");

            NetworkPortVo networkPortVo = new NetworkPortVo
            {
                Host = config.TcpConfig.Host,
                Port = config.TcpConfig.Port
            };

            TcpServerTransport tcpServerTransport = new TcpServerTransport(networkPortVo.Host, networkPortVo.Port);
            await tcpServerTransport.StartAsync();

            tcpServerTransport.DataReceived += TcpTransport_DataReceived;

            return Resp.Ok(networkPortVo);
        }

        private void TcpTransport_DataReceived(byte[] data)
        {
            

            
        }
    }
}
