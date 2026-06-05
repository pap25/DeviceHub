using DeviceHub.Abstractions;
using DeviceHub.Base.Common;
using DeviceHub.Base.Transports;
using DeviceHub.Lis;
using System.IO.Ports;
using System.Text;

namespace DeviceHub.Yhlo
{
    public class TestDriver : IDeviceDriver
    {
        private readonly LisClient lisClient = new();
        private readonly StringBuilder _buffer = new();
        public string Test()
        {
            SerialPortTransport serialPort;
            try
            {
                var serialPortConfig = lisClient.querySerialPortConfigById(1);
                serialPort = new SerialPortTransport(
                    serialPortConfig.PortName,
                    serialPortConfig.BaudRate,
                    (Parity)serialPortConfig.Parity,
                    serialPortConfig.DataBits,
                    (StopBits)serialPortConfig.StopBits
                );
            }
            catch (Exception ex)
            {
                Logger.Error("从LIS拉取配置失败", ex);
                return "从LIS拉取配置失败";
            }
            try
            {
                serialPort.DataReceived += OnDataReceived;
                serialPort.OpenAsync();
            }
            catch (Exception ex)
            {
                Logger.Error("打开串口失败", ex);
                return "打开串口失败！串口不存在或已经打开。";
            }
            return "";
        }

        private void OnDataReceived(byte[] data)
        {
            string text = Encoding.ASCII.GetString(data);
            Logger.Info($"串口接收数据: {text}");
            Logger.Info($"串口接收数据: {Encoding.UTF8.GetString(data)}");
            _buffer.Append(text);
        }
    }
}
