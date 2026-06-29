using DeviceHub.Model;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository.Repositories;

/// <summary>
/// 发送仪器消息队列表数据访问
/// </summary>
public class SendMessageRepository : ISendMessageRepository
{
    private static readonly SendMessageRepository _instance = new();
    public static SendMessageRepository Instance => _instance;
    private SendMessageRepository()
    {
    }

    public async Task<long> InsertAsync(SendMessage entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO send_message (instrument_id, type, sample_no, barcode, status, create_time, update_time)
            VALUES (@instrument_id, @type, @sample_no, @barcode, @status, @create_time, @update_time)
            RETURNING id;
            """;

        var id = await DbHelper.ExecuteScalarAsync<long>(
            sql,
            [
                DbHelper.Param("@instrument_id", entity.InstrumentId),
                DbHelper.Param("@type", (byte)entity.Type),
                DbHelper.Param("@sample_no", entity.SampleNo),
                DbHelper.Param("@barcode", entity.Barcode),
                DbHelper.Param("@status", (byte)entity.Status),
                DbHelper.Param("@create_time", entity.CreateTime),
                DbHelper.Param("@update_time", entity.UpdateTime)
            ],
            cancellationToken);

        return id;
    }

    public async Task<bool> UpdateAsync(SendMessage entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE send_message
            SET instrument_id = @instrument_id,
                type = @type,
                sample_no = @sample_no,
                barcode = @barcode,
                status = @status,
                create_time = @create_time,
                update_time = @update_time
            WHERE id = @id;
            """;

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [
                DbHelper.Param("@id", entity.Id),
                DbHelper.Param("@instrument_id", entity.InstrumentId),
                DbHelper.Param("@type", (byte)entity.Type),
                DbHelper.Param("@sample_no", entity.SampleNo),
                DbHelper.Param("@barcode", entity.Barcode),
                DbHelper.Param("@status", (byte)entity.Status),
                DbHelper.Param("@create_time", entity.CreateTime),
                DbHelper.Param("@update_time", entity.UpdateTime)
            ],
            cancellationToken);

        return rows > 0;
    }

    public async Task<bool> DeleteByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM send_message WHERE id = @id;";

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [DbHelper.Param("@id", id)],
            cancellationToken);

        return rows > 0;
    }

    public Task<SendMessage?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            SelectColumns + " WHERE id = @id;",
            Map,
            [DbHelper.Param("@id", id)],
            cancellationToken);

    public async Task<IReadOnlyList<SendMessage>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            SelectColumns + " ORDER BY id;",
            Map,
            cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<SendMessage>> GetByStatusAsync(SendMessage.StatusEnum status, CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            SelectColumns + " WHERE status = @status ORDER BY id;",
            Map,
            [DbHelper.Param("@status", (byte)status)],
            cancellationToken);

    private const string SelectColumns =
        "SELECT id, instrument_id, type, sample_no, barcode, status, create_time, update_time FROM send_message";

    private static SendMessage Map(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt64(0),
        InstrumentId = reader.GetInt64(1),
        Type = (SendMessage.TypeEnum)reader.GetByte(2),
        SampleNo = reader.GetString(3),
        Barcode = reader.GetString(4),
        Status = (SendMessage.StatusEnum)reader.GetByte(5),
        CreateTime = reader.GetInt64(6),
        UpdateTime = reader.GetInt64(7)
    };
}
