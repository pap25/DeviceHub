using System;

namespace DeviceHub.Lis.Dto
{
    public class SerialPortConfig
    {
        public string? PortName { get; set; }
        public int BaudRate { get; set; }
        public int Parity { get; set; }
        public int DataBits { get; set; }
        public int StopBits { get; set; }
    }
}