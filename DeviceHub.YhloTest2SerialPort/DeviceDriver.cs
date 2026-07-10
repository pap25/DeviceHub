using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Utils;
using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.YhloTestV2SerialPort.Handler;

namespace DeviceHub.YhloTestV2SerialPort
{
    public class DeviceDriver : ISerialDeviceDriver
    {
        private readonly string logType = nameof(DeviceDriver);
        private IConsumeTask receiveTask = null!;
        private IConsumeTask lisIssueApplication = null!;
        private SerialPortSession? session;

        public async Task Start(long instrumentId, SerialPortConfig config)
        {
            receiveTask = new BatchConsumeTask<ReceiveMessage>(new ReceiveHandler(instrumentId));
            receiveTask.StartConsume();

            lisIssueApplication = new BatchConsumeTask<GetSampleApplyListOutput>(new LisIssueApplication(instrumentId));
            lisIssueApplication.StartConsume();

            session = new SerialPortSession(instrumentId, receiveTask, new SendHandler(instrumentId));
            await session.Start(config);

            Logger.Info(logType, $"设备驱动已启动 instrumentId={instrumentId}, port={config.PortName}");
        }

        public void Stop()
        {
            receiveTask?.Shutdown();
            session?.Dispose();
            session = null;
            Logger.Info(logType, $"设备驱动已停止");
        }

        public void NotifyLisIssueApplication()
        {
            lisIssueApplication.NotifyConsume();
        }
    }
}
