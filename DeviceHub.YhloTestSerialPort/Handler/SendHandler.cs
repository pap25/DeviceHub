using DeviceHub.Base.Common;
using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using DeviceHub.Service;
using DeviceHub.YhloTestSerialPort.Protocol;
using System.Text.Json;
using static DeviceHub.YhloTestSerialPort.Driver;

namespace DeviceHub.YhloTestSerialPort.Handler
{
    public class SendHandler : ISenderTaskHandler
    {
        private readonly string logType = nameof(SendHandler);
        private long _instrumentId;
        private readonly SendMessageRepository sendMessageRepository = SendMessageRepository.Instance;
        private readonly SendMessageLargeRepository sendMessageLargeRepository = SendMessageLargeRepository.Instance;
        private readonly SendMessageService sendMessageService = SendMessageService.Instance;
        private long sendMessageId;

        public SendHandler(long instrumentId)
        {
            _instrumentId = instrumentId;
        }

        public List<byte[]> SearchEncoderTask()
        {
            while (true)
            {
                SendMessage? task = null;
                try
                {
                    task = sendMessageRepository.FindFirstByInstrumentIdAndStatusOrderAsc(_instrumentId, SendMessage.StatusEnum.Pending).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Logger.Warn(logType, $"查询DB send_message 异常 instrumentId={_instrumentId}: {ex.Message}");
                }
                if (task == null)
                {
                    return [];
                }

                try
                {
                    sendMessageId = task.Id;
                    SendMessageLarge? receiveMessageLarge = sendMessageLargeRepository.GetBySendMessageId(task.Id).GetAwaiter().GetResult();
                    if (receiveMessageLarge == null)
                    {
                        MarkFailed(task.Id, "数据异常");
                        continue;
                    }

                    if (task.Type == SendMessage.TypeEnum.RequestApplication)
                    {
                        GetSampleApplyItemOutput? getSampleApplyItemOutput = JsonSerializer.Deserialize<GetSampleApplyItemOutput>(receiveMessageLarge.SendJson);
                        if (getSampleApplyItemOutput == null)
                        {
                            MarkFailed(task.Id, "SendJson数据异常");
                            continue;
                        }

                        return AstmMessageEncoder.EncoderRequestApplication(getSampleApplyItemOutput);
                    }
                    else if (task.Type == SendMessage.TypeEnum.IssueApplication)
                    {
                        GetSampleApplyListOutput? getSampleApplyListOutput = JsonSerializer.Deserialize<GetSampleApplyListOutput>(receiveMessageLarge.SendJson);
                        if (getSampleApplyListOutput == null)
                        {
                            MarkFailed(task.Id, "SendJson数据异常");
                            continue;
                        }

                        return AstmMessageEncoder.EncoderIssueApplication(getSampleApplyListOutput);
                    }
                    else
                    {
                        MarkFailed(task.Id, "不支持的类型 " + task.Type);
                        continue;
                    }
                }
                catch (Exception e)
                {
                    MarkFailed(task.Id, "SearchEncoderTask异常" + e.Message);
                    continue;
                }
            }
        }

        private void MarkFailed(long id, string errorMessage)
        {
            sendMessageRepository.UpdateStatusAndErrorMessageAndUpdateTimeById(
                id,
                SendMessage.StatusEnum.Failed,
                errorMessage,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()).GetAwaiter().GetResult();
            Logger.Warn(logType, $"消息处理失败 id={id}: {errorMessage}");
        }

        public void Completed(List<byte[]> sendFrameList)
        {
            try
            {
                sendMessageService.UpdateSuccessRequestApplication(sendMessageId, merge(sendFrameList)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Error(logType, $"消息发送完成回写异常 id={sendMessageId}", ex);
            }
        }

        private byte[] merge(List<byte[]> sendFrameList)
        {
            int totalLength = sendFrameList.Sum(x => x.Length);

            byte[] result = new byte[totalLength];

            int offset = 0;

            foreach (var frame in sendFrameList)
            {
                Buffer.BlockCopy(frame, 0, result, offset, frame.Length);
                offset += frame.Length;
            }

            return result;
        }
    }
}
