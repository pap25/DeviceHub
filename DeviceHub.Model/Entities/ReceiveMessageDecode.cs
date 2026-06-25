using DeviceHub.Model.Enums;

namespace DeviceHub.Model.Entities;

/// <summary>
/// 接收仪器消息解码结果表
/// </summary>
public class ReceiveMessageDecode
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 接收仪器消息队列表Id
    /// </summary>
    public long ReceiveMessageId { get; set; }

    /// <summary>
    /// 解码类型
    /// </summary>
    public ReceiveMessageDecodeType Type { get; set; }

    /// <summary>
    /// 条形码
    /// </summary>
    public string Barcode { get; set; } = string.Empty;

    /// <summary>
    /// 解码结果JSON
    /// </summary>
    public string ResultJson { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（Unix 毫秒时间戳）
    /// </summary>
    public long CreateTime { get; set; }

    /// <summary>
    /// 更新时间（Unix 毫秒时间戳）
    /// </summary>
    public long UpdateTime { get; set; }
}
