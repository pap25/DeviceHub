using System.ComponentModel;

namespace DeviceHub.YhloTestSerialPort.Protocol;

/// <summary>
/// ASTM 应用层消息记录实体（H/P/O/R/C/Q/L）
/// </summary>
public class AstmMessageEntity
{
    /// <summary>
    /// 消息头记录（H 记录）
    /// </summary>
    public class HeaderRecord
    {
        /// <summary>字段1，记录类型 ID（H）</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>字段2，分隔符定义（字段分隔符|、重复分隔符\、成份分隔符^、换码分隔符&amp;）</summary>
        public string DelimiterDefinition { get; set; } = string.Empty;
        /// <summary>字段3，置空，保留</summary>
        public string MessageControlId { get; set; } = string.Empty;
        /// <summary>字段4，置空，保留</summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>字段5，仪器名称^软件版本^仪器序列号</summary>
        public string SenderNameOrId { get; set; } = string.Empty;
        /// <summary>字段5组件1，仪器名称</summary>
        public string InstrumentModel { get; set; } = string.Empty;
        /// <summary>字段5组件2，软件版本</summary>
        public string SoftwareVersion { get; set; } = string.Empty;
        /// <summary>字段5组件3，仪器序列号</summary>
        public string SerialNumber { get; set; } = string.Empty;
        /// <summary>字段6，置空，保留</summary>
        public string SenderStreetAddress { get; set; } = string.Empty;
        /// <summary>字段7，置空，保留</summary>
        public string ReservedField7 { get; set; } = string.Empty;
        /// <summary>字段8，置空，保留</summary>
        public string SenderTelephoneNumber { get; set; } = string.Empty;
        /// <summary>字段9，置空，保留</summary>
        public string CharacteristicsOfSender { get; set; } = string.Empty;
        /// <summary>字段10，置空，保留</summary>
        public string ReceiverId { get; set; } = string.Empty;
        /// <summary>字段11，字符集（ASCII 代表操作系统默认字符编码，WIN7 中文操作系统一般为 GBK；UTF8 代表 UTF8 编码）</summary>
        public string CharacterCodingSet { get; set; } = string.Empty;
        /// <summary>字段12，消息类型（PR-病人测试结果，QR-质控测试结果，CR-定标结果，RQ-样本请求查询，QA-样本查询回应，SA-样本申请信息）</summary>
        public string ProcessingId { get; set; } = string.Empty;
        /// <summary>字段13，协议版本编号，固定为 1394-97</summary>
        public string VersionNumber { get; set; } = string.Empty;
        /// <summary>字段14，消息创建的日期和时间（格式 YYYYMMDDHHMMSS）</summary>
        public string DateAndTime { get; set; } = string.Empty;

        public enum MessageType
        {
            [Description("病人检验结果")]
            PR,

            [Description("质控检验结果")]
            QR,

            [Description("定标结果")]
            CR,

            [Description("去LIS查询样本信息")]
            RQ,
            [Description("返回查询结果(RQ的返回)")]
            QA,

            [Description("样本申请信息")]
            SA
        }
    }

