using DeviceHub.Lis.Dto;
using DeviceHub.Template.Template.SerialPort;
using DeviceHub.YhloTestSerialPort.Protocol;
using System.Text;

namespace DeviceHub.YhloTestSerialPort.Handler
{
    public class SendHandler : SerialPortSendHandlerBase
    {
        public SendHandler(long instrumentId, Encoding encoding) : base(instrumentId, encoding)
        {
        }

        protected override List<byte[]> EncoderIssueApplication(GetSampleApplyItemOutput getSampleApplyItemOutput)
        {
            return AstmMessageEncoder.EncoderIssueApplication(getSampleApplyItemOutput, MessageEncoding);
        }

        protected override List<byte[]> EncoderRequestApplication(GetSampleApplyItemOutput getSampleApplyItemOutput)
        {
            return AstmMessageEncoder.EncoderRequestApplication(getSampleApplyItemOutput, MessageEncoding);
        }
    }
}
