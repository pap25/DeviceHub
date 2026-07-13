namespace DeviceHub.YhloTestTcpServer.Protocol;

/// <summary>
/// HL7 应用层消息段实体（MSH/PID/OBR/OBX/QRD/QRF）
/// </summary>
public class Hl7MessageEntity
{
    public class MshSegment
    {
        /// <summary>字段3，发送端应用程序（YHLO）</summary>
        public string SendingApplication { get; set; } = string.Empty;
        /// <summary>字段4，发送端设备（设备型号）</summary>
        public string SendingFacility { get; set; } = string.Empty;
        /// <summary>字段7，消息时间</summary>
        public string DateTimeOfMessage { get; set; } = string.Empty;
        /// <summary>字段9，消息类型（如 ORU^R01、QRY^Q02）</summary>
        public string MessageType { get; set; } = string.Empty;
        /// <summary>字段10，消息控制 ID</summary>
        public string MessageControlId { get; set; } = string.Empty;
        /// <summary>字段11，处理 ID（P）</summary>
        public string ProcessingId { get; set; } = string.Empty;
        /// <summary>字段12，版本 ID（2.3.1）</summary>
        public string VersionId { get; set; } = string.Empty;
        /// <summary>字段16，结果类型（0-病人样本、1-定标、2-质控）</summary>
        public string ApplicationAcknowledgmentType { get; set; } = string.Empty;
        /// <summary>字段18，字符集（ASCII/UTF8）</summary>
        public string CharacterSet { get; set; } = string.Empty;
    }

    public class PidSegment
    {
        /// <summary>字段2，住院号</summary>
        public string PatientId { get; set; } = string.Empty;
        /// <summary>字段3，病历号</summary>
        public string PatientIdentifierList { get; set; } = string.Empty;
        /// <summary>字段5，病人姓名</summary>
        public string PatientName { get; set; } = string.Empty;
        /// <summary>字段8，性别（M/F/O/U）</summary>
        public string Sex { get; set; } = string.Empty;
    }

    public class ObrSegment
    {
        /// <summary>字段1，OBR 序号</summary>
        public string SetId { get; set; } = string.Empty;
        /// <summary>字段2，请求者医嘱号（样本条码号）</summary>
        public string PlacerOrderNumber { get; set; } = string.Empty;
        /// <summary>字段3，执行者医嘱号（样本编号）</summary>
        public string FillerOrderNumber { get; set; } = string.Empty;
        /// <summary>字段4，通用服务标识符（厂商名^型号）</summary>
        public string UniversalServiceId { get; set; } = string.Empty;
        /// <summary>字段6，样本采集时间</summary>
        public string RequestedDateTime { get; set; } = string.Empty;
        /// <summary>字段7，检验时间</summary>
        public string ObservationDateTime { get; set; } = string.Empty;
        /// <summary>字段15，样本类型</summary>
        public string SpecimenSource { get; set; } = string.Empty;
    }

    public class ObxSegment
    {
        /// <summary>字段1，OBX 序号</summary>
        public string SetId { get; set; } = string.Empty;
        /// <summary>字段2，值类型（NM/ST/BOTH）</summary>
        public string ValueType { get; set; } = string.Empty;
        /// <summary>字段3，检验项目通道号（LIS 通道号）</summary>
        public string ObservationIdentifier { get; set; } = string.Empty;
        /// <summary>字段4，检验项目名称</summary>
        public string ObservationSubId { get; set; } = string.Empty;
        /// <summary>字段5，定量检验结果值</summary>
        public string ObservationValue { get; set; } = string.Empty;
        /// <summary>字段6，单位</summary>
        public string Units { get; set; } = string.Empty;
        /// <summary>字段8，异常标志（L/H/N）</summary>
        public string AbnormalFlags { get; set; } = string.Empty;
        /// <summary>字段9，定性检验结果值（阴性/阳性等）</summary>
        public string Probability { get; set; } = string.Empty;
        /// <summary>字段11，观察结果状态（F）</summary>
        public string ResultStatus { get; set; } = string.Empty;
        /// <summary>字段14，检验时间</summary>
        public string ObservationDateTime { get; set; } = string.Empty;
    }

    public class QrdSegment
    {
        /// <summary>字段1，查询时间</summary>
        public string QueryDateTime { get; set; } = string.Empty;
        /// <summary>字段8，查询人过滤符（样本条码）</summary>
        public string SubjectFilter { get; set; } = string.Empty;
    }

    public class QrfSegment
    {
        /// <summary>字段1，设备型号</summary>
        public string WhereSubjectFilter { get; set; } = string.Empty;
        /// <summary>字段2，查询起始时间</summary>
        public string WhenDataStartDateTime { get; set; } = string.Empty;
        /// <summary>字段3，查询结束时间</summary>
        public string WhenDataEndDateTime { get; set; } = string.Empty;
        /// <summary>字段4，样本起始编号</summary>
        public string SampleStartNo { get; set; } = string.Empty;
        /// <summary>字段5，样本终止编号</summary>
        public string SampleEndNo { get; set; } = string.Empty;
    }
}