    /// <summary>
    /// 患者信息记录（P 记录）
    /// </summary>
    public class PatientRecord
    {
        /// <summary>字段1，记录类型 ID（P）</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>字段2，序列号（指示病人信息记录在当前层的序列号，从 1 起连续递增）</summary>
        public string SequenceNumber { get; set; } = string.Empty;
        /// <summary>字段3，置空，保留</summary>
        public string PracticeAssignedPatientId { get; set; } = string.Empty;
        /// <summary>字段4，病历号</summary>
        public string PatientId { get; set; } = string.Empty;
        /// <summary>字段5，住院号</summary>
        public string PatientId3 { get; set; } = string.Empty;
        /// <summary>字段6，病人姓名</summary>
        public string PatientName { get; set; } = string.Empty;
        /// <summary>字段7，置空，保留</summary>
        public string ReservedField7 { get; set; } = string.Empty;
        /// <summary>字段8，出生日期^年龄^年龄单位（Y-年，M-月，W-周，D-天，H-小时，空默认年）</summary>
        public string BirthDateOrAge { get; set; } = string.Empty;
        /// <summary>字段9，病人性别（M-女，F-男，U-未知）</summary>
        public string PatientSex { get; set; } = string.Empty;
        /// <summary>字段10，民族</summary>
        public string PatientRace { get; set; } = string.Empty;
        /// <summary>字段11，病人地址</summary>
        public string PatientAddress { get; set; } = string.Empty;
        /// <summary>字段12，血型（A/B/O/AB/其他自定义）</summary>
        public string ReservedField12 { get; set; } = string.Empty;
        /// <summary>字段13，联系方式</summary>
        public string PatientTelephone { get; set; } = string.Empty;
        /// <summary>字段14，置空，保留</summary>
        public string AttendingPhysicianName { get; set; } = string.Empty;
        /// <summary>字段15，置空，保留</summary>
        public string SpecialField1 { get; set; } = string.Empty;
        /// <summary>字段16，医保帐号</summary>
        public string BodySurfaceArea { get; set; } = string.Empty;
        /// <summary>字段17，置空，保留</summary>
        public string PatientHeight { get; set; } = string.Empty;
        /// <summary>字段18，置空，保留</summary>
        public string PatientWeight { get; set; } = string.Empty;
        /// <summary>字段19，置空，保留</summary>
        public string PatientDiagnosis { get; set; } = string.Empty;
        /// <summary>字段20，置空，保留</summary>
        public string PatientMedications { get; set; } = string.Empty;
        /// <summary>字段21，血袋编号</summary>
        public string PatientDiet { get; set; } = string.Empty;
        /// <summary>字段22，置空，保留</summary>
        public string PracticeField1 { get; set; } = string.Empty;
        /// <summary>字段23，置空，保留</summary>
        public string PracticeField2 { get; set; } = string.Empty;
        /// <summary>字段25，置空，保留</summary>
        public string AdmissionStatus { get; set; } = string.Empty;
        /// <summary>字段26，病区</summary>
        public string Location { get; set; } = string.Empty;
    }

    /// <summary>
    /// 检验申请记录（O 记录）
    /// </summary>
    public class TestOrderRecord
    {
        /// <summary>字段1，记录类型 ID（O）</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>字段2，序列号（指示测试订单记录在当前层的序列号，从 1 起连续递增）</summary>
        public string SequenceNumber { get; set; } = string.Empty;
        /// <summary>字段3，样本编号^样本架号/盘号^样本位置号（LIS 下发时仪器忽略架号/位置）</summary>
        public string SampleId { get; set; } = string.Empty;
        /// <summary>字段3组件1，样本架号/样本盘号</summary>
        public string SampleTrayNo { get; set; } = string.Empty;
        /// <summary>字段3组件2，样本在架上/盘上的位置</summary>
        public string SamplePosNo { get; set; } = string.Empty;
        /// <summary>字段4，样本条码</summary>
        public string InstrumentSpecimenId { get; set; } = string.Empty;
        /// <summary>字段5，项目通道号^项目名称^稀释倍数^重测标志（多项目用\分隔，最多 160 个）</summary>
        public string AssayNo { get; set; } = string.Empty;
        /// <summary>字段6，是否急诊（R-常规，S-STAT 测试）</summary>
        public string Priority { get; set; } = string.Empty;
        /// <summary>字段7，样本：申请时间；质控：质控时间（格式 YYYYMMDDHHMMSS）</summary>
        public string RequestedDateTime { get; set; } = string.Empty;
        /// <summary>字段8，样本采集时间（格式 YYYYMMDDHHMMSS）</summary>
        public string SpecimenCollectionDateTime { get; set; } = string.Empty;
        /// <summary>字段9，置空，保留</summary>
        public string CollectionEndTime { get; set; } = string.Empty;
        /// <summary>字段10，采集量</summary>
        public string CollectionVolume { get; set; } = string.Empty;
        /// <summary>字段11，采集者</summary>
        public string CollectedBy { get; set; } = string.Empty;
        /// <summary>字段12，样本：置空；质控：质控液编号^名称^批号^有效期^均值^浓度水平^标准差^结果值（多液用\分隔）</summary>
        public string ActionCode { get; set; } = string.Empty;
        /// <summary>字段13，置空，保留</summary>
        public string DangerCode { get; set; } = string.Empty;
        /// <summary>字段14，置空，保留</summary>
        public string RelevantClinicalInformation { get; set; } = string.Empty;
        /// <summary>字段15，送检时间（格式 YYYYMMDDHHMMSS）</summary>
        public string SpecimenReceivedDateTime { get; set; } = string.Empty;
        /// <summary>字段16，样本类型（serum/urine/CSF/plasma/timed/other/blood/amniotic/urethral/saliva/cervical/synovial，大小写敏感）</summary>
        public string SpecimenType { get; set; } = string.Empty;
        /// <summary>字段17，送检医生</summary>
        public string OrderingPhysician { get; set; } = string.Empty;
        /// <summary>字段18，送检科室</summary>
        public string PhysicianPhoneNumber { get; set; } = string.Empty;
        /// <summary>字段19，手工稀释因子</summary>
        public string OfflineDilutionFactor { get; set; } = string.Empty;
        /// <summary>字段20，检验医生</summary>
        public string UserField2 { get; set; } = string.Empty;
        /// <summary>字段21，置空，保留</summary>
        public string LaboratoryField1 { get; set; } = string.Empty;
        /// <summary>字段22，置空，保留</summary>
        public string LaboratoryField2 { get; set; } = string.Empty;
        /// <summary>字段23，置空，保留</summary>
        public string ResultsReportedDateTime { get; set; } = string.Empty;
        /// <summary>字段24，置空，保留</summary>
        public string InstrumentChargeToComputer { get; set; } = string.Empty;
        /// <summary>字段25，置空，保留</summary>
        public string InstrumentSectionId { get; set; } = string.Empty;
        /// <summary>字段26，报告类型（O-来自 LIS 的请求，Q-查询响应，F-最终结果）</summary>
        public string ReportType { get; set; } = string.Empty;
        /// <summary>字段27，置空，保留</summary>
        public string ReservedField27 { get; set; } = string.Empty;
        /// <summary>字段28，置空，保留</summary>
        public string Location { get; set; } = string.Empty;
        /// <summary>字段29，置空，保留</summary>
        public string NosocomialInfectionFlag { get; set; } = string.Empty;
        /// <summary>字段30，置空，保留</summary>
        public string SpecimenService { get; set; } = string.Empty;
        /// <summary>字段31，置空，保留</summary>
        public string SpecimenInstitution { get; set; } = string.Empty;
    }

