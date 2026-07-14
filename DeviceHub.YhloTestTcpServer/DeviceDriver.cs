using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Utils;

namespace DeviceHub.YhloTestTcpServer
{
    public class DeviceDriver : ITcpDeviceDriver
    {
        private readonly string logType = nameof(DeviceDriver);
        private TcpServerSession session;

        public DeviceDriver()
        {
            session = new TcpServerSession();
        }

        public async Task Start(long instrumentId, TcpConfig config)
        {
            await session.Start(instrumentId, config);

            Logger.Info(logType, $"设备驱动已启动 instrumentId={instrumentId}, host={config.Host}, port={config.Port}");
        }

        public void Stop()
        {
            session?.Stop();
            Logger.Info(logType, "设备驱动已停止");
        }

        public string GetClientRemoteEndPoint()
        {
            return session.GetClientRemoteEndPoint();
        }
    }
}
