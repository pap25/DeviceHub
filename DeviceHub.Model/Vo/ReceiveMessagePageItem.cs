namespace DeviceHub.Model.Vo;

/// <summary>
/// 接收仪器消息分页展示项
/// </summary>
public class ReceiveMessagePageItem
{
    public string StatusName { get; set; }

    public string TypeName { get; set; }

    public string RawMessage { get; set; }

    public string DecodeResult { get; set; }

    public string Barcode { get; set; }

    public string SampleNo { get; set; }

    public string CreateTime { get; set; }

    public string ErrorMessage { get; set; }
}
