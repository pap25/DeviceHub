using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using DeviceHub.Service;
using DeviceHub.Utils;
using System.Text.Json;

namespace DeviceHub.Template.Template
{
    public abstract class SendHandlerBase : IBatchTaskHandler<SendMessage>
    {
        private readonly string logType = nameof(SendHandlerBase);
        private long _instrumentId;
        private readonly SendMessageRepository sendMessageRepository = SendMessageRepository.Instance;
        private readonly SendMessageLargeRepository sendMessageLargeRepository = SendMessageLargeRepository.Instance;
        private readonly SendMessageService sendMessageService = SendMessageService.Instance;

        public SendHandlerBase(long instrumentId)
        {
            this._instrumentId = instrumentId;
        }

        public IEnumerable<SendMessage> SearchTask()
        {
            List<SendMessage> taskList = sendMessageRepository
                .FindByInstrumentIdAndStatusOrderAsc(_instrumentId, SendMessage.StatusEnum.Pending, 15).GetAwaiter().GetResult();
            if (taskList.Count > 0)
                Logger.Debug(logType, $"查询待发送消息 {taskList.Count} 条");
            return taskList;
        }
        public void HandleTask(SendMessage task)
        {
            try
            {
                SendMessageLarge? receiveMessageLarge = sendMessageLargeRepository.GetBySendMessageId(task.Id).GetAwaiter().GetResult();
                if (receiveMessageLarge == null)
                {
                    MarkFailed(task.Id, "数据异常");
                    return;
                }

                switch (task.Type)
                {
                    case SendMessage.TypeEnum.RequestApplication:
                        GetSampleApplyItemOutput? getSampleApplyItemOutput = JsonSerializer.Deserialize<GetSampleApplyItemOutput>(receiveMessageLarge.SendJson);
                        if (getSampleApplyItemOutput == null)
                        {
                            MarkFailed(task.Id, "数据异常");
                            return;
                        }

                        byte[] rawMessage = EncoderRequestApplicationSend(getSampleApplyItemOutput);

                        sendMessageService.UpdateSuccessRequestApplication(task.Id, rawMessage).GetAwaiter().GetResult();
                        Logger.Debug(logType, $"待发送请求查询申请信息处理成功 id={task.Id}");
                        break;
                    case SendMessage.TypeEnum.IssueApplication:
                        GetSampleApplyListOutput? getSampleApplyListOutput = JsonSerializer.Deserialize<GetSampleApplyListOutput>(receiveMessageLarge.SendJson);
                        if (getSampleApplyListOutput == null)
                        {
                            MarkFailed(task.Id, "SendJson数据异常");
                            return;
                        }

                        rawMessage = EncoderIssueApplicationSend(getSampleApplyListOutput);

                        sendMessageService.UpdateSuccessRequestApplication(task.Id, rawMessage).GetAwaiter().GetResult();
                        Logger.Debug(logType, $"待发送LIS下发申请信息处理成功 id={task.Id}");
                        break;
                    default:
                        MarkFailed(task.Id, "不支持的类型 " + task.Type);
                        break;
                }
            }
            catch (Exception e)
            {
                MarkFailed(task.Id, "HandleTask异常" + e.Message);
            }
        }

        public abstract byte[] EncoderRequestApplicationSend(GetSampleApplyItemOutput getSampleApplyItemOutput);

        public abstract byte[] EncoderIssueApplicationSend(GetSampleApplyListOutput getSampleApplyListOutput);

        private void MarkFailed(long id, string errorMessage)
        {
            sendMessageRepository.UpdateStatusAndErrorMessageAndUpdateTimeById(
                id,
                SendMessage.StatusEnum.Failed,
                errorMessage,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()).GetAwaiter().GetResult();
            Logger.Warn(logType, $"待发送消息处理失败 id={id}: {errorMessage}");
        }
    }
}