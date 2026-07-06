namespace DeviceHub.Model.Entities;

/// <summary>
/// 发送仪器消息编码记录
/// </summary>
public class SendMessageEncoder
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 发送仪器消息队列表Id
    /// </summary>
    public long SendMessageId { get; set; }

    /// <summary>
    /// 发送报文内容（二进制）
    /// </summary>
    public byte[] SendContent { get; set; } = [];

    /// <summary>
    /// 创建时间（Unix 毫秒时间戳）
    /// </summary>
    public long CreateTime { get; set; }

    /// <summary>
    /// 更新时间（Unix 毫秒时间戳）
    /// </summary>
    public long UpdateTime { get; set; }
}
