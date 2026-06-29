namespace DeviceHub.Model.Entities;

/// <summary>
/// 发送仪器消息队列表扩展表
/// </summary>
public class SendMessageLarge
{
    /// <summary>
    /// 发送仪器消息队列表Id
    /// </summary>
    public long SendMessageId { get; set; }

    /// <summary>
    /// 发送内容JSON
    /// </summary>
    public string SendJson { get; set; }
}
