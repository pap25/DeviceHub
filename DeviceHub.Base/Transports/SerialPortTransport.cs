using DeviceHub.Base.Common;
using System.IO.Ports;
using System.Text;

namespace DeviceHub.Base.Transports
{
    public class SerialPortTransport : IDisposable
    {
        private readonly string logType = nameof(SerialPortTransport);
        private readonly SerialPort _serialPort;
        public event Action<byte[]>? DataReceived;

        public bool IsOpen => _serialPort.IsOpen;

        public SerialPortTransport(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);

            _serialPort.DataReceived += SerialPort_DataReceived;

            Logger.Info(logType, $"初始化串口 portName:{portName}, baudRate:{baudRate}, parity:{parity}, " +
                $"dataBits:{dataBits}, stopBits:{stopBits}");
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
                Logger.Info(logType, $"串口已打开: {_serialPort.PortName}");
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                Logger.Info(logType, $"串口已关闭: {_serialPort.PortName}");
            }
        }

        /// <summary>
        /// 发送字节
        /// </summary>
        public void Send(byte[] data)
        {
            if (!_serialPort.IsOpen)
            {
                throw new InvalidOperationException("串口未打开");
            }

            _serialPort.Write(data, 0, data.Length);
        }

        public void Send(byte data)
        {
            Send([data]);
        }

        /// <summary>
        /// 发送字符串
        /// </summary>
        public void Send(string message, Encoding? encoding = null)
        {
            encoding ??= Encoding.ASCII;

            Send(encoding.GetBytes(message));
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int length = _serialPort.BytesToRead;

                if (length <= 0)
                {
                    return;
                }

                byte[] buffer = new byte[length];

                _serialPort.Read(
                    buffer,
                    0,
                    length);

                DataReceived?.Invoke(buffer);
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "串口DataReceived事件异常", ex);
            }
        }

        public void Dispose()
        {
            _serialPort.DataReceived -= SerialPort_DataReceived;

            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }

            _serialPort.Dispose();

            Logger.Info(logType, $"串口 Dispose: {_serialPort.PortName}");
        }
    }
}
