namespace DeviceHub.Lis.Dto;

/// <summary>
/// LIS 主动下发的样本申请信息（编码为 DSR^Q03，MSH-15=P）。
/// </summary>
public class GetSampleApplyListOutput : GetSampleApplyItemOutput
{
    /// <summary>LIS 侧申请记录 ID（用于拉取游标）</summary>
    public long Id { get; set; }
}
