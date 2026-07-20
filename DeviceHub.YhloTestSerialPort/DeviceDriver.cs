using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Template.Template;
using DeviceHub.Template.Template.SerialPort;
using DeviceHub.Utils;
using DeviceHub.YhloTestSerialPort.Handler;
using System.Text;

namespace DeviceHub.YhloTestSerialPort
{
    public class DeviceDriver : ISerialDeviceDriver
    {
        private readonly string logType = nameof(DeviceDriver);
        private IConsumeTask? receiveTask;
        private SerialPortSession session;
        private IConsumeTask? lisIssueApplicationTask;

        public DeviceDriver()
        {
            session = new SerialPortSession();
        }

        public async Task Start(long instrumentId, SerialPortConfig config)
        {
            Encoding encoding = TextEncodings.GetEncoding(config.Encoding);
            receiveTask = new BatchConsumeTask<ReceiveMessage>(new ReceiveHandler(instrumentId, encoding));
            receiveTask.StartConsume();

            await session.Start(instrumentId, config, receiveTask, new SendHandler(instrumentId, encoding));

            lisIssueApplicationTask = new BatchConsumeTask<GetSampleApplyItemOutput>(new LisIssueApplicationHandler(instrumentId, session.SendTask), "lis_issue_application", 30 * 1000);
            lisIssueApplicationTask.StartConsume();

            Logger.Info(logType, $"设备驱动已启动 instrumentId={instrumentId}, port={config.PortName}");
        }

        public void NotifyLisIssueApplication()
        {
            lisIssueApplicationTask?.NotifyConsume();
        }

        public string GetLineStateName()
        {
            return EnumExtensions.GetDescription(session.GetLineStateUnlocked());
        }

        public void Stop()
        {
            receiveTask?.Shutdown();
            session.Stop();
            Logger.Info(logType, $"设备驱动已停止");
        }
    }
}