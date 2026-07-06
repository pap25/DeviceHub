using DeviceHub.Base.Common;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using DeviceHub.Yhlo.Protocol;

namespace DeviceHub.Yhlo.Handler
{
    public class ReceiveHandler : IBatchTaskHandler<ReceiveMessage>
    {
        private readonly string logType = nameof(ReceiveHandler);
        private long _instrumentId;
        private readonly ReceiveMessageRepository receiveMessageRepository = ReceiveMessageRepository.Instance;
        private readonly ReceiveMessageLargeRepository receiveMessageLargeRepository = ReceiveMessageLargeRepository.Instance;

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
            ReceiveMessageLarge? receiveMessageLarge = receiveMessageLargeRepository.GetByReceiveMessageId(task.Id).GetAwaiter().GetResult();
            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (receiveMessageLarge == null)
            {
                MarkFailed(task.Id, "数据异常", now);
                return;
            }

            AstmMessageParser.ParseResult parseResult = AstmMessageParser.TryParse(receiveMessageLarge.RawMessage);
            if (!parseResult.Success)
            {
                MarkFailed(task.Id, parseResult.ErrorMessage, now);
                return;
            }

            ParseData(parseResult.ParsedData, task);
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
        private void ParseData(string data, ReceiveMessage task)
        {
            // TODO: 解析 ASTM 记录并上传检验结果
            // receiveMessageRepository.UpdateStatusAndUpdateTimeById(task.Id, ReceiveMessage.StatusEnum.Success, now);
            // 解码后保存 receive_message_decode，更新 receive_message
        }
    }
}
