using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Lis.Dto;
using DeviceHub.Utils;
using DeviceHub.YhloTestTcpServer.Handler;

namespace DeviceHub.YhloTestTcpServer
{
    public class DeviceDriver : ITcpDeviceDriver
    {
        private readonly string logType = nameof(DeviceDriver);
        private IConsumeTask lisIssueApplication = null!;
        private TcpServerSession session;

        public DeviceDriver()
        {
            session = new TcpServerSession();
        }

        public async Task Start(long instrumentId, TcpConfig config)
        {
            lisIssueApplication = new BatchConsumeTask<GetSampleApplyListOutput>(new LisIssueApplication(instrumentId));
            lisIssueApplication.StartConsume();

            await session.Start(instrumentId, config);

            Logger.Info(logType, $"设备驱动已启动 instrumentId={instrumentId}, host={config.Host}, port={config.Port}");
        }

        public void Stop()
        {
            session?.Stop();
            lisIssueApplication?.Shutdown();
            Logger.Info(logType, "设备驱动已停止");
        }

        public void NotifyLisIssueApplication()
        {
            lisIssueApplication.NotifyConsume();
        }

        public string GetClientRemoteEndPoint()
        {
            return session.GetClientRemoteEndPoint();
        }
    }
}
