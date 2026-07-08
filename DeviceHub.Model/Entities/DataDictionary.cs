namespace DeviceHub.Model.Entities;

/// <summary>
/// 数据字典
/// </summary>
public class DataDictionary
{
    /// <summary>
    /// 配置 key
    /// </summary>
    public string Ckey { get; set; } = string.Empty;

    /// <summary>
    /// 配置值
    /// </summary>
    public string Value { get; set; } = string.Empty;

    public static class Keys
    {
        public const string LisIssueApplicationLastId = "LisIssueApplicationLastId";
    }
}
