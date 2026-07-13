using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using DeviceHub.Service;
using DeviceHub.Utils;
using DeviceHub.YhloTestTcpServer.Protocol;
using System.Text.Json;

namespace DeviceHub.YhloTestTcpServer.Handler
{
    public class SendHandler : IBatchTaskHandler<SendMessage>
    {
        private readonly string logType = nameof(SendHandler);
        private long _instrumentId;
        private readonly SendMessageRepository sendMessageRepository = SendMessageRepository.Instance;
        private readonly SendMessageLargeRepository sendMessageLargeRepository = SendMessageLargeRepository.Instance;
        private readonly SendMessageService sendMessageService = SendMessageService.Instance;

        public SendHandler(long instrumentId)
        {
            _instrumentId = instrumentId;
        }

        public IEnumerable<SendMessage> SearchTask()
        {
            List<SendMessage> taskList = sendMessageRepository
                .FindByInstrumentIdAndStatusOrderAsc(_instrumentId, SendMessage.StatusEnum.Pending, 15).GetAwaiter().GetResult();
            return taskList;
        }
        public void HandleTask(SendMessage task)
        {
            try
            {
                SendMessageLarge? receiveMessageLarge = sendMessageLargeRepository.GetBySendMessageId(task.Id).GetAwaiter().GetResult();
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (receiveMessageLarge == null)
                {
                    MarkFailed(task.Id, "数据异常", now);
                    return;
                }

                if (task.Type == SendMessage.TypeEnum.RequestApplication)
                {
                    GetSampleApplyItemOutput? getSampleApplyItemOutput = JsonSerializer.Deserialize<GetSampleApplyItemOutput>(receiveMessageLarge.SendJson);
                    if (getSampleApplyItemOutput == null)
                    {
                        MarkFailed(task.Id, "数据异常", now);
                        return;
                    }

                    byte[] rawMessage = Hl7MessageEncoder.EncoderRequestApplication(getSampleApplyItemOutput);

                    // 发送
                    sendMessageService.UpdateSuccessRequestApplication(task.Id, rawMessage).GetAwaiter();
                }
                else if (task.Type == SendMessage.TypeEnum.IssueApplication)
                {

                }
                else
                {
                    MarkFailed(task.Id, "不支持的类型 " + task.Type, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                }
            }
            catch (Exception e)
            {
                MarkFailed(task.Id, "HandleTask异常" + e.Message, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
        }

        private void MarkFailed(long id, string errorMessage, long now)
        {
            sendMessageRepository.UpdateStatusAndErrorMessageAndUpdateTimeById(
                id,
                SendMessage.StatusEnum.Failed,
                errorMessage,
                now).GetAwaiter().GetResult();
            Logger.Warn(logType, $"消息处理失败 id={id}: {errorMessage}");
        }
    }
}