using DeviceHub.Abstractions.Dto;

namespace DeviceHub.Abstractions
{
    public interface ISerialDeviceDriver
    {
        Task<Resp> Start(long instrumentId, SerialPortConfig serialPortConfig);
        void Stop();
    }
}