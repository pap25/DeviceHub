using DeviceHub.Base.Common;
using DeviceHub.Base.Transports;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DeviceHub.Yhlo.Handler
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
            int pageSize = 10;
            // select * from receive_message where instrument_id=? and status=? order by id asc limit ?
            List<ReceiveMessage> taskList = receiveMessageRepository.FindByInstrumentIdAndStatusOrderAsc(_instrumentId, ReceiveMessage.StatusEnum.Pending, pageSize);
            return taskList;
        }
        public void HandleTask(ReceiveMessage task)
        {
            // 调用接口上传检验结果
            // 解码后保存 receive_message_decode 更新receive_message

            //ReceiveMessageLarge? receiveMessageLarge = receiveMessageLargeRepository.GetByReceiveMessageId(task.Id);
        }
    }
}
