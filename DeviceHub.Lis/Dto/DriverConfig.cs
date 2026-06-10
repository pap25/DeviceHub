using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Lis.Dto
{
    public class DriverConfig
    {
        public TcpConfig TcpConfig { get; set; }
        public SerialPortConfig SerialPortConfig { get; set; }
    }
}
