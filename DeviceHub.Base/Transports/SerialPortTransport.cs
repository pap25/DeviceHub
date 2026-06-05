using DeviceHub.Base.Common;
using System.IO.Ports;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviceHub.Base.Transports
{
    public delegate void SerialDataReceivedEventHandler(string data);
    public class SerialPortTransport
    {
        private readonly SerialPort _serialPort;
        private readonly StringBuilder _buffer = new();
        private readonly string[] _endSymbols;
        public event SerialDataReceivedEventHandler DataReceived;

        public SerialPortTransport(string portName, int baudRate = 9600, Parity parity = Parity.None,
            int dataBits = 8, StopBits stopBits = StopBits.One, params string[] endSymbols)
        {
            this._serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
            {
                Encoding = Encoding.UTF8
            };

            this._serialPort.DataReceived += SerialPort_DataReceived;
            this._endSymbols = endSymbols;
            Logger.Info(
                $"初始化串口: portName={portName}, baudRate={baudRate}, parity={parity}, " +
                $"dataBits={dataBits}, stopBits={stopBits}, endSymbols=[{string.Join(", ", endSymbols)}]");
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = _serialPort.ReadExisting();

                _buffer.Append(data);

                Logger.Debug($"串口DataReceived事件: {data}");

                if (isEnd(data))
                {

                    DataReceived(_buffer.ToString());

                    _buffer.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("串口DataReceived事件异常", ex);
            }
        }

        private bool isEnd(string data)
        {
            if (_endSymbols.Length == 0)
            {
                return true;
            }
            foreach (var item in _endSymbols)
            {
                if (data.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();

                Logger.Info($"串口已打开: {_serialPort.PortName}");
            }
        }

        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();

                Logger.Info($"串口已关闭: {_serialPort.PortName}");
            }
        }
    }
}
