using DeviceHub.Model.Enums;

namespace DeviceHub.Model.Entities;

/// <summary>
/// 接收仪器消息队列表
/// </summary>
public class ReceiveMessage
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 仪器ID
    /// </summary>
    public long InstrumentId { get; set; }

    /// <summary>
    /// 处理状态
    /// </summary>
    public MessageStatus Status { get; set; }

    /// <summary>
    /// 处理失败原因
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（Unix 毫秒时间戳）
    /// </summary>
    public long CreateTime { get; set; }
}
