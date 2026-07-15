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

        protected override List<byte[]> EncoderIssueApplication(GetSampleApplyItemOutput getSampleApplyItemOutput)
        {
            return AstmMessageEncoder.EncoderIssueApplication(getSampleApplyItemOutput);
        }

        protected override List<byte[]> EncoderRequestApplication(GetSampleApplyItemOutput getSampleApplyItemOutput)
        {
            return AstmMessageEncoder.EncoderRequestApplication(getSampleApplyItemOutput);
        }
    }
}
