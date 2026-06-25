using DeviceHub.Model.Entities;
using DeviceHub.Model.Enums;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository.Repositories;

/// <summary>
/// 接收仪器消息队列表数据访问
/// </summary>
public class ReceiveMessageRepository : IReceiveMessageRepository
{
    public async Task<long> InsertAsync(ReceiveMessage entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO receive_message (instrument_id, status, error_message, create_time)
            VALUES (@instrument_id, @status, @error_message, @create_time)
            RETURNING id;
            """;

        var id = await DbHelper.ExecuteScalarAsync<long>(
            sql,
            [
                DbHelper.Param("@instrument_id", entity.InstrumentId),
                DbHelper.Param("@status", (byte)entity.Status),
                DbHelper.Param("@error_message", entity.ErrorMessage),
                DbHelper.Param("@create_time", entity.CreateTime)
            ],
            cancellationToken);

        return id;
    }

    public async Task<bool> UpdateAsync(ReceiveMessage entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE receive_message
            SET instrument_id = @instrument_id,
                status = @status,
                error_message = @error_message,
                create_time = @create_time
            WHERE id = @id;
            """;

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [
                DbHelper.Param("@id", entity.Id),
                DbHelper.Param("@instrument_id", entity.InstrumentId),
                DbHelper.Param("@status", (byte)entity.Status),
                DbHelper.Param("@error_message", entity.ErrorMessage),
                DbHelper.Param("@create_time", entity.CreateTime)
            ],
            cancellationToken);

        return rows > 0;
    }

    public async Task<bool> DeleteByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM receive_message WHERE id = @id;";

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [DbHelper.Param("@id", id)],
            cancellationToken);

        return rows > 0;
    }

    public Task<ReceiveMessage?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            "SELECT id, instrument_id, status, error_message, create_time FROM receive_message WHERE id = @id;",
            Map,
            [DbHelper.Param("@id", id)],
            cancellationToken);

    public async Task<IReadOnlyList<ReceiveMessage>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            "SELECT id, instrument_id, status, error_message, create_time FROM receive_message ORDER BY id;",
            Map,
            cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<ReceiveMessage>> GetByInstrumentIdAsync(long instrumentId, CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            "SELECT id, instrument_id, status, error_message, create_time FROM receive_message WHERE instrument_id = @instrument_id ORDER BY id;",
            Map,
            [DbHelper.Param("@instrument_id", instrumentId)],
            cancellationToken);

    public async Task<IReadOnlyList<ReceiveMessage>> GetByStatusAsync(MessageStatus status, CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            "SELECT id, instrument_id, status, error_message, create_time FROM receive_message WHERE status = @status ORDER BY id;",
            Map,
            [DbHelper.Param("@status", (byte)status)],
            cancellationToken);

    private static ReceiveMessage Map(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt64(0),
        InstrumentId = reader.GetInt64(1),
        Status = (MessageStatus)reader.GetByte(2),
        ErrorMessage = reader.GetString(3),
        CreateTime = reader.GetInt64(4)
    };
}
