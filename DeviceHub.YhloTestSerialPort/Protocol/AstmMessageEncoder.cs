using DeviceHub.Lis.Dto;
using DeviceHub.Template.Constant;
using DeviceHub.Template.Protocol;
using System.Text;
using static DeviceHub.YhloTestSerialPort.Protocol.AstmMessageEntity;

namespace DeviceHub.YhloTestSerialPort.Protocol
{
    /// <summary>
    /// ASTM 样本申请信息编码（QA 查询回应 / SA 主动下发）。
    /// 报文结构：H + {P + {O}} + L，每条记录单独成帧。
    /// </summary>
    public class AstmMessageEncoder
    {
        /// <summary>
        /// 仪器查询后 LIS 返回样本申请信息（H-12=QA，O-26=Q）。
        /// </summary>
        public static List<byte[]> EncoderRequestApplication(GetSampleApplyItemOutput sampleApplyItem, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(sampleApplyItem);
            return EncodeSampleApplication(
                sampleApplyItem,
                nameof(HeaderRecord.MessageType.QA),
                "Q",
                encoding);
        }

        /// <summary>
        /// LIS 主动下发样本申请信息到仪器（H-12=SA，O-26=O）。
        /// </summary>
        public static List<byte[]> EncoderIssueApplication(GetSampleApplyItemOutput sampleApplyItem, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(sampleApplyItem);
            return EncodeSampleApplication(
                sampleApplyItem,
                nameof(HeaderRecord.MessageType.SA),
                "O",
                encoding);
        }

        /// <summary>
        /// 组装 H/P/O/L 记录并封装为 ASTM 帧列表。
        /// </summary>
        private static List<byte[]> EncodeSampleApplication(
            GetSampleApplyItemOutput sample,
            string processingId,
            string reportType,
            Encoding encoding)
        {
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string characterSet = string.IsNullOrEmpty(sample.CharacterSet) ? "ASCII" : sample.CharacterSet;
            string sender = string.IsNullOrEmpty(sample.DeviceModel)
                ? "^^"
                : $"{EscapeComponent(sample.DeviceModel)}^^";

            var records = new List<string>
            {
                // H|\^&|||仪器^版本^序列号|||||||字符集|QA/SA|1394-97|时间
                $"H|\\^&|||{sender}||||||{Escape(characterSet)}|{processingId}|1394-97|{datetime}"
            };

            string status = string.IsNullOrEmpty(sample.QueryResponseStatus)
                ? "OK"
                : sample.QueryResponseStatus;

            if (string.Equals(status, "NF", StringComparison.OrdinalIgnoreCase))
            {
                // 无样本信息：L-3=I
                records.Add("L|1|I");
            }
            else if (string.Equals(status, "AE", StringComparison.OrdinalIgnoreCase)
                     || string.Equals(status, "AR", StringComparison.OrdinalIgnoreCase))
            {
                // 查询出错：L-3=Q
                records.Add("L|1|Q");
            }
            else
            {
                records.Add(BuildPatientRecord(sample));
                records.Add(BuildOrderRecord(sample, reportType));
                records.Add("L|1|N");
            }

            return BuildFrames(records, encoding);
        }

        /// <summary>
        /// P|序号||病历号|住院号|姓名||出生日期^年龄^单位|性别|民族|地址|血型|电话|||医保帐号||||病人类别||||||病区|床号
        /// </summary>
        private static string BuildPatientRecord(GetSampleApplyItemOutput sample)
        {
            string birth = FormatBirthDateOrAge(sample.BirthDate);
            string race = !string.IsNullOrEmpty(sample.Nation) ? sample.Nation : sample.Race;

            var fields = new string[27];
            fields[0] = "P";
            fields[1] = "1";
            fields[2] = string.Empty;
            fields[3] = Escape(sample.MedicalRecordNo);
            fields[4] = Escape(sample.HospitalNo);
            fields[5] = Escape(sample.PatientName);
            fields[6] = string.Empty;
            fields[7] = birth;
            fields[8] = Escape(sample.Sex);
            fields[9] = Escape(race);
            fields[10] = Escape(sample.Address);
            fields[11] = Escape(sample.BloodType);
            fields[12] = Escape(sample.HomePhone);
            fields[13] = string.Empty;
            fields[14] = string.Empty;
            fields[15] = Escape(sample.InsuranceAccount);
            fields[16] = string.Empty;
            fields[17] = string.Empty;
            fields[18] = string.Empty;
            fields[19] = Escape(sample.PatientType);
            fields[20] = string.Empty;
            fields[21] = string.Empty;
            fields[22] = string.Empty;
            fields[23] = string.Empty;
            fields[24] = string.Empty;
            fields[25] = string.Empty;
            fields[26] = Escape(sample.BedNo);

            // 病区无独立字段，床号放 P-27；与手册通信示例 Area|Bed 对齐时 Area 置空
            return string.Join('|', fields);
        }

