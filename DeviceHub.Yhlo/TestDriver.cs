using DeviceHub.Abstractions;
using DeviceHub.Base.Transports;
using System.IO.Ports;

namespace DeviceHub.Yhlo
{
    public class TestDriver: IDeviceDriver
    {
        public void Test()
        {
            SerialPortTransport serialPort = new SerialPortTransport("COM4", 9600, Parity.None, 8, StopBits.One, "\r");
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();
        }

        private void SerialPort_DataReceived(string data)
        {
            Console.WriteLine("数据："+data);
        }
    }
}
