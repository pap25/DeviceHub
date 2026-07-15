using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Template.Template;
using DeviceHub.Utils;
using DeviceHub.YhloTestTcpServer.Handler;

namespace DeviceHub.YhloTestTcpServer
{
    public class DeviceDriver : ITcpDeviceDriver
    {
        private readonly string logType = nameof(DeviceDriver);
        private readonly TcpServerSession session;
        private IConsumeTask? sendTask;
        private IConsumeTask? lisIssueApplicationTask;

        public DeviceDriver()
        {
            session = new TcpServerSession();
        }

        public Task Start(long instrumentId, TcpConfig config)
        {
            ReceiveHandler receiveHandler = new(instrumentId);
            IConsumeTask receiveTask = new BatchConsumeTask<ReceiveMessage>(receiveHandler);
            receiveTask.StartConsume();

            session.Start(instrumentId, config, receiveTask);

            sendTask = new BatchConsumeTask<SendMessage>(new SendHandler(instrumentId, session));
            sendTask.StartConsume();

            receiveHandler.SendTask = sendTask;

            lisIssueApplicationTask = new BatchConsumeTask<GetSampleApplyItemOutput>(new LisIssueApplicationHandler(instrumentId, sendTask));
            lisIssueApplicationTask.StartConsume();

            Logger.Info(logType, $"设备驱动已启动 instrumentId={instrumentId}, host={config.Host}, port={config.Port}");

            return Task.CompletedTask;
        }

        public void NotifyLisIssueApplication()
        {
            lisIssueApplicationTask?.NotifyConsume();
        }

        public string GetClientRemoteEndPoint()
        {
            return session.GetClientRemoteEndPoint();
        }

        public void Stop()
        {
            session?.Stop();
            Logger.Info(logType, "设备驱动已停止");
        }
    }
}
