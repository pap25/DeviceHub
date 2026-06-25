using DeviceHub.Model.Enums;

namespace DeviceHub.Model.Entities;

/// <summary>
/// 仪器通信日志表
/// </summary>
public class ClientLog
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 日志类型
    /// </summary>
    public byte Type { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public ClientLogLevel Level { get; set; }

    /// <summary>
    /// 日志内容
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（Unix 毫秒时间戳）
    /// </summary>
    public long CreateTime { get; set; }
}
