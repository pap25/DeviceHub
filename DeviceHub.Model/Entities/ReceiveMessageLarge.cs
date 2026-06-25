namespace DeviceHub.Model.Entities;

/// <summary>
/// 接收仪器消息队列表扩展表
/// </summary>
public class ReceiveMessageLarge
{
    /// <summary>
    /// 接收仪器消息队列表Id
    /// </summary>
    public long ReceiveMessageId { get; set; }

    /// <summary>
    /// 原始报文
    /// </summary>
    public string RawMessage { get; set; } = string.Empty;
}
