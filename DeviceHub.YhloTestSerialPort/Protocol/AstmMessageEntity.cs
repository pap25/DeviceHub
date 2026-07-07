namespace DeviceHub.YhloTestSerialPort.Protocol;

/// <summary>
/// ASTM 应用层消息记录实体（H/P/O/R/C/L）
/// </summary>
public class AstmMessageEntity
{
    /// <summary>
    /// 消息头记录（H 记录）
    /// </summary>
    public class HeaderRecord
    {
        /// <summary>记录类型标识</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>分隔符定义</summary>
        public string DelimiterDefinition { get; set; } = string.Empty;
        /// <summary>消息控制 ID</summary>
        public string MessageControlId { get; set; } = string.Empty;
        /// <summary>密码</summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>发送方名称或 ID</summary>
        public string SenderNameOrId { get; set; } = string.Empty;
        /// <summary>仪器型号</summary>
        public string InstrumentModel { get; set; } = string.Empty;
        /// <summary>软件版本</summary>
        public string SoftwareVersion { get; set; } = string.Empty;
        /// <summary>序列号</summary>
        public string SerialNumber { get; set; } = string.Empty;
        /// <summary>发送方街道地址</summary>
        public string SenderStreetAddress { get; set; } = string.Empty;
        /// <summary>保留字段 7</summary>
        public string ReservedField7 { get; set; } = string.Empty;
        /// <summary>发送方电话号码</summary>
        public string SenderTelephoneNumber { get; set; } = string.Empty;
        /// <summary>发送方特征</summary>
        public string CharacteristicsOfSender { get; set; } = string.Empty;
        /// <summary>接收方 ID</summary>
        public string ReceiverId { get; set; } = string.Empty;
        /// <summary>字符编码集</summary>
        public string CharacterCodingSet { get; set; } = string.Empty;
        /// <summary>处理标识</summary>
        public string ProcessingId { get; set; } = string.Empty;
        /// <summary>版本号</summary>
        public string VersionNumber { get; set; } = string.Empty;
        /// <summary>字符集</summary>
        public string CharacterSet { get; set; } = string.Empty;
        /// <summary>日期和时间</summary>
        public string DateAndTime { get; set; } = string.Empty;
    }

    /// <summary>
    /// 患者信息记录（P 记录）
    /// </summary>
    public class PatientRecord
    {
        /// <summary>记录类型标识</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>序号</summary>
        public string SequenceNumber { get; set; } = string.Empty;
        /// <summary>医疗机构分配的患者 ID</summary>
        public string PracticeAssignedPatientId { get; set; } = string.Empty;
        /// <summary>患者 ID</summary>
        public string PatientId { get; set; } = string.Empty;
        /// <summary>患者 ID（第 3 字段）</summary>
        public string PatientId3 { get; set; } = string.Empty;
        /// <summary>患者姓名</summary>
        public string PatientName { get; set; } = string.Empty;
        /// <summary>保留字段 7</summary>
        public string ReservedField7 { get; set; } = string.Empty;
        /// <summary>出生日期或年龄</summary>
        public string BirthDateOrAge { get; set; } = string.Empty;
        /// <summary>患者性别</summary>
        public string PatientSex { get; set; } = string.Empty;
        /// <summary>患者种族</summary>
        public string PatientRace { get; set; } = string.Empty;
        /// <summary>患者地址</summary>
        public string PatientAddress { get; set; } = string.Empty;
        /// <summary>保留字段 12</summary>
        public string ReservedField12 { get; set; } = string.Empty;
        /// <summary>患者电话</summary>
        public string PatientTelephone { get; set; } = string.Empty;
        /// <summary>主治医生姓名</summary>
        public string AttendingPhysicianName { get; set; } = string.Empty;
        /// <summary>特殊字段 1</summary>
        public string SpecialField1 { get; set; } = string.Empty;
        /// <summary>体表面积</summary>
        public string BodySurfaceArea { get; set; } = string.Empty;
        /// <summary>患者身高</summary>
        public string PatientHeight { get; set; } = string.Empty;
        /// <summary>患者体重</summary>
        public string PatientWeight { get; set; } = string.Empty;
        /// <summary>患者诊断</summary>
        public string PatientDiagnosis { get; set; } = string.Empty;
        /// <summary>患者用药</summary>
        public string PatientMedications { get; set; } = string.Empty;
        /// <summary>患者饮食</summary>
        public string PatientDiet { get; set; } = string.Empty;
        /// <summary>医疗机构字段 1</summary>
        public string PracticeField1 { get; set; } = string.Empty;
        /// <summary>医疗机构字段 2</summary>
        public string PracticeField2 { get; set; } = string.Empty;
        /// <summary>入院状态</summary>
        public string AdmissionStatus { get; set; } = string.Empty;
        /// <summary>位置</summary>
        public string Location { get; set; } = string.Empty;
    }

