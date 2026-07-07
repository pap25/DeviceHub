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

                _ = ParseData(parseResult.ParsedData, task);
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
        private async Task ParseData(List<string> dataList, ReceiveMessage task)
        {
            ParseResult parseResult = AstmMessageDecode.Parse(dataList);
            UploadSpecimenTestResultInput uploadSpecimenTestResultInput = ToUploadSpecimenTestResultInput(parseResult);

            // 判断是检验结果还是查询检验信息
            // 如果是检验结果上报检验信息
            // 如果是查询检验信息，调用LIS接口查询信息，添加发送队列

            Resp<UploadSpecimenTestResultOutput> resp = await lisClient.UploadSpecimenTestResult(uploadSpecimenTestResultInput);
            if (!resp.IsSuccess())
            {
                MarkFailed(task.Id, resp.GetErrorMsg() ?? "", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                return;
            }
            string resultId = resp.GetData().ResultId;

            await receiveMessageService.UpdateSuccess(task.Id, ReceiveMessageDecode.TypeEnum.TestResult, resultId, "", "", JsonSerializer.Serialize(uploadSpecimenTestResultInput));
        }

        private UploadSpecimenTestResultInput ToUploadSpecimenTestResultInput(ParseResult parseResult)
        {
            return new UploadSpecimenTestResultInput();
        }
    }
}
