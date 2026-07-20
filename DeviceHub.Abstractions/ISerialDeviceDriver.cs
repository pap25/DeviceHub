using DeviceHub.Abstractions.Dto;

namespace DeviceHub.Abstractions
{
    public interface ISerialDeviceDriver
    {
        Task Start(long instrumentId, SerialPortConfig serialPortConfig);

        void NotifyLisIssueApplication();

        string GetLineStateName();

        void Stop();
    }
}