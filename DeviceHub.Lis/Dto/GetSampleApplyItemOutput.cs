namespace DeviceHub.Lis.Dto;

/// <summary>
/// 仪器查询样本申请信息的 LIS 返回（编码为 DSR^Q03，MSH-15 非 P）。
/// DSP Data Line 字段顺序见《YHLO LIS接口手册》DSP 段说明。
/// </summary>
public class GetSampleApplyItemOutput
{
    /// <summary>MSH-10 / MSA-2，对应原查询消息控制 ID</summary>
    public string MessageControlId { get; set; } = string.Empty;

    /// <summary>设备型号（QRF-1）</summary>
    public string DeviceModel { get; set; } = string.Empty;

    /// <summary>字符集（MSH-18），默认 ASCII</summary>
    public string CharacterSet { get; set; } = "ASCII";

    /// <summary>QAK-2：OK / NF / AE / AR</summary>
    public string QueryResponseStatus { get; set; } = "OK";

    /// <summary>DSC-1 连续指针；空表示本批最后一条</summary>
    public string ContinuationPointer { get; set; } = string.Empty;

    /// <summary>DSP-1 住院号</summary>
    public string HospitalNo { get; set; } = string.Empty;

    /// <summary>DSP-2 床号</summary>
    public string BedNo { get; set; } = string.Empty;

    /// <summary>DSP-3 病人姓名</summary>
    public string PatientName { get; set; } = string.Empty;

    /// <summary>DSP-4 出生日期（yyyyMMddHHmmss）</summary>
    public string BirthDate { get; set; } = string.Empty;

    /// <summary>DSP-5 性别（M/F/O）</summary>
    public string Sex { get; set; } = string.Empty;

    /// <summary>DSP-6 血型</summary>
    public string BloodType { get; set; } = string.Empty;

    /// <summary>DSP-7 种族</summary>
    public string Race { get; set; } = string.Empty;

    /// <summary>DSP-8 地址</summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>DSP-9 邮编</summary>
    public string ZipCode { get; set; } = string.Empty;

    /// <summary>DSP-10 家庭电话</summary>
    public string HomePhone { get; set; } = string.Empty;

    /// <summary>DSP-11 样本位（架号^位号）</summary>
    public string SamplePosition { get; set; } = string.Empty;

    /// <summary>DSP-12 样本采集时间</summary>
    public string CollectionTime { get; set; } = string.Empty;

    /// <summary>DSP-13 病历号</summary>
    public string MedicalRecordNo { get; set; } = string.Empty;

    /// <summary>DSP-14 操作码（NW/RF/CA）</summary>
    public string ActionCode { get; set; } = string.Empty;

    /// <summary>DSP-15 病人类别</summary>
    public string PatientType { get; set; } = string.Empty;

    /// <summary>DSP-16 医保帐号</summary>
    public string InsuranceAccount { get; set; } = string.Empty;

    /// <summary>DSP-17 收费类型</summary>
    public string FeeType { get; set; } = string.Empty;

    /// <summary>DSP-18 民族</summary>
    public string Nation { get; set; } = string.Empty;

    /// <summary>DSP-19 籍贯</summary>
    public string NativePlace { get; set; } = string.Empty;

    /// <summary>DSP-20 国家</summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>DSP-21 样本条码（必填）</summary>
    public string Barcode { get; set; } = string.Empty;

    /// <summary>DSP-22 样本编号（必填）</summary>
    public string SampleNo { get; set; } = string.Empty;

    /// <summary>DSP-23 送检时间</summary>
    public string SubmitTime { get; set; } = string.Empty;

    /// <summary>DSP-24 是否急诊（Y/N，空默认 N）</summary>
    public string IsEmergency { get; set; } = string.Empty;

    /// <summary>DSP-26 样本类型（必填）</summary>
    public string SampleType { get; set; } = string.Empty;

    /// <summary>DSP-27 送检医生</summary>
    public string RequestDoctor { get; set; } = string.Empty;

    /// <summary>DSP-28 送检科室</summary>
    public string RequestDept { get; set; } = string.Empty;

    /// <summary>DSP-29 起测试项目列表</summary>
    public List<SampleApplyTestItem> Items { get; set; } = [];
}
