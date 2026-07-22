using DeviceHub.Abstractions.Dto;

namespace DeviceHub.Abstractions
{
    public interface ITcpDeviceDriver : IDeviceDriver
    {
        Task Start(long instrumentId, TcpConfig tcpConfig);

        string GetClientRemoteEndPoint();

        void Stop();
    }
}
