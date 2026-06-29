using System.ComponentModel;

namespace DeviceHub.Model.Entities;

/// <summary>
/// 仪器通信日志表
/// </summary>
public class ClientLog
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 日志类型
    /// </summary>
    public TypeEnum Type { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public LevelEnum Level { get; set; }

    /// <summary>
    /// 日志内容（最长 5000 字符）
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 创建时间（Unix 毫秒时间戳）
    /// </summary>
    public long CreateTime { get; set; }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum TypeEnum : byte
    {
        /// <summary>
        /// LIS接口
        /// </summary>
        [Description("LIS接口")]
        LisInterface = 0,

        /// <summary>
        /// 仪器通信
        /// </summary>
        [Description("仪器通信")]
        InstrumentCommunication = 1,

        /// <summary>
        /// 本地数据库
        /// </summary>
        [Description("本地数据库")]
        LocalDatabase = 2
    }

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LevelEnum : byte
    {
        /// <summary>
        /// 普通
        /// </summary>
        [Description("普通")]
        Info = 0,

        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        Warning = 1,

        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
        Error = 2
    }
}
