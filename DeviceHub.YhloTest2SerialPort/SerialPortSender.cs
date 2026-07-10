using DeviceHub.Utils;
using DeviceHub.Base.Constant;

namespace DeviceHub.YhloTest2SerialPort
{
    public class SerialPortSender
    {
        private readonly string logType = nameof(SerialPortSender);
        private readonly SerialPortSession session;
        private readonly ISenderTaskHandler senderTaskHandler;

        private List<byte[]> sendFrameList = [];
        private int sendFrameOffset;

        public SerialPortSender(SerialPortSession session, ISenderTaskHandler senderTaskHandler)
        {
            this.session = session;
            this.senderTaskHandler = senderTaskHandler;
        }

        internal IReadOnlyList<byte[]> SendFrameList => sendFrameList;

        internal void ClearSendFrames()
        {
            sendFrameList = [];
            sendFrameOffset = 0;
        }

        /// <summary>
        /// 发送一个完整帧（主动向仪器下发数据）。调用方须已持有 stateLock。
        /// </summary>
        internal void TrySendNextUnlocked()
        {
            switch (session.GetLineStateUnlocked())
            {
                case LineState.Idle:
                    sendFrameList = senderTaskHandler.SearchEncoderTask();
                    if (sendFrameList.Count > 0)
                    {
                        Logger.Debug(logType, $"待发送帧条数: {sendFrameList.Count}");
                        sendFrameOffset = 0;
                        session.SendUnlocked(ASTMProtocols.ENQ);
                        session.SetLineStateUnlocked(LineState.WaitingAck);
                    }
                    break;

                case LineState.WaitingAck:
                    break;

                case LineState.Sending:
                    if (sendFrameList.Count > sendFrameOffset)
                    {
                        byte[] frame = sendFrameList[sendFrameOffset];
                        sendFrameOffset++;
                        session.SendFrameUnlocked(frame);
                    }
                    else if (sendFrameList.Count > 0)
                    {
                        senderTaskHandler.Completed(sendFrameList);
                        sendFrameList = [];
                        sendFrameOffset = 0;
                        session.SetLineStateUnlocked(LineState.Idle);
                        session.SendUnlocked(ASTMProtocols.EOT);
                    }
                    break;

                case LineState.Receiving:
                    break;
            }
        }
    }
}
