using DeviceHub.Base.Common;
using DeviceHub.Repository;

namespace DeviceHub.Win.Base;

/// <summary>
/// 数据库初始化
/// </summary>
internal static class DatabaseInitializer
{
    /// <summary>
    /// 配置连接并创建表结构
    /// </summary>
    public static async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = AppConfig.DatabaseConnectionString;
        DbHelper.Configure(connectionString);

        Logger.Info(nameof(DatabaseInitializer), $"初始化数据库: {connectionString}");
        await DbHelper.InitializeAsync(cancellationToken);
        Logger.Info(nameof(DatabaseInitializer), "数据库表结构初始化完成");
    }
}
