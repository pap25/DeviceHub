namespace DeviceHub.Lis.Dto;

public class QualityControlResultItem
{
    /// <summary>质控液编号</summary>
    public string QcNo { get; set; } = string.Empty;
    /// <summary>质控液名称</summary>
    public string QcName { get; set; } = string.Empty;
    /// <summary>质控液批号</summary>
    public string LotNo { get; set; } = string.Empty;
    /// <summary>质控液有效期</summary>
    public string ExpiryDate { get; set; } = string.Empty;
    /// <summary>浓度水平（H/M/L）</summary>
    public string Level { get; set; } = string.Empty;
    /// <summary>质控液均值</summary>
    public string Mean { get; set; } = string.Empty;
    /// <summary>质控液标准差</summary>
    public string StdDev { get; set; } = string.Empty;
    /// <summary>测试结果值（浓度）</summary>
    public string ResultValue { get; set; } = string.Empty;
    /// <summary>质控条码号</summary>
    public string QcBarcode { get; set; } = string.Empty;
}
