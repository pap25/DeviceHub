using DeviceHub.Lis;
using DeviceHub.Lis.Impl;
using DeviceHub.Model.Entities;
using DeviceHub.Service;
using DeviceHub.Template.Protocol;
using DeviceHub.Template.Template;
using DeviceHub.YhloTestSerialPort.Protocol;

namespace DeviceHub.YhloTestSerialPort.Handler
{
    public partial class ReceiveHandler : ReceiveHandlerBase
    {
        private long _instrumentId;
        private readonly ReceiveMessageService receiveMessageService = ReceiveMessageService.Instance;
        private readonly ILisClient lisClient = LisClient.Instance;

        public ReceiveHandler(long instrumentId) : base(instrumentId)
        {
            _instrumentId = instrumentId;
        }

        protected override void ParseData(byte[] rawMessage, ReceiveMessage task)
        {
            AstmMessageVerify.VerifyParseResult verifyResult = AstmMessageVerify.VerifyParse(rawMessage);
            if (!verifyResult.Success)
            {
                MarkFailed(task.Id, verifyResult.ErrorMessage);
                return;
            }

            AstmMessageDecode.ParseResult parseResult = AstmMessageDecode.Parse(verifyResult.ParsedRecord);
            if (parseResult.RequestInformationRecord != null)
            {
                SaveSampleQuery(task, parseResult.RequestInformationRecord);
                return;
            }

            string processingId = parseResult.HeaderRecord.ProcessingId;
            if (string.Equals(processingId, nameof(AstmMessageEntity.HeaderRecord.MessageType.QR), StringComparison.OrdinalIgnoreCase))
            {
                UploadQualityControlTestResult(task, parseResult);
                return;
            }

            if (string.Equals(processingId, nameof(AstmMessageEntity.HeaderRecord.MessageType.PR), StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(processingId))
            {
                UploadSpecimenTestResult(task, parseResult);
                return;
            }

            MarkFailed(task.Id, $"不支持消息类型 {processingId}");
        }
    }
}
