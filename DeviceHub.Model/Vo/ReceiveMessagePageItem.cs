namespace DeviceHub.Model.Vo;

/// <summary>
/// 接收仪器消息分页展示项
/// </summary>
public class ReceiveMessagePageItem
{
    public string Status { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string RawMessage { get; set; } = string.Empty;

    public string DecodeResult { get; set; } = string.Empty;

    public string Barcode { get; set; } = string.Empty;

    public string SampleNo { get; set; } = string.Empty;

    public string CreateTime { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;
}
