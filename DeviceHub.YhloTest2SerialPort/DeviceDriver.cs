using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Base.Common;
using DeviceHub.Lis.Dto;
using DeviceHub.YhloTest2SerialPort.Handler;

namespace DeviceHub.YhloTest2SerialPort
{
    public class DeviceDriver : ISerialDeviceDriver
    {
        private readonly string logType = nameof(DeviceDriver);
        private long _instrumentId;
        private IConsumeTask lisIssueApplication;

        public async Task Start(long instrumentId, SerialPortConfig config)
        {
            _instrumentId = instrumentId;
            lisIssueApplication = new BatchConsumeTask<GetSampleApplyListOutput>(new LisIssueApplication(instrumentId));
            lisIssueApplication.StartConsume();
        }

        public void Stop()
        {

        }

        public void NotifyLisIssueApplication()
        {
            lisIssueApplication.NotifyConsume();
        }
    }
}