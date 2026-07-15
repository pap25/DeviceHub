using DeviceHub.Lis.Dto;
using DeviceHub.Template.Template.SerialPort;
using DeviceHub.YhloTestSerialPort.Protocol;

namespace DeviceHub.YhloTestSerialPort.Handler
{
    public class SendHandler : SerialPortSendHandlerBase
    {
        public SendHandler(long instrumentId) : base(instrumentId)
        {
        }

        protected override List<byte[]> EncoderIssueApplication(GetSampleApplyListOutput getSampleApplyListOutput)
        {
            return AstmMessageEncoder.EncoderIssueApplication(getSampleApplyListOutput);
        }

        protected override List<byte[]> EncoderRequestApplication(GetSampleApplyItemOutput getSampleApplyItemOutput)
        {
            return AstmMessageEncoder.EncoderRequestApplication(getSampleApplyItemOutput);
        }
    }
}
