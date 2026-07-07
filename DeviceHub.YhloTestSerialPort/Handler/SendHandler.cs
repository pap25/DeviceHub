using DeviceHub.Base.Common;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using DeviceHub.Service;
using DeviceHub.YhloTestSerialPort.Protocol;
using System.Text.Json;

namespace DeviceHub.YhloTestSerialPort.Handler
{
    public class SendHandler : IBatchTaskHandler<SendMessage>
    {
        private readonly string logType = nameof(SendHandler);
        private long _instrumentId;
        private readonly SendMessageRepository sendMessageRepository = SendMessageRepository.Instance;
        private readonly SendMessageLargeRepository sendMessageLargeRepository = SendMessageLargeRepository.Instance;
        private readonly SendMessageService sendMessageService = SendMessageService.Instance;
        private readonly ILisClient lisClient = LisClient.Instance;

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
                SendMessageLarge? receiveMessageLarge = sendMessageLargeRepository.GetByReceiveMessageId(task.Id).GetAwaiter().GetResult();
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (receiveMessageLarge == null)
                {
                    MarkFailed(task.Id, "数据异常", now);
                    return;
                }
                GetSampleApplyItemOutput? getSampleApplyItemOutput = JsonSerializer.Deserialize<GetSampleApplyItemOutput>(receiveMessageLarge.SendJson);
                if (getSampleApplyItemOutput == null)
                {
                    MarkFailed(task.Id, "数据异常", now);
                    return;
                }
                string sendContent = AstmMessageEncoder.EncoderIssueApplication(getSampleApplyItemOutput);

                // 发送 发时候先看看在收没，没有收的话在发
                // 保存 send_message_encoder
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
