using DeviceHub.Lis.Dto;
using DeviceHub.Template.Template;
using DeviceHub.YhloTestTcpServer.Protocol;
using System.Text;

namespace DeviceHub.YhloTestTcpServer.Handler
{
    public class SendHandler : SendHandlerBase
    {
        private TcpServerSession tcpServerSession;

        public SendHandler(long instrumentId, TcpServerSession tcpServerSession, Encoding encoding) : base(instrumentId, encoding)
        {
            this.tcpServerSession = tcpServerSession;
        }

        protected override byte[] EncoderIssueApplicationSend(GetSampleApplyItemOutput getSampleApplyItemOutput)
        {
            byte[] rawMessage = Hl7MessageEncoder.EncoderIssueApplication(getSampleApplyItemOutput, messageEncoding);
            tcpServerSession.SendAsync(rawMessage).GetAwaiter().GetResult();
            return rawMessage;
        }

        protected override byte[] EncoderRequestApplicationSend(GetSampleApplyItemOutput getSampleApplyItemOutput)
        {
            byte[] rawMessage = Hl7MessageEncoder.EncoderRequestApplication(getSampleApplyItemOutput, messageEncoding);
            tcpServerSession.SendAsync(rawMessage).GetAwaiter().GetResult();
            return rawMessage;
        }
    }
}