        /// <summary>
        /// O|序号|样本号^架号^位号|条码|项目列表|优先级|申请时间|采集时间||采集量||||||送检时间|样本类型|送检医生|送检科室|稀释因子||||||||报告类型
        /// </summary>
        private static string BuildOrderRecord(GetSampleApplyItemOutput sample, string reportType)
        {
            string priority = string.Equals(sample.IsEmergency, "Y", StringComparison.OrdinalIgnoreCase)
                ? "S"
                : "R";

            var fields = new string[26];
            fields[0] = "O";
            fields[1] = "1";
            fields[2] = BuildSampleIdField(sample);
            fields[3] = Escape(sample.Barcode);
            fields[4] = BuildAssayField(sample.Items);
            fields[5] = priority;
            fields[6] = Escape(sample.SubmitTime);
            fields[7] = Escape(sample.CollectionTime);
            fields[8] = string.Empty;
            fields[9] = string.Empty;
            fields[10] = string.Empty;
            fields[11] = string.Empty;
            fields[12] = string.Empty;
            fields[13] = string.Empty;
            fields[14] = Escape(sample.SubmitTime);
            fields[15] = Escape(sample.SampleType);
            fields[16] = Escape(sample.RequestDoctor);
            fields[17] = Escape(sample.RequestDept);
            fields[18] = string.Empty;
            fields[19] = string.Empty;
            fields[20] = string.Empty;
            fields[21] = string.Empty;
            fields[22] = string.Empty;
            fields[23] = string.Empty;
            fields[24] = string.Empty;
            fields[25] = reportType;

            return string.Join('|', fields) + "|||||";
        }

        /// <summary>O-3：样本编号^架号^位号</summary>
        private static string BuildSampleIdField(GetSampleApplyItemOutput sample)
        {
            var parts = new List<string>();

            if (!string.IsNullOrEmpty(sample.SampleNo))
                parts.Add(EscapeComponent(sample.SampleNo));

            if (!string.IsNullOrEmpty(sample.SamplePosition))
            {
                foreach (string part in sample.SamplePosition.Split('^'))
                    parts.Add(EscapeComponent(part));
            }

            return string.Join('^', parts);
        }

        /// <summary>O-5：项目通道号^项目名称^稀释倍数^重测标志，多项目用 \ 分隔</summary>
        private static string BuildAssayField(IList<SampleApplyTestItem>? items)
        {
            if (items is null || items.Count == 0)
                return string.Empty;

            return string.Join('\\', items.Select(FormatAssayItem));
        }

        private static string FormatAssayItem(SampleApplyTestItem item)
        {
            return string.Join('^',
                EscapeComponent(item.ItemCode),
                EscapeComponent(item.ItemName),
                EscapeComponent(item.DilutionFactor),
                EscapeComponent(item.RetestFlag));
        }

        /// <summary>
        /// 出生日期字段：支持 yyyyMMdd / yyyyMMddHHmmss，或已含 ^ 的完整值。
        /// </summary>
        private static string FormatBirthDateOrAge(string? birthDate)
        {
            if (string.IsNullOrEmpty(birthDate))
                return string.Empty;

            if (birthDate.Contains('^', StringComparison.Ordinal))
            {
                string[] parts = birthDate.Split('^');
                return string.Join('^', parts.Select(EscapeComponent));
            }

            string date = birthDate.Length >= 8 ? birthDate[..8] : birthDate;
            return EscapeComponent(date);
        }

        /// <summary>
        /// 每条记录单独成帧：中间帧 ETX，末帧 ETB（与手册通信示例一致）。
        /// 帧格式：&lt;STX&gt;FN DATA&lt;CR&gt;&lt;ETX/ETB&gt;CS&lt;CR&gt;&lt;LF&gt;
        /// </summary>
        private static List<byte[]> BuildFrames(List<string> records, Encoding encoding)
        {
            var frames = new List<byte[]>(records.Count);
            for (int i = 0; i < records.Count; i++)
            {
                bool isLast = i == records.Count - 1;
                byte frameNumber = (byte)('1' + (i % 7));
                byte frameEnd = isLast ? ASTMProtocols.ETB : ASTMProtocols.ETX;
                frames.Add(BuildFrame(frameNumber, records[i], frameEnd, encoding));
            }

            return frames;
        }

        private static byte[] BuildFrame(byte frameNumber, string record, byte frameEnd, Encoding encoding)
        {
            byte[] payload = encoding.GetBytes(record);
            // STX + FN + DATA + CR + ETX/ETB + CS(2) + CR + LF
            byte[] frame = new byte[1 + 1 + payload.Length + 1 + 1 + 2 + 2];
            int offset = 0;
            frame[offset++] = ASTMProtocols.STX;
            frame[offset++] = frameNumber;
            Buffer.BlockCopy(payload, 0, frame, offset, payload.Length);
            offset += payload.Length;
            frame[offset++] = ASTMProtocols.CR;
            frame[offset++] = frameEnd;

            byte checksum = AstmMessageVerify.CalculateChecksum(frame.AsSpan(1, offset - 1));
            frame[offset++] = ToHexChar((checksum >> 4) & 0x0F);
            frame[offset++] = ToHexChar(checksum & 0x0F);
            frame[offset++] = ASTMProtocols.CR;
            frame[offset] = ASTMProtocols.LF;
            return frame;
        }

        private static byte ToHexChar(int value) =>
            (byte)(value < 10 ? '0' + value : 'A' + (value - 10));

        /// <summary>字段级转义（保留已有组件/重复分隔符结构时请先拆分再转义）。</summary>
        private static string Escape(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value
                .Replace("&", "&E&", StringComparison.Ordinal)
                .Replace("|", "&F&", StringComparison.Ordinal)
                .Replace("\\", "&R&", StringComparison.Ordinal)
                .Replace("^", "&S&", StringComparison.Ordinal);
        }

        private static string EscapeComponent(string? value) => Escape(value);
    }
}
