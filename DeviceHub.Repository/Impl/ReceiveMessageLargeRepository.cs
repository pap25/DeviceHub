using DeviceHub.Model.Entities;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository.Repositories;

/// <summary>
/// 接收仪器消息队列表扩展表数据访问
/// </summary>
public class ReceiveMessageLargeRepository : IReceiveMessageLargeRepository
{
    private static readonly ReceiveMessageLargeRepository _instance = new();
    public static ReceiveMessageLargeRepository Instance => _instance;
    private ReceiveMessageLargeRepository()
    {
    }

    public Task<bool> Insert(ReceiveMessageLarge entity, CancellationToken cancellationToken = default) =>
        Insert(entity, null, null, cancellationToken);

    public async Task<bool> Insert(
        ReceiveMessageLarge entity,
        SqliteConnection? connection,
        SqliteTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO receive_message_large (receive_message_id, raw_message)
            VALUES (@receive_message_id, @raw_message);
            """;

        var parameters = new SqliteParameter[]
        {
            DbHelper.Param("@receive_message_id", entity.ReceiveMessageId),
            DbHelper.Param("@raw_message", entity.RawMessage)
        };

        int rows = connection is not null && transaction is not null
            ? await DbHelper.ExecuteNonQueryAsync(connection, transaction, sql, parameters, cancellationToken)
            : await DbHelper.ExecuteNonQueryAsync(sql, parameters, cancellationToken);

        return rows > 0;
    }

    public async Task<bool> Update(ReceiveMessageLarge entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE receive_message_large
            SET raw_message = @raw_message
            WHERE receive_message_id = @receive_message_id;
            """;

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [
                DbHelper.Param("@receive_message_id", entity.ReceiveMessageId),
                DbHelper.Param("@raw_message", entity.RawMessage)
            ],
            cancellationToken);

        return rows > 0;
    }

    public async Task<bool> DeleteByReceiveMessageId(long receiveMessageId, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM receive_message_large WHERE receive_message_id = @receive_message_id;";

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [DbHelper.Param("@receive_message_id", receiveMessageId)],
            cancellationToken);

        return rows > 0;
    }

    public Task<ReceiveMessageLarge?> GetByReceiveMessageId(long receiveMessageId, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            "SELECT receive_message_id, raw_message FROM receive_message_large WHERE receive_message_id = @receive_message_id;",
            Map,
            [DbHelper.Param("@receive_message_id", receiveMessageId)],
            cancellationToken);

    public async Task<IReadOnlyList<ReceiveMessageLarge>> GetAll(CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            "SELECT receive_message_id, raw_message FROM receive_message_large ORDER BY receive_message_id;",
            Map,
            cancellationToken: cancellationToken);

    private static ReceiveMessageLarge Map(SqliteDataReader reader) => new()
    {
        ReceiveMessageId = reader.GetInt64(0),
        RawMessage = reader.GetString(1)
    };
}
