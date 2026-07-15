using DeviceHub.Model.Entities;

namespace DeviceHub.Model.view;

public class SendMessageView
{
    public SendMessage.StatusEnum Status { get; set; }

    public string SendJson { get; set; }

    public byte[] SendContent { get; set; }

    public string Barcode { get; set; }

    public string SampleNo { get; set; }

    public long CreateTime { get; set; }

    public string ErrorMessage { get; set; }
}
