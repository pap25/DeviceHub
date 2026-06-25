using DeviceHub.Abstractions.Dto;

namespace DeviceHub.Abstractions
{
    public interface ISerialDeviceDriver
    {
        Task<Resp> Start(SerialPortConfig serialPortConfig);
        void Stop();
    }
}