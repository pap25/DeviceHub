using DeviceHub.Lis.Dto;
using DeviceHub.Template.Template;
using DeviceHub.YhloTestTcpServer.Protocol;

namespace DeviceHub.YhloTestTcpServer.Handler
{
    public class SendHandler : SendHandlerBase
    {
        private TcpServerSession tcpServerSession;

        public SendHandler(long instrumentId, TcpServerSession tcpServerSession) : base(instrumentId)
        {
            this.tcpServerSession = tcpServerSession;
        }

        protected override byte[] EncoderIssueApplicationSend(GetSampleApplyItemOutput getSampleApplyItemOutput)
        {
            byte[] rawMessage = Hl7MessageEncoder.EncoderIssueApplication(getSampleApplyItemOutput);
            tcpServerSession.SendAsync(rawMessage).GetAwaiter().GetResult();
            return rawMessage;
        }

        protected override byte[] EncoderRequestApplicationSend(GetSampleApplyItemOutput getSampleApplyItemOutput)
        {
            byte[] rawMessage = Hl7MessageEncoder.EncoderRequestApplication(getSampleApplyItemOutput);
            tcpServerSession.SendAsync(rawMessage).GetAwaiter().GetResult();
            return rawMessage;
        }
    }
}