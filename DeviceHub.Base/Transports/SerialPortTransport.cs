using DeviceHub.Base.Common;
using System.IO.Ports;
using System.Text;

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
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = _serialPort.ReadExisting();

                _buffer.Append(data);

                Console.WriteLine("======= RECEIVE =======");
                Console.WriteLine(data);

                if (isEnd(data))
                {

                    DataReceived(_buffer.ToString());

                    _buffer.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常：" + ex.Message);
            }
        }

        private bool isEnd(String data)
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

                Logger.Info("串口已打开" + _serialPort.PortName);
            }
        }

        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();

                Console.WriteLine("串口已关闭");
            }
        }

        //private void ParseMessage(string msg)
        //{
        //    // 去掉MLLP
        //    msg = msg.TrimStart((char)0x0B)
        //             .TrimEnd((char)0x1C, (char)0x0D);

        //    Console.WriteLine("======= FULL HL7 =======");
        //    Console.WriteLine(msg);

        //    /*HL7Parser parser = new();

        //    parser.Parse(msg);*/
        //}

        //private void SendACK()
        //{
        //    // ASTM ACK
        //    byte[] ack = { 0x06 };

        //    _serialPort.Write(ack, 0, ack.Length);

        //    Console.WriteLine("======= SEND ACK =======");
        //}

        //public void Send(string msg)
        //{
        //    if (!_serialPort.IsOpen)
        //        return;

        //    byte[] data = BuildMLLP(msg);

        //    _serialPort.Write(data, 0, data.Length);

        //    Console.WriteLine("======= SEND =======");
        //    Console.WriteLine(msg);
        //}

        //private byte[] BuildMLLP(string msg)
        //{
        //    List<byte> bytes = new();

        //    bytes.Add(0x0B);

        //    bytes.AddRange(Encoding.UTF8.GetBytes(msg));

        //    bytes.Add(0x1C);
        //    bytes.Add(0x0D);

        //    return bytes.ToArray();
        //}
    }
}
