namespace DeviceHub.Lis.Dto;

/// <summary>
/// 样本申请测试项目（对应 DSP Data Line 第 29 字段及后续扩展字段）。
/// 格式：项目通道号^项目名称^单位^参考范围^稀释倍数^重测标志^指定测试模块^指定试剂批号^指定试剂瓶
/// </summary>
public class SampleApplyTestItem
{
    /// <summary>项目通道号（必填）</summary>
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>项目名称</summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>单位</summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>参考范围</summary>
    public string ReferenceRange { get; set; } = string.Empty;

    /// <summary>稀释倍数（如 10 表示 1:10）</summary>
    public string DilutionFactor { get; set; } = string.Empty;

    /// <summary>重测标志：0-非重测，1-重测</summary>
    public string RetestFlag { get; set; } = string.Empty;

    /// <summary>指定测试模块（如 M1）</summary>
    public string TestModule { get; set; } = string.Empty;

    /// <summary>指定试剂批号</summary>
    public string ReagentLotNo { get; set; } = string.Empty;

    /// <summary>指定试剂瓶号</summary>
    public string ReagentBottleNo { get; set; } = string.Empty;
}
