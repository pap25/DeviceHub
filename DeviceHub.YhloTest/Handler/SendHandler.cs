using DeviceHub.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.YhloTestTcpServer.Handler
{
    public class SendHandler : IBatchTaskHandler<Object>
    {
        public void HandleTask(object task)
        {
            // 发送
            // 保存 send_message_encoder
            throw new NotImplementedException();
        }

        public IEnumerable<object> SearchTask()
        {
            // 查询 send_message
            throw new NotImplementedException();
        }
    }
}
