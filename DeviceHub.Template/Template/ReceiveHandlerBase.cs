using DeviceHub.Utils;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using System.Text;

namespace DeviceHub.Template.Template
{
    public abstract class ReceiveHandlerBase : IBatchTaskHandler<ReceiveMessage>
    {
        private readonly string logType = nameof(ReceiveHandlerBase);
        private long _instrumentId;
        private readonly ReceiveMessageRepository receiveMessageRepository = ReceiveMessageRepository.Instance;
        private readonly ReceiveMessageLargeRepository receiveMessageLargeRepository = ReceiveMessageLargeRepository.Instance;
        protected Encoding messageEncoding;

        public ReceiveHandlerBase(long instrumentId, Encoding encoding)
        {
            _instrumentId = instrumentId;
            messageEncoding = encoding;
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

        protected void MarkFailed(long id, string errorMessage)
        {
            string dbErrorMessage = errorMessage.Length > 500 ? errorMessage[..500] : errorMessage;
            receiveMessageRepository.UpdateStatusAndErrorMessageAndUpdateTimeById(
                id,
                ReceiveMessage.StatusEnum.Failed,
                dbErrorMessage,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()).GetAwaiter().GetResult();
            Logger.Warn(logType, $"待解码消息处理失败 id:{id} {errorMessage}");
        }

        protected abstract void ParseData(byte[] rawMessage, ReceiveMessage task);
    }
}
