using DeviceHub.Abstractions.Dto;

namespace DeviceHub.Abstractions
{
    public interface ITcpDeviceDriver
    {
        Task Start(long instrumentId, TcpConfig tcpConfig);

        void NotifyLisIssueApplication();

        string GetClientRemoteEndPoint();
        void Stop();
    }
}
