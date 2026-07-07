using DeviceHub.Base.Common;
using DeviceHub.Lis;
using DeviceHub.Lis.Impl;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using DeviceHub.Service;

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

            // 发送 发时候先看看在收没，没有收的话在发
            // 保存 send_message_encoder
            
        }
    }
}
