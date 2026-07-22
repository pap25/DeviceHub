using DeviceHub.Model.Entities;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository.Repositories;

/// <summary>
/// 发送仪器消息队列表扩展表数据访问
/// </summary>
public class SendMessageLargeRepository : ISendMessageLargeRepository
{
    private static readonly ISendMessageLargeRepository _instance = new SendMessageLargeRepository();
    public static ISendMessageLargeRepository Instance => _instance;
    private SendMessageLargeRepository()
    {
    }

    public Task<bool> Insert(SendMessageLarge entity, CancellationToken cancellationToken = default) =>
        Insert(entity, null, null, cancellationToken);

    public async Task<bool> Insert(
        SendMessageLarge entity,
        SqliteConnection? connection,
        SqliteTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO send_message_large (send_message_id, send_json)
            VALUES (@send_message_id, @send_json);
            """;

        var parameters = new SqliteParameter[]
        {
            DbHelper.Param("@send_message_id", entity.SendMessageId),
            DbHelper.Param("@send_json", entity.SendJson)
        };

        int rows = connection is not null && transaction is not null
            ? await DbHelper.ExecuteNonQueryAsync(connection, transaction, sql, parameters, cancellationToken)
            : await DbHelper.ExecuteNonQueryAsync(sql, parameters, cancellationToken);

        return rows > 0;
    }

    public async Task<bool> Update(SendMessageLarge entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE send_message_large
            SET send_json = @send_json
            WHERE send_message_id = @send_message_id;
            """;

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [
                DbHelper.Param("@send_message_id", entity.SendMessageId),
                DbHelper.Param("@send_json", entity.SendJson)
            ],
            cancellationToken);

        return rows > 0;
    }

    public async Task<bool> DeleteBySendMessageId(long sendMessageId, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM send_message_large WHERE send_message_id = @send_message_id;";

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [DbHelper.Param("@send_message_id", sendMessageId)],
            cancellationToken);

        return rows > 0;
    }

    public Task<SendMessageLarge?> GetBySendMessageId(long sendMessageId, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            "SELECT send_message_id, send_json FROM send_message_large WHERE send_message_id = @send_message_id;",
            Map,
            [DbHelper.Param("@send_message_id", sendMessageId)],
            cancellationToken);

    public async Task<IReadOnlyList<SendMessageLarge>> GetAll(CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            "SELECT send_message_id, send_json FROM send_message_large ORDER BY send_message_id;",
            Map,
            cancellationToken: cancellationToken);

    private static SendMessageLarge Map(SqliteDataReader reader) => new()
    {
        SendMessageId = reader.GetInt64(0),
        SendJson = reader.GetString(1)
    };
}
