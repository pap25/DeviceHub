using DeviceHub.Base.Common;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using DeviceHub.Service;
using System.Text.Json;

namespace DeviceHub.YhloTestSerialPort.Handler
{
    public class LisIssueApplication : IBatchTaskHandler<GetSampleApplyListOutput>
    {
        private readonly string logType = nameof(SendHandler);
        private long _instrumentId;
        private readonly ILisClient lisClient = LisClient.Instance;
        private readonly SendMessageService sendMessageService = SendMessageService.Instance;

        public LisIssueApplication(long instrumentId)
        {
            _instrumentId = instrumentId;
        }

        public IEnumerable<GetSampleApplyListOutput> SearchTask()
        {
            long lastId = 0;
            return lisClient.GetSampleApplyList(_instrumentId, lastId, 20).GetAwaiter().GetResult();
        }
        public void HandleTask(GetSampleApplyListOutput task)
        {
            sendMessageService.SaveIssueApplication(task.Id, _instrumentId, "", "", "", JsonSerializer.Serialize(task));
        }
    }
}
