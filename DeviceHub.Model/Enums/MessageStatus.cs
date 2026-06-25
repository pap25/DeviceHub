namespace DeviceHub.Model.Enums;

/// <summary>
/// 消息处理状态
/// </summary>
public enum MessageStatus : byte
{
    /// <summary>
    /// 待处理
    /// </summary>
    Pending = 0,

    /// <summary>
    /// 处理成功
    /// </summary>
    Success = 1,

    /// <summary>
    /// 处理失败
    /// </summary>
    Failed = 2
}
