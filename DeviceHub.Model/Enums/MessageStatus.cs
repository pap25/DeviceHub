using System.ComponentModel;

namespace DeviceHub.Model.Enums;

/// <summary>
/// 消息处理状态
/// </summary>
public enum MessageStatus : byte
{
    /// <summary>
    /// 待处理
    /// </summary>
    [Description("待处理")]
    Pending = 0,

    /// <summary>
    /// 处理成功
    /// </summary>
    [Description("处理成功")]
    Success = 1,

    /// <summary>
    /// 处理失败
    /// </summary>
    [Description("处理失败")]
    Failed = 2
}
