using DeviceHub.Lis.Dto;
using DeviceHub.Template.Constant;
using DeviceHub.Utils;
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
        public static byte[]? EncoderAck(MshSegment msh, Encoding encoding)
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

            return WrapMllp(sb.ToString(), encoding);
        }

        /// <summary>
        /// 仪器向 LIS 查询样本后，LIS 应答样本申请信息（DSR^Q03，MSH-15 非 P）。
        /// </summary>
        public static byte[] EncoderRequestApplication(GetSampleApplyItemOutput getSampleApplyItemOutput, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(getSampleApplyItemOutput);
            return EncodeDsr(getSampleApplyItemOutput, isProactiveIssue: false, encoding);
        }

        /// <summary>
        /// LIS 服务器主动下发样本申请信息到仪器（DSR^Q03，MSH-15=P）。
        /// </summary>
        public static byte[] EncoderIssueApplication(GetSampleApplyItemOutput getSampleApplyItemOutput, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(getSampleApplyItemOutput);
            return EncodeDsr(getSampleApplyItemOutput, isProactiveIssue: true, encoding);
        }

        /// <summary>
        /// 组装 DSR^Q03：MSH + MSA + ERR + QAK + QRD + QRF + {DSP} + DSC。
        /// </summary>
        private static byte[] EncodeDsr(GetSampleApplyItemOutput sample, bool isProactiveIssue, Encoding encoding)
        {
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string messageControlId = !string.IsNullOrEmpty(sample.MessageControlId)
                ? sample.MessageControlId
                : sample.Id > 0
                    ? sample.Id.ToString()
                    : "1";
            string characterSet = string.IsNullOrEmpty(sample.CharacterSet) ? "ASCII" : sample.CharacterSet;
            string deviceModel = sample.DeviceModel ?? string.Empty;
            string qakStatus = string.IsNullOrEmpty(sample.QueryResponseStatus) ? "OK" : sample.QueryResponseStatus;
            string acceptAckType = isProactiveIssue ? "P" : string.Empty;

            var sb = new StringBuilder();

            // MSH|^~\&|||||时间||DSR^Q03|控制ID|P|2.3.1|||AcceptAck||CharacterSet|||
            // 主动下发时 MSH-15=P
            sb.Append("MSH|^~\\&|||||");
            sb.Append(datetime);
            sb.Append("||DSR^Q03|");
            sb.Append(messageControlId);
            sb.Append("|P|2.3.1|||");
            sb.Append(acceptAckType);
            sb.Append("||");
            sb.Append(characterSet);
            sb.Append("|||");
            sb.Append('\r');

            sb.Append("MSA|AA|");
            sb.Append(messageControlId);
            sb.Append("|Messageaccepted|||0|");
            sb.Append('\r');

            sb.Append("ERR|0|\r");

            sb.Append("QAK|SR|");
            sb.Append(qakStatus);
            sb.Append("|\r");

            // QRD|时间|R|D|控制ID|||RD||OTH|||T|
            sb.Append("QRD|");
            sb.Append(datetime);
            sb.Append("|R|D|");
            sb.Append(messageControlId);
            sb.Append("|||RD||OTH|||T|");
            sb.Append('\r');

            // QRF|设备型号|||||RCT|COR|ALL||
            sb.Append("QRF|");
            sb.Append(Escape(deviceModel));
            sb.Append("|||||RCT|COR|ALL||");
            sb.Append('\r');

            if (!string.Equals(qakStatus, "NF", StringComparison.OrdinalIgnoreCase))
            {
                AppendDspSegments(sb, sample);
            }

            // DSC|连续指针| ；空表示最后一条
            sb.Append("DSC|");
            sb.Append(Escape(sample.ContinuationPointer ?? string.Empty));
            sb.Append('|');
            sb.Append('\r');

            return WrapMllp(sb.ToString(), encoding);
        }

        private static void AppendDspSegments(StringBuilder sb, GetSampleApplyItemOutput sample)
        {
            string emergency = string.IsNullOrEmpty(sample.IsEmergency) ? "N" : sample.IsEmergency;

            AppendDsp(sb, 1, sample.HospitalNo);
            AppendDsp(sb, 2, sample.BedNo);
            AppendDsp(sb, 3, sample.PatientName);
            AppendDsp(sb, 4, sample.BirthDate);
            AppendDsp(sb, 5, sample.Sex);
            AppendDsp(sb, 6, sample.BloodType);
            AppendDsp(sb, 7, sample.Race);
            AppendDsp(sb, 8, sample.Address);
            AppendDsp(sb, 9, sample.ZipCode);
            AppendDsp(sb, 10, sample.HomePhone);
            AppendDsp(sb, 11, sample.SamplePosition);
            AppendDsp(sb, 12, sample.CollectionTime);
            AppendDsp(sb, 13, sample.MedicalRecordNo);
            AppendDsp(sb, 14, sample.ActionCode);
            AppendDsp(sb, 15, sample.PatientType);
            AppendDsp(sb, 16, sample.InsuranceAccount);
            AppendDsp(sb, 17, sample.FeeType);
            AppendDsp(sb, 18, sample.Nation);
            AppendDsp(sb, 19, sample.NativePlace);
            AppendDsp(sb, 20, sample.Country);
            AppendDsp(sb, 21, sample.Barcode);
            AppendDsp(sb, 22, sample.SampleNo);
            AppendDsp(sb, 23, sample.SubmitTime);
            AppendDsp(sb, 24, emergency);
            AppendDsp(sb, 25, string.Empty);
            AppendDsp(sb, 26, sample.SampleType);
            AppendDsp(sb, 27, sample.RequestDoctor);
            AppendDsp(sb, 28, sample.RequestDept);

            IList<SampleApplyTestItem> items = sample.Items ?? [];
            for (int i = 0; i < items.Count; i++)
            {
                AppendDsp(sb, 29 + i, FormatTestItem(items[i]));
            }
        }

        private static void AppendDsp(StringBuilder sb, int setId, string? dataLine)
        {
            sb.Append("DSP|");
            sb.Append(setId);
            sb.Append("||");
            sb.Append(Escape(dataLine ?? string.Empty));
            sb.Append("|||");
            sb.Append('\r');
        }

        private static string FormatTestItem(SampleApplyTestItem item)
        {
            // 项目通道号^项目名称^单位^参考范围^稀释倍数^重测标志^指定测试模块^指定试剂批号^指定试剂瓶
            // 各组件先转义，再以 ^ 连接（^ 为组件分隔符，不转义）
            return string.Join('^',
                EscapeComponent(item.ItemCode),
                EscapeComponent(item.ItemName),
                EscapeComponent(item.Unit),
                EscapeComponent(item.ReferenceRange),
                EscapeComponent(item.DilutionFactor),
                EscapeComponent(item.RetestFlag),
                EscapeComponent(item.TestModule),
                EscapeComponent(item.ReagentLotNo),
                EscapeComponent(item.ReagentBottleNo));
        }

        /// <summary>Data Line 整体转义；保留已有组件分隔符 ^（如样本位 架号^位号）。</summary>
        private static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value
                .Replace("\\", "\\E\\", StringComparison.Ordinal)
                .Replace("|", "\\F\\", StringComparison.Ordinal)
                .Replace("&", "\\T\\", StringComparison.Ordinal)
                .Replace("~", "\\R\\", StringComparison.Ordinal)
                .Replace("\r", "\\.br\\", StringComparison.Ordinal);
        }

        private static string EscapeComponent(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return Escape(value).Replace("^", "\\S\\", StringComparison.Ordinal);
        }

        private static byte[] WrapMllp(string message, Encoding encoding)
        {
            byte[] body = encoding.GetBytes(message);
            byte[] framed = new byte[body.Length + 3];
            framed[0] = HL7Protocols.VT;
            Buffer.BlockCopy(body, 0, framed, 1, body.Length);
            framed[^2] = HL7Protocols.EB;
            framed[^1] = HL7Protocols.CR;
            return framed;
        }
    }
}
