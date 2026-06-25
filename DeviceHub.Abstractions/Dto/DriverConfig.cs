using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Abstractions.Dto
{
    public class DriverConfig
    {
        public TcpConfig? TcpConfig { get; set; }
        public SerialPortConfig? SerialPortConfig { get; set; }
    }
}
