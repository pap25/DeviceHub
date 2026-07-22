using DeviceHub.Abstractions.Dto;

namespace DeviceHub.Abstractions
{
    public interface ISerialDeviceDriver : IDeviceDriver
    {
        Task Start(long instrumentId, SerialPortConfig serialPortConfig);

        string GetLineStateName();

        void Stop();
    }
}