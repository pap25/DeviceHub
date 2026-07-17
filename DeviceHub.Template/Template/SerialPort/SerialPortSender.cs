using DeviceHub.Template.Constant;
using DeviceHub.Utils;

namespace DeviceHub.Template.Template.SerialPort
{
    /// <summary>
    /// 由 <see cref="NotifyTask"/> 驱动：循环调用 <see cref="HandleTask"/>，
    /// 执行一步发送状态机后等待；Session 收到 ACK/EOT 等时 NotifyConsume 唤起。
    /// </summary>
    public class SerialPortSender : INotifyTaskHandler
    {
        private readonly string logType = nameof(SerialPortSender);
        private readonly SerialPortSession session;
        private readonly ISenderSerialPortTaskHandler senderTaskHandler;

        private List<byte[]> sendFrameList = [];
        private int sendFrameOffset = 0;

        public SerialPortSender(SerialPortSession session, ISenderSerialPortTaskHandler senderTaskHandler)
        {
            this.session = session;
            this.senderTaskHandler = senderTaskHandler;
        }

        internal IReadOnlyList<byte[]> SendFrameList => sendFrameList;

        public void HandleTask()
        {
            try
            {
                session.ExecuteWithStateLock(HandleTaskUnlocked);
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "HandleTask 异常", ex);
            }
        }

        /// <summary>
        /// 执行一步 ASTM 发送状态机。调用方须已持有 stateLock。
        /// </summary>
        private void HandleTaskUnlocked()
        {
            session.CheckReceiveIdleTimeoutUnlocked();

            switch (session.GetLineStateUnlocked())
            {
                case LineState.Idle:
                    sendFrameList = senderTaskHandler.SearchEncoderTask();
                    if (sendFrameList.Count > 0)
                    {
                        Logger.Debug(logType, $"待发送帧条数: {sendFrameList.Count}");
                        sendFrameOffset = 0;
                        session.SendUnlocked(ASTMProtocols.ENQ);
                        session.SetLineStateUnlocked(LineState.WaitingEnqAck);
                    }
                    break;

                case LineState.WaitingEnqAck:
                    // 已发 ENQ，等待对端 ACK；由 Session 收到 ACK 后改状态并 NotifyConsume
                    break;

                case LineState.Sending:
                    if (sendFrameList.Count > sendFrameOffset)
                    {
                        byte[] frame = sendFrameList[sendFrameOffset];
                        session.SendFrameUnlocked(frame);
                        sendFrameOffset++;
                        // 发完一帧后等待 ACK，再由 Session NotifyConsume 唤起发下一帧
                    }
                    else if (sendFrameList.Count > 0)
                    {
                        senderTaskHandler.Completed(sendFrameList);
                        ClearSendFrames();
                        session.SetLineStateUnlocked(LineState.Idle);
                        session.SendUnlocked(ASTMProtocols.EOT);
                    }
                    break;

                case LineState.Receiving:
                    // 对端占用线路，不发送
                    break;
            }
        }

        public void ClearSendFrames()
        {
            sendFrameList = [];
            sendFrameOffset = 0;
        }
    }
}
