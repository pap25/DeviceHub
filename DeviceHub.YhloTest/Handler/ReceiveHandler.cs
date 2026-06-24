using DeviceHub.Base.Common;
using DeviceHub.Base.Transports;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Yhlo.Handler
{
    public class ReceiveHandler : IBatchTaskHandler<Object>
    {
        public void HandleTask(object task)
        {
            // 调用接口上传检验结果
            // 解码后保存 receive_message_decode 更新receive_message
            throw new NotImplementedException();
        }

        public IEnumerable<object> SearchTask()
        {
            // 查询 receive_message
            throw new NotImplementedException();
        }
    }
}
