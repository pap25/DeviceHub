using DeviceHub.Abstractions.Vo;

namespace DeviceHub.Abstractions
{
    public interface IDeviceDriver
    {
        Task<Resp> Start(int driverId);
    }
}
