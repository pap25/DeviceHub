using DeviceHub.Abstractions.Dto;
using DeviceHub.Base.Common;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using DeviceHub.Service;
using DeviceHub.Yhlo.Protocol;
using System.Text.Json;
using static DeviceHub.Yhlo.Protocol.AstmMessageDecode;
using static DeviceHub.YhloTestSerialPort.Protocol.AstmMessageEntity;

namespace DeviceHub.Yhlo.Handler
{
    public class ReceiveHandler : IBatchTaskHandler<ReceiveMessage>
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
            if (parseResult.HeaderRecord.ProcessingId == HeaderRecord.MessageType.PR.ToString())
            {
                // 检验结果 1 上传到LIS 2 更新状态并记录
                UploadSpecimenTestResultInput uploadSpecimenTestResultInput = ToUploadSpecimenTestResultInput(parseResult);
                Resp<UploadSpecimenTestResultOutput> resp = lisClient.UploadSpecimenTestResult(uploadSpecimenTestResultInput).GetAwaiter().GetResult();
                if (!resp.IsSuccess())
                {
                    MarkFailed(task.Id, resp.GetErrorMsg() ?? "", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                    return;
                }
                string resultId = resp.GetData().ResultId;

                receiveMessageService.UpdateSuccess(task.Id, ReceiveMessageDecode.TypeEnum.TestResult, resultId, "", "", JsonSerializer.Serialize(uploadSpecimenTestResultInput)).GetAwaiter();
                return;
            }
            else if (parseResult.HeaderRecord.ProcessingId == HeaderRecord.MessageType.RQ.ToString())
            {
                if (parseResult.TestOrderRecord == null)
                {
                    MarkFailed(task.Id, "数据异常没有订单记录", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                    return;
                }
                receiveMessageService.SaveSampleQuery(_instrumentId, task.Id, parseResult.TestOrderRecord.SampleId, parseResult.TestOrderRecord.InstrumentSpecimenId);
                return;
            }

            MarkFailed(task.Id, $"不支持消息类型 {parseResult.HeaderRecord.ProcessingId}", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        private UploadSpecimenTestResultInput ToUploadSpecimenTestResultInput(ParseResult parseResult)
        {
            return new UploadSpecimenTestResultInput();
        }
    }
}
