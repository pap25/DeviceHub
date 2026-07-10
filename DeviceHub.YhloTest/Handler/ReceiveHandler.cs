using DeviceHub.Utils;
using DeviceHub.Base.Transports;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DeviceHub.YhloTestTcpServer.Handler
{
    public class ReceiveHandler : IBatchTaskHandler<ReceiveMessage>
    {
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
            // 调用接口上传检验结果
            // 解码后保存 receive_message_decode 更新receive_message
            ReceiveMessageLarge? receiveMessageLarge = receiveMessageLargeRepository.GetByReceiveMessageId(task.Id).GetAwaiter().GetResult();
            if (receiveMessageLarge == null)
            {
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                //receiveMessageRepository.UpdateStatusAndErrorMessageAndUpdateTimeById(task.Id, ReceiveMessage.StatusEnum.Failed, "数据异常", now);
                return;
            }
            byte[] rawMessage = receiveMessageLarge.RawMessage;


            //receiveMessageRepository.UpdateStatusAndUpdateTimeById(task.Id, ReceiveMessage.StatusEnum.Success, now);
        }
    }
}
