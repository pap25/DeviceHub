using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Base.Common;
using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.YhloTest2SerialPort.Handler;

namespace DeviceHub.YhloTest2SerialPort
{
    public class DeviceDriver : ISerialDeviceDriver
    {
        private readonly string logType = nameof(DeviceDriver);
        private long _instrumentId;
        private IConsumeTask receiveTask = null!;
        private IConsumeTask lisIssueApplication = null!;
        private SerialPortSession? session;

        public async Task Start(long instrumentId, SerialPortConfig config)
        {
            _instrumentId = instrumentId;

            receiveTask = new BatchConsumeTask<ReceiveMessage>(new ReceiveHandler(instrumentId));
            receiveTask.StartConsume();

            lisIssueApplication = new BatchConsumeTask<GetSampleApplyListOutput>(new LisIssueApplication(instrumentId));
            lisIssueApplication.StartConsume();

            ISenderTaskHandler senderTaskHandler = new SendHandler(instrumentId);

            session = new SerialPortSession(instrumentId, receiveTask, senderTaskHandler);
            await session.Start(config);

            Logger.Info(logType, $"设备驱动已启动 instrumentId={instrumentId}, port={config.PortName}");
        }

        public void Stop()
        {
            receiveTask?.Shutdown();
            session?.Stop();
            session?.Dispose();
            session = null;
            Logger.Info(logType, $"设备驱动已停止 instrumentId={_instrumentId}");
        }

        public void NotifyLisIssueApplication()
        {
            lisIssueApplication.NotifyConsume();
        }
    }
}