    /// <summary>
    /// 检验结果记录（R 记录）
    /// </summary>
    public class ResultRecord
    {
        /// <summary>字段1，记录类型 ID（R）</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>字段2，序列号</summary>
        public string SequenceNumber { get; set; } = string.Empty;
        /// <summary>字段3，项目通道号^项目名称^结果重复次数编号^结果类型（I-定性，F-定量，B-定量+定性）</summary>
        public string AssayNo { get; set; } = string.Empty;
        /// <summary>字段3组件2，项目名称</summary>
        public string AssayName { get; set; } = string.Empty;
        /// <summary>字段3组件3，结果重复次数编号</summary>
        public string ReplicateNumber { get; set; } = string.Empty;
        /// <summary>字段3组件4，结果类型（I-定性结果，F-定量结果，B-定量结果+定性描述）</summary>
        public string ResultType { get; set; } = string.Empty;
        /// <summary>字段4，测量值（F/B 时为浓度值，可含 &lt;/&gt; 标记；I/B 时为定性结果如阴性(-)、阳性(+)）</summary>
        public string MeasurementValue { get; set; } = string.Empty;
        /// <summary>字段5，结果单位</summary>
        public string Unit { get; set; } = string.Empty;
        /// <summary>字段6，参考范围上限^参考范围下限</summary>
        public string ReferenceRange { get; set; } = string.Empty;
        /// <summary>字段7，异常结果标记（L-偏低，H-偏高，N-正常）</summary>
        public string AbnormalFlag { get; set; } = string.Empty;
        /// <summary>字段8，定性参考值（结果类型为 I 或 B 时有效）</summary>
        public string NatureOfAbnormalTest { get; set; } = string.Empty;
        /// <summary>字段9，结果状态（F-最终结果）</summary>
        public string ResultStatus { get; set; } = string.Empty;
        /// <summary>字段10，结果追溯信息（发光值^试剂批号^试剂瓶号^试剂规格^校准时间^预激发液批号^预激发液瓶号^激发液批号^激发液瓶号^稀释液批号^稀释液瓶号）</summary>
        public string DateOfChangeInInstrumentNormativeValues { get; set; } = string.Empty;
        /// <summary>字段11，置空，保留</summary>
        public string OperatorIdentification { get; set; } = string.Empty;
        /// <summary>字段12，置空，保留</summary>
        public string TestStartedDateTime { get; set; } = string.Empty;
        /// <summary>字段13，测试完成时间（格式 YYYYMMDDHHMMSS）</summary>
        public string TestCompletedDateTime { get; set; } = string.Empty;
        /// <summary>字段14，测试模块（级联产品中表示级联中哪一个模块测试的，例如 M1）</summary>
        public string InstrumentIdentification { get; set; } = string.Empty;
        /// <summary>字段15，置空，保留</summary>
        public string ReservedField16 { get; set; } = string.Empty;
        /// <summary>字段16，置空，保留</summary>
        public string ReservedField17 { get; set; } = string.Empty;
    }

