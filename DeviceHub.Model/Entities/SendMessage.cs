using DeviceHub.Model.Enums;

namespace DeviceHub.Model.Entities;

/// <summary>
/// 发送仪器消息队列表
/// </summary>
public class SendMessage
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
    /// 消息类型
    /// </summary>
    public SendMessageType Type { get; set; }

    /// <summary>
    /// 条形码
    /// </summary>
    public string Barcode { get; set; } = string.Empty;

    /// <summary>
    /// 处理状态
    /// </summary>
    public MessageStatus Status { get; set; }

    /// <summary>
    /// 创建时间（Unix 毫秒时间戳）
    /// </summary>
    public long CreateTime { get; set; }

    /// <summary>
    /// 更新时间（Unix 毫秒时间戳）
    /// </summary>
    public long UpdateTime { get; set; }
}
