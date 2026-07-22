using System.ComponentModel;

namespace DeviceHub.Model.Entities;

/// <summary>
/// 发送仪器消息队列表
/// </summary>
public class SendMessage
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 仪器ID
    /// </summary>
    public long InstrumentId { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public TypeEnum Type { get; set; }

    /// <summary>
    /// 外部编号（LIS 侧唯一标识）
    /// </summary>
    public string ExternalNo { get; set; }

    /// <summary>
    /// 样本号
    /// </summary>
    public string SampleNo { get; set; }

    /// <summary>
    /// 条形码
    /// </summary>
    public string Barcode { get; set; }

    /// <summary>
    /// 处理状态
    /// </summary>
    public StatusEnum Status { get; set; }

    /// <summary>
    /// 处理失败原因
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// 创建时间（Unix 毫秒时间戳）
    /// </summary>
    public long CreateTime { get; set; }

    /// <summary>
    /// 更新时间（Unix 毫秒时间戳）
    /// </summary>
    public long UpdateTime { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum TypeEnum : byte
    {
        /// <summary>
        /// 下发申请信息
        /// </summary>
        [Description("LIS下发申请信息")]
        IssueApplication = 0,
        [Description("请求查询申请信息")]
        RequestApplication = 1
    }

    /// <summary>
    /// 处理状态
    /// </summary>
    public enum StatusEnum : byte
    {
        /// <summary>
        /// 待处理
        /// </summary>
        [Description("待处理")]
        Pending = 0,

        /// <summary>
        /// 处理成功
        /// </summary>
        [Description("处理成功")]
        Success = 1,

        /// <summary>
        /// 处理失败
        /// </summary>
        [Description("处理失败")]
        Failed = 2
    }

    public const string RequestApplicationPrefix = "QA_";
}