    /// <summary>
    /// 检验申请记录（O 记录）
    /// </summary>
    public class TestOrderRecord
    {
        /// <summary>记录类型标识</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>序号</summary>
        public string SequenceNumber { get; set; } = string.Empty;
        /// <summary>样本 ID</summary>
        public string SampleId { get; set; } = string.Empty;
        /// <summary>样本盘号</summary>
        public string SampleTrayNo { get; set; } = string.Empty;
        /// <summary>样本位置号</summary>
        public string SamplePosNo { get; set; } = string.Empty;
        /// <summary>仪器标本 ID</summary>
        public string InstrumentSpecimenId { get; set; } = string.Empty;
        /// <summary>检测项目编号</summary>
        public string AssayNo { get; set; } = string.Empty;
        /// <summary>优先级</summary>
        public string Priority { get; set; } = string.Empty;
        /// <summary>申请日期时间</summary>
        public string RequestedDateTime { get; set; } = string.Empty;
        /// <summary>标本采集日期时间</summary>
        public string SpecimenCollectionDateTime { get; set; } = string.Empty;
        /// <summary>采集结束时间</summary>
        public string CollectionEndTime { get; set; } = string.Empty;
        /// <summary>采集量</summary>
        public string CollectionVolume { get; set; } = string.Empty;
        /// <summary>采集人</summary>
        public string CollectedBy { get; set; } = string.Empty;
        /// <summary>操作代码</summary>
        public string ActionCode { get; set; } = string.Empty;
        /// <summary>危险代码</summary>
        public string DangerCode { get; set; } = string.Empty;
        /// <summary>相关临床信息</summary>
        public string RelevantClinicalInformation { get; set; } = string.Empty;
        /// <summary>标本接收日期时间</summary>
        public string SpecimenReceivedDateTime { get; set; } = string.Empty;
        /// <summary>标本类型</summary>
        public string SpecimenType { get; set; } = string.Empty;
        /// <summary>申请医生</summary>
        public string OrderingPhysician { get; set; } = string.Empty;
        /// <summary>医生电话号码</summary>
        public string PhysicianPhoneNumber { get; set; } = string.Empty;
        /// <summary>离线稀释因子</summary>
        public string OfflineDilutionFactor { get; set; } = string.Empty;
        /// <summary>用户字段 2</summary>
        public string UserField2 { get; set; } = string.Empty;
        /// <summary>实验室字段 1</summary>
        public string LaboratoryField1 { get; set; } = string.Empty;
        /// <summary>实验室字段 2</summary>
        public string LaboratoryField2 { get; set; } = string.Empty;
        /// <summary>结果报告日期时间</summary>
        public string ResultsReportedDateTime { get; set; } = string.Empty;
        /// <summary>仪器计费到计算机</summary>
        public string InstrumentChargeToComputer { get; set; } = string.Empty;
        /// <summary>仪器部门 ID</summary>
        public string InstrumentSectionId { get; set; } = string.Empty;
        /// <summary>报告类型</summary>
        public string ReportType { get; set; } = string.Empty;
        /// <summary>保留字段 27</summary>
        public string ReservedField27 { get; set; } = string.Empty;
        /// <summary>位置</summary>
        public string Location { get; set; } = string.Empty;
        /// <summary>医院感染标志</summary>
        public string NosocomialInfectionFlag { get; set; } = string.Empty;
        /// <summary>标本服务</summary>
        public string SpecimenService { get; set; } = string.Empty;
        /// <summary>标本机构</summary>
        public string SpecimenInstitution { get; set; } = string.Empty;
    }

    /// <summary>
    /// 检验结果记录（R 记录）
    /// </summary>
    public class ResultRecord
    {
        /// <summary>记录类型标识</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>序号</summary>
        public string SequenceNumber { get; set; } = string.Empty;
        /// <summary>检测项目编号</summary>
        public string AssayNo { get; set; } = string.Empty;
        /// <summary>检测项目名称</summary>
        public string AssayName { get; set; } = string.Empty;
        /// <summary>重复次数编号</summary>
        public string ReplicateNumber { get; set; } = string.Empty;
        /// <summary>结果类型（I=仪器原始值，F=最终结果，B=两者）</summary>
        public string ResultType { get; set; } = string.Empty;
        /// <summary>测量值</summary>
        public string MeasurementValue { get; set; } = string.Empty;
        /// <summary>单位</summary>
        public string Unit { get; set; } = string.Empty;
        /// <summary>参考范围</summary>
        public string ReferenceRange { get; set; } = string.Empty;
        /// <summary>异常标志</summary>
        public string AbnormalFlag { get; set; } = string.Empty;
        /// <summary>异常检测性质</summary>
        public string NatureOfAbnormalTest { get; set; } = string.Empty;
        /// <summary>结果状态</summary>
        public string ResultStatus { get; set; } = string.Empty;
        /// <summary>仪器参考值变更日期</summary>
        public string DateOfChangeInInstrumentNormativeValues { get; set; } = string.Empty;
        /// <summary>操作员标识</summary>
        public string OperatorIdentification { get; set; } = string.Empty;
        /// <summary>检测开始日期时间</summary>
        public string TestStartedDateTime { get; set; } = string.Empty;
        /// <summary>检测完成日期时间</summary>
        public string TestCompletedDateTime { get; set; } = string.Empty;
        /// <summary>仪器标识</summary>
        public string InstrumentIdentification { get; set; } = string.Empty;
        /// <summary>保留字段 16</summary>
        public string ReservedField16 { get; set; } = string.Empty;
        /// <summary>保留字段 17</summary>
        public string ReservedField17 { get; set; } = string.Empty;
    }

    /// <summary>
    /// 结果注释记录（C 记录）
    /// </summary>
    public class ResultsCommentRecord
    {
        /// <summary>记录类型标识</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>序号</summary>
        public string SequenceNumber { get; set; } = string.Empty;
        /// <summary>注释来源</summary>
        public string CommentSource { get; set; } = string.Empty;
        /// <summary>注释文本</summary>
        public string CommentText { get; set; } = string.Empty;
        /// <summary>注释类型</summary>
        public string CommentType { get; set; } = string.Empty;
    }

    /// <summary>
    /// 消息终止记录（L 记录）
    /// </summary>
    public class MessageTerminatorRecord
    {
        /// <summary>记录类型标识</summary>
        public string RecordTypeId { get; set; } = string.Empty;
        /// <summary>序号</summary>
        public string SequenceNumber { get; set; } = string.Empty;
        /// <summary>终止代码</summary>
        public string TerminationCode { get; set; } = string.Empty;
    }
}
