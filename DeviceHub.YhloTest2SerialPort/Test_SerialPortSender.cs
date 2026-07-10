//using DeviceHub.Base.Constant;
//using DeviceHub.Utils;

//namespace DeviceHub.YhloTest2SerialPort
//{
//    public class Test_SerialPortSender : INotifyTaskHandler
//    {
//        private readonly string logType = nameof(Test_SerialPortSender);
//        private readonly SerialPortSession session;
//        private readonly ISenderTaskHandler senderTaskHandler;

//        private List<byte[]> sendFrameList = [];
//        private int sendFrameOffset;

//        public Test_SerialPortSender(SerialPortSession session, ISenderTaskHandler senderTaskHandler)
//        {
//            this.session = session;
//            this.senderTaskHandler = senderTaskHandler;
//        }

//        public IReadOnlyList<byte[]> SendFrameList => sendFrameList;

//        public void ClearSendFrames()
//        {
//            sendFrameList = [];
//            sendFrameOffset = 0;
//        }

//        public void HandleTask()
//        {
//            switch (session.GetLineStateUnlocked())
//            {
//                case LineState.Idle:
//                    sendFrameList = senderTaskHandler.SearchEncoderTask();
//                    if (sendFrameList.Count > 0)
//                    {
//                        Logger.Debug(logType, $"待发送帧条数: {sendFrameList.Count}");
//                        sendFrameOffset = 0;
//                        session.SendUnlocked(ASTMProtocols.ENQ);
//                        session.SetLineStateUnlocked(LineState.WaitingAck);
//                    }
//                    break;

//                case LineState.WaitingAck:
//                    break;

//                case LineState.Sending:
//                    if (sendFrameList.Count > sendFrameOffset)
//                    {
//                        byte[] frame = sendFrameList[sendFrameOffset];
//                        sendFrameOffset++;
//                        session.SendFrameUnlocked(frame);
//                    }
//                    else if (sendFrameList.Count > 0)
//                    {
//                        senderTaskHandler.Completed(sendFrameList);
//                        ClearSendFrames();
//                        session.SetLineStateUnlocked(LineState.Idle);
//                        session.SendUnlocked(ASTMProtocols.EOT);
//                    }
//                    break;

//                case LineState.Receiving:
//                    break;
//            }
//        }
//    }
//}
