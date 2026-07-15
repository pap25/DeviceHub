using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Template.Template;
using DeviceHub.Template.Template.SerialPort;
using DeviceHub.Utils;
using DeviceHub.YhloTestSerialPort.Handler;

namespace DeviceHub.YhloTestSerialPort
{
    public class DeviceDriver : ISerialDeviceDriver
    {
        private readonly string logType = nameof(DeviceDriver);
        private IConsumeTask? receiveTask;
        private SerialPortSession? session;
        private IConsumeTask? lisIssueApplicationTask;

        public async Task Start(long instrumentId, SerialPortConfig config)
        {
            receiveTask = new BatchConsumeTask<ReceiveMessage>(new ReceiveHandler(instrumentId));
            receiveTask.StartConsume();

            session = new SerialPortSession(instrumentId, receiveTask, new SendHandler(instrumentId));
            await session.Start(config);

            lisIssueApplicationTask = new BatchConsumeTask<GetSampleApplyItemOutput>(new LisIssueApplicationHandler(instrumentId, null));
            lisIssueApplicationTask.StartConsume();

            Logger.Info(logType, $"设备驱动已启动 instrumentId={instrumentId}, port={config.PortName}");
        }

        public void NotifyLisIssueApplication()
        {
            lisIssueApplicationTask?.NotifyConsume();
        }

        public void Stop()
        {
            receiveTask?.Shutdown();
            session?.Stop();
            Logger.Info(logType, $"设备驱动已停止");
        }
    }
}