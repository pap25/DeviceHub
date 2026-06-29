using DeviceHub.Model.Entities;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository.Repositories;

/// <summary>
/// 发送仪器消息编码记录数据访问
/// </summary>
public class SendMessageEncoderRepository : ISendMessageEncoderRepository
{
    private static readonly SendMessageEncoderRepository _instance = new();
    public static SendMessageEncoderRepository Instance => _instance;
    private SendMessageEncoderRepository()
    {
    }

    public async Task<long> Insert(SendMessageEncoder entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO send_message_encoder (send_message_id, send_content, create_time, update_time)
            VALUES (@send_message_id, @send_content, @create_time, @update_time)
            RETURNING id;
            """;

        var id = await DbHelper.ExecuteScalarAsync<long>(
            sql,
            [
                DbHelper.Param("@send_message_id", entity.SendMessageId),
                DbHelper.Param("@send_content", entity.SendContent),
                DbHelper.Param("@create_time", entity.CreateTime),
                DbHelper.Param("@update_time", entity.UpdateTime)
            ],
            cancellationToken);

        return id;
    }

    public async Task<bool> Update(SendMessageEncoder entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE send_message_encoder
            SET send_message_id = @send_message_id,
                send_content = @send_content,
                create_time = @create_time,
                update_time = @update_time
            WHERE id = @id;
            """;

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [
                DbHelper.Param("@id", entity.Id),
                DbHelper.Param("@send_message_id", entity.SendMessageId),
                DbHelper.Param("@send_content", entity.SendContent),
                DbHelper.Param("@create_time", entity.CreateTime),
                DbHelper.Param("@update_time", entity.UpdateTime)
            ],
            cancellationToken);

        return rows > 0;
    }

    public async Task<bool> DeleteById(long id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM send_message_encoder WHERE id = @id;";

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [DbHelper.Param("@id", id)],
            cancellationToken);

        return rows > 0;
    }

    public Task<SendMessageEncoder?> GetById(long id, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            SelectColumns + " WHERE id = @id;",
            Map,
            [DbHelper.Param("@id", id)],
            cancellationToken);

    public Task<SendMessageEncoder?> GetBySendMessageId(long sendMessageId, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            SelectColumns + " WHERE send_message_id = @send_message_id;",
            Map,
            [DbHelper.Param("@send_message_id", sendMessageId)],
            cancellationToken);

    public async Task<IReadOnlyList<SendMessageEncoder>> GetAll(CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            SelectColumns + " ORDER BY id;",
            Map,
            cancellationToken: cancellationToken);

    private const string SelectColumns =
        "SELECT id, send_message_id, send_content, create_time, update_time FROM send_message_encoder";

    private static SendMessageEncoder Map(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt64(0),
        SendMessageId = reader.GetInt64(1),
        SendContent = reader.GetString(2),
        CreateTime = reader.GetInt64(3),
        UpdateTime = reader.GetInt64(4)
    };
}
