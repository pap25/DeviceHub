namespace DeviceHub.Model.Vo;

/// <summary>
/// 操作日志分页展示项
/// </summary>
public class ClientLogPageItem
{
    public long Id { get; set; }

    public string LevelName { get; set; }

    public string TypeName { get; set; }

    public string Message { get; set; }

    public string CreateTime { get; set; }
}
