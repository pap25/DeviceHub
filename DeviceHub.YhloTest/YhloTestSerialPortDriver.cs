using DeviceHub.Abstractions;
using DeviceHub.Base.Common;
using DeviceHub.Base.Constant;
using DeviceHub.Base.Transports;
using DeviceHub.Lis.Dto;
using System.IO.Ports;
using System.Text;

namespace DeviceHub.Yhlo
{
    public class YhloTestSerialPortDriver : ISerialDeviceDriver
    {
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