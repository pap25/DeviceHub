using DeviceHub.Utils;
using DeviceHub.Lis;
using DeviceHub.Lis.Impl;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using DeviceHub.Service;
using DeviceHub.YhloTestTcpServer.Protocol;

namespace DeviceHub.YhloTestTcpServer.Handler
{
    public partial class ReceiveHandler : IBatchTaskHandler<ReceiveMessage>
    {
        private readonly string logType = nameof(ReceiveHandler);
        private long _instrumentId;
        private readonly ReceiveMessageRepository receiveMessageRepository = ReceiveMessageRepository.Instance;
        private readonly ReceiveMessageLargeRepository receiveMessageLargeRepository = ReceiveMessageLargeRepository.Instance;
        private readonly ReceiveMessageService receiveMessageService = ReceiveMessageService.Instance;
        private readonly ILisClient lisClient = LisClient.Instance;

        public ReceiveHandler(long instrumentId)
        {
            _instrumentId = instrumentId;
        }

        public IEnumerable<ReceiveMessage> SearchTask()
        {
            List<ReceiveMessage> taskList = receiveMessageRepository
                .FindByInstrumentIdAndStatusOrderAsc(_instrumentId, ReceiveMessage.StatusEnum.Pending, 15).GetAwaiter().GetResult();
            if (taskList.Count > 0)
                Logger.Debug(logType, $"查询待解码消息 {taskList.Count} 条");
            return taskList;
        }

        public void HandleTask(ReceiveMessage task)
        {
            try
            {
                ReceiveMessageLarge? receiveMessageLarge = receiveMessageLargeRepository.GetByReceiveMessageId(task.Id).GetAwaiter().GetResult();
                if (receiveMessageLarge == null)
                {
                    MarkFailed(task.Id, "数据异常");
                    return;
                }

                ParseData(receiveMessageLarge.RawMessage, task);
            }
            catch (Exception e)
            {
                MarkFailed(task.Id, "HandleTask异常" + e.Message);
            }
        }

        private void MarkFailed(long id, string errorMessage)
        {
            receiveMessageRepository.UpdateStatusAndErrorMessageAndUpdateTimeById(
                id,
                ReceiveMessage.StatusEnum.Failed,
                errorMessage,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()).GetAwaiter().GetResult();
            Logger.Warn(logType, $"待解码消息处理失败 id={id}: {errorMessage}");
        }

        /// <summary>
        /// HL7 DATA 解析
        /// </summary>
        private void ParseData(byte[] rawMessage, ReceiveMessage task)
        {
            Hl7MessageVerify.VerifyParseResult verifyResult = Hl7MessageVerify.VerifyParse(rawMessage);
            if (!verifyResult.Success)
            {
                MarkFailed(task.Id, verifyResult.ErrorMessage);
                return;
            }

            Hl7MessageDecode.ParseResult parseResult = Hl7MessageDecode.Parse(verifyResult.SegmentList);
            string messageType = parseResult.MshSegment.MessageType;
            if (messageType.StartsWith("ORU", StringComparison.OrdinalIgnoreCase))
            {
                if (parseResult.IsQcResult)
                {
                    UploadQualityControlTestResult(task, parseResult);
                    return;
                }

                UploadSpecimenTestResult(task, parseResult);
                return;
            }

            if (messageType.StartsWith("QRY", StringComparison.OrdinalIgnoreCase))
            {
                SaveSampleQuery(task, parseResult);
                return;
            }

            MarkFailed(task.Id, $"不支持消息类型 {messageType}");
        }
    }
}
