using DeviceHub.Lis.Dto;

namespace DeviceHub.Abstractions
{
    public interface IDeviceDriver
    {
        Task<Resp> Start(DriverConfig driverConfig);
    }
}
