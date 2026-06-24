using DeviceHub.Lis.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Abstractions
{
    public interface ITcpDeviceDriver
    {
        Task<Resp> Start(TcpConfig tcpConfig);
        void Stop();
    }
}
