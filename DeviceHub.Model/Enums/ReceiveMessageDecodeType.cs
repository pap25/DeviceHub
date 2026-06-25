namespace DeviceHub.Model.Enums;

/// <summary>
/// 接收消息解码类型
/// </summary>
public enum ReceiveMessageDecodeType : byte
{
    /// <summary>
    /// 检验结果
    /// </summary>
    TestResult = 0,

    /// <summary>
    /// 查询样本申请信息
    /// </summary>
    SampleQuery = 1
}
