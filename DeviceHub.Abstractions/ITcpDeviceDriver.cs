using DeviceHub.Abstractions.Dto;

namespace DeviceHub.Abstractions
{
    public interface ITcpDeviceDriver
    {
        Task<Resp> Start(long instrumentId, TcpConfig tcpConfig);

        string GetClientRemoteEndPoint();
        void Stop();
    }
}
