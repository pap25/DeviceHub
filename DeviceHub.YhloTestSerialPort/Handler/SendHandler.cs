using DeviceHub.Base.Common;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
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
        private readonly ILisClient lisClient = LisClient.Instance;

        public SendHandler(long instrumentId)
        {
            _instrumentId = instrumentId;
        }

        public SendMessage? SearchTask()
        {
            List<SendMessage> taskList = sendMessageRepository
                .FindByInstrumentIdAndStatusOrderAsc(_instrumentId, SendMessage.StatusEnum.Pending, 1).GetAwaiter().GetResult();
            return taskList[0];
        }
        public List<byte[]> SearchEncoderTask()
        {
            SendMessage task = null;
            try
            {
                SendMessageLarge? receiveMessageLarge = sendMessageLargeRepository.GetBySendMessageId(task.Id).GetAwaiter().GetResult();
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (receiveMessageLarge == null)
                {
                    MarkFailed(task.Id, "数据异常", now);
                    return null;
                }

                if (task.Type == SendMessage.TypeEnum.RequestApplication)
                {
                    GetSampleApplyItemOutput? getSampleApplyItemOutput = JsonSerializer.Deserialize<GetSampleApplyItemOutput>(receiveMessageLarge.SendJson);
                    if (getSampleApplyItemOutput == null)
                    {
                        MarkFailed(task.Id, "数据异常", now);
                        return null;
                    }

                    List<byte[]> sendFrameList = AstmMessageEncoder.EncoderRequestApplication(getSampleApplyItemOutput);

                    // 发送 发时候先看看在收没，没有收的话在发
                    foreach (byte[] sendFrame in sendFrameList)
                    {

                    }

                    sendMessageService.UpdateSuccessRequestApplication(task.Id, merge(sendFrameList)).GetAwaiter();
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
            return null;
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

        public void Completed(List<byte[]> sendFrameList)
        {
            throw new NotImplementedException();
        }
    }
}
