namespace DeviceHub.Model.Enums;

/// <summary>
/// 客户端日志级别
/// </summary>
public enum ClientLogLevel : byte
{
    /// <summary>
    /// 普通
    /// </summary>
    Info = 0,

    /// <summary>
    /// 警告
    /// </summary>
    Warning = 1,

    /// <summary>
    /// 异常
    /// </summary>
    Error = 2
}
