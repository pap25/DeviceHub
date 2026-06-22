using DeviceHub.Abstractions;
using DeviceHub.Base.Common;
using DeviceHub.Base.Constant;
using DeviceHub.Base.Transports;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using System.IO.Ports;
using System.Text;

namespace DeviceHub.Yhlo
{
    public class YhloTestSerialPortDriver : ISerialDeviceDriver
    {
        private readonly ILisClient lisClient = LisClient.Instance;
        private SerialPortTransport transport;
        private readonly List<byte> buffer = new();
        public async Task<Resp> Start(SerialPortConfig config)
        {
            transport = new(
                config.PortName,
                config.BaudRate,
                (Parity)config.Parity,
                config.DataBits,
                (StopBits)config.StopBits
            );
            await transport.OpenAsync();

            transport.DataReceived += Transport_DataReceived;

            return Resp.Ok();
        }

        //// 检测完成后自动推送结果
        //public async Task<Resp> TestResults()
        //{
        //    return Resp.Ok();
        //}

        //// 向 LIS 服务器查询样本申请信息
        //public async Task<Resp> RequestRecord(Object param)
        //{
        //    // lisClient
        //    return Resp.Ok();
        //}

        // LIS 主动推送检验申请单到仪器

        private async void Transport_DataReceived(byte[] data)
        {
            Logger.Info($"串口接收消息: {Encoding.UTF8.GetString(data)}");
            switch (data[0])
            {
                case ASTMProtocols.ENQ:
                    Console.WriteLine("收到 ENQ");
                    await transport.SendAsync(ASTMProtocols.ACK);
                    return;

                case ASTMProtocols.ACK:
                    Console.WriteLine("收到 ACK");
                    return;

                case ASTMProtocols.NAK:
                    Console.WriteLine("收到 NAK");
                    await Task.Delay(10000);
                    await transport.SendAsync(ASTMProtocols.EOT);
                    return;

                case ASTMProtocols.EOT:
                    Console.WriteLine("收到 EOT");
                    return;
            }
        }
    }
}