    /// <summary>
    /// 结果注释记录（C 记录）
    /// </summary>
    public class ResultsCommentRecord
    {
        /// <summary>字段1，记录类型 ID（C）</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>字段2，序列号</summary>
        public string SequenceNumber { get; set; } = string.Empty;
        /// <summary>字段3，注释来源（I-分析仪发送，L-LIS 主机发送）</summary>
        public string CommentSource { get; set; } = string.Empty;
        /// <summary>字段4，注释文本</summary>
        public string CommentText { get; set; } = string.Empty;
        /// <summary>字段5，注释类型（G-结果注释，I-异常字符串）</summary>
        public string CommentType { get; set; } = string.Empty;
    }

    /// <summary>
    /// 请求信息记录（Q 记录）
    /// </summary>
    public class RequestInformationRecord
    {
        /// <summary>字段1，记录类型 ID（Q）</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>字段2，序列号（1-n）</summary>
        public string SequenceNumber { get; set; } = string.Empty;
        /// <summary>字段3，病人 ID^样本 ID（样本起始编号^样本条码）</summary>
        public string PatientIdAndSpecimenId { get; set; } = string.Empty;
        /// <summary>字段3组件1，病人 ID / 样本起始编号（最长 20）</summary>
        public string PatientId { get; set; } = string.Empty;
        /// <summary>字段3组件2，样本 ID / 样本条码（最长 29）</summary>
        public string SpecimenId { get; set; } = string.Empty;
        /// <summary>字段4，样本结束编号（查询单个样本时与样本起始编号相同，最长 20）</summary>
        public string EndingRangeId { get; set; } = string.Empty;
        /// <summary>字段5，通用测试 ID（系统总是请求所有已发出未清订单，固定为 ALL）</summary>
        public string UniversalTestId { get; set; } = string.Empty;
        /// <summary>字段6，请求时限性质，置空，保留</summary>
        public string NatureOfRequestTimeLimits { get; set; } = string.Empty;
        /// <summary>字段7，查询起始时间（格式 YYYYMMDDHHMMSS，最长 14）</summary>
        public string BeginningRequestResultsDateTime { get; set; } = string.Empty;
        /// <summary>字段8，查询截止时间（格式 YYYYMMDDHHMMSS，最长 14）</summary>
        public string EndingRequestResultsDateTime { get; set; } = string.Empty;
        /// <summary>字段9，申请医生姓名，置空，保留</summary>
        public string RequestingPhysicianName { get; set; } = string.Empty;
        /// <summary>字段10，申请医生电话，置空，保留</summary>
        public string RequestingPhysicianTelephoneNumber { get; set; } = string.Empty;
        /// <summary>字段11，用户字段 1，置空，保留</summary>
        public string UserField1 { get; set; } = string.Empty;
        /// <summary>字段12，用户字段 2，置空，保留</summary>
        public string UserField2 { get; set; } = string.Empty;
        /// <summary>字段13，查询命令码（O-请求样本查询，A-取消当前查询请求）</summary>
        public string RequestInformationStatusCode { get; set; } = string.Empty;

        public enum RequestInformationStatus
        {
            [Description("请求样本查询")]
            O,

            [Description("取消当前查询请求")]
            A
        }
    }

    /// <summary>
    /// 消息终止记录（L 记录）
    /// </summary>
    public class MessageTerminatorRecord
    {
        /// <summary>字段1，记录类型 ID（L）</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>字段2，序列号</summary>
        public string SequenceNumber { get; set; } = string.Empty;
        /// <summary>字段3，结束码（N-正常终止，I-上次查询无信息，Q-上次请求出错）</summary>
        public string TerminationCode { get; set; } = string.Empty;
    }
}
