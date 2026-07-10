using DeviceHub.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.YhloTestTcpServer
{
    public class ScheduledTasks : IDisposable
    {
        private readonly string logType = nameof(ScheduledTasks);
        private Timer _heartbeatTimer;
        private Timer _requestOrderTimer;

        /// <summary>
        /// 启动所有定时任务
        /// </summary>
        public void Start()
        {
            // 1秒后执行，以后每30秒执行一次
            _heartbeatTimer = new Timer(
                HeartbeatTask,
                null,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(30));

            // 10秒后执行，以后每1小时执行一次
            _requestOrderTimer = new Timer(
                RequestOrderTask,
                null,
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(5));

            Logger.Info(logType, "ScheduledTasks started.");
        }

        /// <summary>
        /// 心跳任务
        /// </summary>
        private void HeartbeatTask(object state)
        {
            try
            {
                //Logger.Debug("执行设备心跳检查");

                // TODO:
                // 1. 检查TCP连接状态
                // 2. 判断最后通信时间
                // 3. 超时则重连设备
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "设备心跳任务异常", ex);
            }
        }

        private void RequestOrderTask(object state)
        {
            try
            {
                // 查询申请单
                // 查询到后保存send_message
                // 唤起SendHandler消费
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "异常", ex);
            }
        }

        /// <summary>
        /// 停止定时任务
        /// </summary>
        public void Dispose()
        {
            _heartbeatTimer?.Dispose();
            _requestOrderTimer?.Dispose();

            Logger.Info(logType, "ScheduledTasks stopped.");
        }
    }
}
