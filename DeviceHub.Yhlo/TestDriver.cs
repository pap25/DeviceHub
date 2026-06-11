using DeviceHub.Abstractions;
using DeviceHub.Base.Common;
using DeviceHub.Base.Transports;
using DeviceHub.Lis;
using DeviceHub.Lis.Impl;
using System.IO.Ports;
using System.Text;

namespace DeviceHub.Yhlo
{
    public class TestDriver
    {
        private readonly ILisClient lisClient = LisClient.Instance;
        private readonly StringBuilder _buffer = new();
        public Resp Start()
        {
            SerialPortTransport serialPort;
            try
            {
                serialPort = null;
                //var serialPortConfig = lisClient.queryDriverConfig(1);
                //serialPort = new SerialPortTransport(
                //    serialPortConfig.PortName,
                //    serialPortConfig.BaudRate,
                //    (Parity)serialPortConfig.Parity,
                //    serialPortConfig.DataBits,
                //    (StopBits)serialPortConfig.StopBits
                //);
            }
            catch (Exception ex)
            {
                Logger.Error("从LIS拉取配置失败", ex);
                return Resp.Fail("从LIS拉取配置失败");
            }
            try
            {
                serialPort.DataReceived += OnDataReceived;
                serialPort.OpenAsync();
            }
            catch (Exception ex)
            {
                Logger.Error("打开串口失败", ex);
                return Resp.Fail("打开串口失败！串口不存在或已经打开");
            }
            return Resp.Ok();
        }

        private void OnDataReceived(byte[] data)
        {
            // 国产仪器GBK  新设备UTF8
            string text = TextEncodings.Gbk.GetString(data);
            Logger.Info($"串口接收数据: {text}");
            _buffer.Append(text);
        }
    }
}
