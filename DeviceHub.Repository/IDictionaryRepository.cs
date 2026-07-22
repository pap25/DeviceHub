using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository;

/// <summary>
/// 数据字典数据访问接口
/// </summary>
public interface IDictionaryRepository
{
    Task<string?> GetValueByCkey(string ckey, CancellationToken cancellationToken = default);

    Task Insert(
        string ckey,
        string value,
        CancellationToken cancellationToken = default);

    Task UpdateValueByCkey(
        string ckey,
        string value,
        CancellationToken cancellationToken = default);

    Task UpdateValueByCkey(
        string ckey,
        string value,
        SqliteConnection connection,
        SqliteTransaction transaction,
        CancellationToken cancellationToken = default);
}
