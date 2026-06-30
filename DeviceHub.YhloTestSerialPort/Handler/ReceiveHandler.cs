using DeviceHub.Base.Common;
using DeviceHub.Base.Transports;
using DeviceHub.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.YhloTestSerialPort.Handler
{
    public class ReceiveHandler : IBatchTaskHandler<ReceiveMessage>
    {
        public void HandleTask(ReceiveMessage task)
        {
            // 调用接口上传检验结果
            // 解码后保存 receive_message_decode 更新receive_message
            throw new NotImplementedException();
        }

        public IEnumerable<ReceiveMessage> SearchTask()
        {
            // 查询 receive_message
            throw new NotImplementedException();
        }
    }
}
