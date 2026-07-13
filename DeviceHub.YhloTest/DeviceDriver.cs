using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Base.Transports;
using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Utils;
using DeviceHub.YhloTestTcpServer.Handler;

namespace DeviceHub.YhloTestTcpServer
{
    public class DeviceDriver : ITcpDeviceDriver
    {
        private readonly string logType = nameof(DeviceDriver);
        private IConsumeTask receiveTask = null!;
        private IConsumeTask lisIssueApplication = null!;
        private TcpServerReceiver? receiver;
        private TcpServerTransport transport;

        public async Task Start(long instrumentId, TcpConfig config)
        {
            receiveTask = new BatchConsumeTask<ReceiveMessage>(new ReceiveHandler(instrumentId));
            receiveTask.StartConsume();

            lisIssueApplication = new BatchConsumeTask<GetSampleApplyListOutput>(new LisIssueApplication(instrumentId));
            lisIssueApplication.StartConsume();

            receiver = new TcpServerReceiver(instrumentId, receiveTask);

            transport = new(config.Host, config.Port);
            transport.DataReceived += receiver.Transport_DataReceived;
            await transport.StartListeningAsync();

            Logger.Info(logType, $"设备驱动已启动 instrumentId={instrumentId}, port={config.PortName}");
        }

        public void Stop()
        {
            
        }

        public void NotifyLisIssueApplication()
        {
            lisIssueApplication.NotifyConsume();
        }

        public string GetClientRemoteEndPoint()
        {
            return transport.GetClientRemoteEndPoint();
        }
    }
}
