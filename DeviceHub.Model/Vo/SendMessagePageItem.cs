namespace DeviceHub.Model.Vo;

/// <summary>
/// 发送仪器消息分页展示项
/// </summary>
public class SendMessagePageItem
{
    public long Id { get; set; }

    public string ExternalNo { get; set; }

    public string StatusName { get; set; }

    public string SendJson { get; set; }

    public string SendContent { get; set; }

    public string Barcode { get; set; }

    public string SampleNo { get; set; }

    public string CreateTime { get; set; }

    public string ErrorMessage { get; set; }
}
