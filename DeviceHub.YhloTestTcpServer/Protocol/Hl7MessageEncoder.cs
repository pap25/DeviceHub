using DeviceHub.Base.Constant;
using DeviceHub.Lis.Dto;
using System.Text;
using static DeviceHub.YhloTestTcpServer.Protocol.Hl7MessageEntity;

namespace DeviceHub.YhloTestTcpServer.Protocol
{
    public class Hl7MessageEncoder
    {
        /// <summary>
        /// 根据仪器上报 MSH 生成应答：ORU→ACK^R01（MSH+MSA），QRY→QCK^Q02（MSH+MSA+ERR+QAK）。
        /// 仪器发来的 ACK 无需再应答，返回 null。
        /// </summary>
        public static byte[]? EncoderAck(MshSegment msh)
        {
            if (msh is null || string.IsNullOrEmpty(msh.MessageType))
                return null;

            string messageType = msh.MessageType;
            if (messageType.StartsWith("ACK", StringComparison.OrdinalIgnoreCase))
                return null;

            string responseType;
            bool isQueryAck;
            if (messageType.StartsWith("ORU", StringComparison.OrdinalIgnoreCase))
            {
                responseType = "ACK^R01";
                isQueryAck = false;
            }
            else if (messageType.StartsWith("QRY", StringComparison.OrdinalIgnoreCase))
            {
                responseType = "QCK^Q02";
                isQueryAck = true;
            }
            else
            {
                return null;
            }

            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string versionId = string.IsNullOrEmpty(msh.VersionId) ? "2.3.1" : msh.VersionId;
            string processingId = string.IsNullOrEmpty(msh.ProcessingId) ? "P" : msh.ProcessingId;
            string characterSet = string.IsNullOrEmpty(msh.CharacterSet) ? "ASCII" : msh.CharacterSet;
            string appAckType = isQueryAck ? string.Empty : msh.ApplicationAcknowledgmentType;

            var sb = new StringBuilder();
            // MSH|^~\&|||YHLO|设备型号|时间||ACK^R01|控制ID|P|2.3.1||||结果类型||字符集|||
            sb.Append("MSH|^~\\&|||");
            sb.Append(msh.SendingApplication);
            sb.Append('|');
            sb.Append(msh.SendingFacility);
            sb.Append('|');
            sb.Append(datetime);
            sb.Append("||");
            sb.Append(responseType);
            sb.Append('|');
            sb.Append(msh.MessageControlId);
            sb.Append('|');
            sb.Append(processingId);
            sb.Append('|');
            sb.Append(versionId);
            sb.Append("||||");
            sb.Append(appAckType);
            sb.Append("||");
            sb.Append(characterSet);
            sb.Append("|||");
            sb.Append('\r');

            // MSA|AA|MessageControlID|Messageaccepted|||0|
            sb.Append("MSA|AA|");
            sb.Append(msh.MessageControlId);
            sb.Append("|Messageaccepted|||0|");
            sb.Append('\r');

            if (isQueryAck)
            {
                sb.Append("ERR|0|\r");
                sb.Append("QAK|SR|OK|\r");
            }

            return WrapMllp(sb.ToString());
        }

        public static byte[] EncoderRequestApplication(GetSampleApplyItemOutput getSampleApplyItemOutput)
        {
            throw new NotImplementedException();
        }

        public static byte[] EncoderIssueApplication(GetSampleApplyListOutput getSampleApplyListOutput)
        {
            throw new NotImplementedException();
        }

        private static byte[] WrapMllp(string message)
        {
            byte[] body = Encoding.UTF8.GetBytes(message);
            byte[] framed = new byte[body.Length + 3];
            framed[0] = HL7Protocols.VT;
            Buffer.BlockCopy(body, 0, framed, 1, body.Length);
            framed[^2] = HL7Protocols.EB;
            framed[^1] = HL7Protocols.CR;
            return framed;
        }
    }
}
