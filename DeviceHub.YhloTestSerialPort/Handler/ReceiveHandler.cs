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
            string rawMessage = receiveMessageLarge.RawMessage;

            // 解析开始结束符
            // 校验<CS> CS：必须校验
            // 验证FN FN：只检查是否合法（0~7） 不检查连续性
            // 失败更新队列失败，并记录失败原因
            // 验证成功则进入解析步骤

            //receiveMessageRepository.UpdateStatusAndUpdateTimeById(task.Id, ReceiveMessage.StatusEnum.Success, now);
        }
    }
}
