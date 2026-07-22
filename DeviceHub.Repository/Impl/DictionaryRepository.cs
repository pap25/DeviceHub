using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository.Repositories;

/// <summary>
/// 数据字典数据访问
/// </summary>
public class DictionaryRepository : IDictionaryRepository
{
    private static readonly IDictionaryRepository _instance = new DictionaryRepository();
    public static IDictionaryRepository Instance => _instance;

    private DictionaryRepository()
    {
    }

    public Task<string?> GetValueByCkey(string ckey, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            "SELECT value FROM dictionary WHERE ckey = @ckey;",
            reader => reader.GetString(0),
            [DbHelper.Param("@ckey", ckey)],
            cancellationToken);

    public Task Insert(string ckey, string value, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO dictionary (ckey, value)
            VALUES (@ckey, @value);
            """;

        return DbHelper.ExecuteNonQueryAsync(
            sql,
            [
                DbHelper.Param("@ckey", ckey),
                DbHelper.Param("@value", value)
            ],
            cancellationToken);
    }

    public Task UpdateValueByCkey(string ckey, string value, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dictionary
            SET value = @value
            WHERE ckey = @ckey;
            """;

        return DbHelper.ExecuteNonQueryAsync(
            sql,
            [
                DbHelper.Param("@ckey", ckey),
                DbHelper.Param("@value", value)
            ],
            cancellationToken);
    }

    public Task UpdateValueByCkey(
        string ckey,
        string value,
        SqliteConnection connection,
        SqliteTransaction transaction,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dictionary
            SET value = @value
            WHERE ckey = @ckey;
            """;

        return DbHelper.ExecuteNonQueryAsync(
            connection,
            transaction,
            sql,
            [
                DbHelper.Param("@ckey", ckey),
                DbHelper.Param("@value", value)
            ],
            cancellationToken);
    }
}
