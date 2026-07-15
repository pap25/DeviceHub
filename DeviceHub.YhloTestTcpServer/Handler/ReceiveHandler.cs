using DeviceHub.Lis;
using DeviceHub.Lis.Impl;
using DeviceHub.Model.Entities;
using DeviceHub.Service;
using DeviceHub.Template.Protocol;
using DeviceHub.Template.Template;
using DeviceHub.Utils;
using DeviceHub.YhloTestTcpServer.Protocol;

namespace DeviceHub.YhloTestTcpServer.Handler
{
    public partial class ReceiveHandler : ReceiveHandlerBase
    {
        private long _instrumentId;
        private readonly ReceiveMessageService receiveMessageService = ReceiveMessageService.Instance;
        private readonly ILisClient lisClient = LisClient.Instance;
        private IConsumeTask? sendTask;
        public IConsumeTask SendTask
        {
            set
            {
                this.sendTask = value;
            }
        }

        public ReceiveHandler(long instrumentId) : base(instrumentId)
        {
            this._instrumentId = instrumentId;
        }

        protected override void ParseData(byte[] rawMessage, ReceiveMessage task)
        {
            Hl7MessageVerify.VerifyParseResult verifyResult = Hl7MessageVerify.VerifyParse(rawMessage);
            if (!verifyResult.Success)
            {
                MarkFailed(task.Id, verifyResult.ErrorMessage);
                return;
            }

            Hl7MessageDecode.ParseResult parseResult = Hl7MessageDecode.Parse(verifyResult.SegmentList);
            string messageType = parseResult.MshSegment.MessageType;
            if (messageType.StartsWith("ORU", StringComparison.OrdinalIgnoreCase))
            {
                if (parseResult.IsQcResult)
                {
                    UploadQualityControlTestResult(task, parseResult);
                    return;
                }

                UploadSpecimenTestResult(task, parseResult);
                return;
            }

            if (messageType.StartsWith("QRY", StringComparison.OrdinalIgnoreCase))
            {
                SaveSampleQuery(task, parseResult);
                this.sendTask?.NotifyConsume();
                return;
            }

            MarkFailed(task.Id, $"不支持消息类型 {messageType}");
        }
    }
}