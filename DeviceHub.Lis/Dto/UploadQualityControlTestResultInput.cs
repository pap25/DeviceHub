namespace DeviceHub.Lis.Dto;

public class UploadQualityControlTestResultInput
{
    public long InstrumentId { get; set; }
    /// <summary>项目编号（OBR-2）</summary>
    public string ItemCode { get; set; } = string.Empty;
    /// <summary>项目名称（OBR-3）</summary>
    public string ItemName { get; set; } = string.Empty;
    /// <summary>质控时间（OBR-7）</summary>
    public string TestTime { get; set; } = string.Empty;
    /// <summary>质控规则（OBR-9：0-Westguard，1-累积和，2-TWIN-PLOT）</summary>
    public string QcRule { get; set; } = string.Empty;
    /// <summary>测试模块（OBR-16，如 M1）</summary>
    public string ModuleId { get; set; } = string.Empty;
    /// <summary>结果追溯信息（OBR-8）</summary>
    public string TraceabilityInfo { get; set; } = string.Empty;
    /// <summary>各质控液测定结果（按质控液个数展开）</summary>
    public List<QualityControlResultItem> Items { get; set; } = [];
}
