using DeviceHub.Utils;
using DeviceHub.Lis;
using DeviceHub.Lis.Impl;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using DeviceHub.Service;
using DeviceHub.YhloTestV2SerialPort.Protocol;
using static DeviceHub.YhloTestV2SerialPort.Protocol.AstmMessageDecode;

namespace DeviceHub.YhloTestV2SerialPort.Handler
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
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (receiveMessageLarge == null)
                {
                    MarkFailed(task.Id, "数据异常", now);
                    return;
                }

                AstmMessageVerify.VerifyParseResult parseResult = AstmMessageVerify.VerifyParse(receiveMessageLarge.RawMessage);
                if (!parseResult.Success)
                {
                    MarkFailed(task.Id, parseResult.ErrorMessage, now);
                    return;
                }

                ParseData(parseResult.ParsedRecord, task);
            }
            catch (Exception e)
            {
                MarkFailed(task.Id, "HandleTask异常" + e.Message, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
        }

        private void MarkFailed(long id, string errorMessage, long now)
        {
            receiveMessageRepository.UpdateStatusAndErrorMessageAndUpdateTimeById(
                id,
                ReceiveMessage.StatusEnum.Failed,
                errorMessage,
                now).GetAwaiter().GetResult();
            Logger.Warn(logType, $"消息处理失败 id={id}: {errorMessage}");
        }

        /// <summary>
        /// DATA 解析
        /// </summary>
        private void ParseData(List<string> recordList, ReceiveMessage task)
        {
            ParseResult parseResult = AstmMessageDecode.Parse(recordList);
            if (parseResult.RequestInformationRecord != null)
            {
                SaveSampleQuery(task, parseResult.RequestInformationRecord);
                return;
            }
            else
            {
                UploadSpecimenTestResult(task, parseResult);
                return;
            }

            //MarkFailed(task.Id, $"不支持消息类型 {parseResult.HeaderRecord.ProcessingId}", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }
    }
}
