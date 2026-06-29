using DeviceHub.Model;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository.Repositories;

/// <summary>
/// 接收仪器消息解码结果表数据访问
/// </summary>
public class ReceiveMessageDecodeRepository : IReceiveMessageDecodeRepository
{
    private static readonly ReceiveMessageDecodeRepository _instance = new();
    public static ReceiveMessageDecodeRepository Instance => _instance;
    private ReceiveMessageDecodeRepository()
    {
    }

    public async Task<long> InsertAsync(ReceiveMessageDecode entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO receive_message_decode (receive_message_id, type, sample_no, barcode, result_json, create_time, update_time)
            VALUES (@receive_message_id, @type, @sample_no, @barcode, @result_json, @create_time, @update_time)
            RETURNING id;
            """;

        var id = await DbHelper.ExecuteScalarAsync<long>(
            sql,
            [
                DbHelper.Param("@receive_message_id", entity.ReceiveMessageId),
                DbHelper.Param("@type", (byte)entity.Type),
                DbHelper.Param("@sample_no", entity.SampleNo),
                DbHelper.Param("@barcode", entity.Barcode),
                DbHelper.Param("@result_json", entity.ResultJson),
                DbHelper.Param("@create_time", entity.CreateTime),
                DbHelper.Param("@update_time", entity.UpdateTime)
            ],
            cancellationToken);

        return id;
    }

    public async Task<bool> UpdateAsync(ReceiveMessageDecode entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE receive_message_decode
            SET receive_message_id = @receive_message_id,
                type = @type,
                sample_no = @sample_no,
                barcode = @barcode,
                result_json = @result_json,
                create_time = @create_time,
                update_time = @update_time
            WHERE id = @id;
            """;

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [
                DbHelper.Param("@id", entity.Id),
                DbHelper.Param("@receive_message_id", entity.ReceiveMessageId),
                DbHelper.Param("@type", (byte)entity.Type),
                DbHelper.Param("@sample_no", entity.SampleNo),
                DbHelper.Param("@barcode", entity.Barcode),
                DbHelper.Param("@result_json", entity.ResultJson),
                DbHelper.Param("@create_time", entity.CreateTime),
                DbHelper.Param("@update_time", entity.UpdateTime)
            ],
            cancellationToken);

        return rows > 0;
    }

    public async Task<bool> DeleteByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM receive_message_decode WHERE id = @id;";

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [DbHelper.Param("@id", id)],
            cancellationToken);

        return rows > 0;
    }

    public Task<ReceiveMessageDecode?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            SelectColumns + " WHERE id = @id;",
            Map,
            [DbHelper.Param("@id", id)],
            cancellationToken);

    public Task<ReceiveMessageDecode?> GetByReceiveMessageIdAsync(long receiveMessageId, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            SelectColumns + " WHERE receive_message_id = @receive_message_id;",
            Map,
            [DbHelper.Param("@receive_message_id", receiveMessageId)],
            cancellationToken);

    public async Task<IReadOnlyList<ReceiveMessageDecode>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            SelectColumns + " ORDER BY id;",
            Map,
            cancellationToken: cancellationToken);

    private const string SelectColumns =
        "SELECT id, receive_message_id, type, sample_no, barcode, result_json, create_time, update_time FROM receive_message_decode";

    private static ReceiveMessageDecode Map(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt64(0),
        ReceiveMessageId = reader.GetInt64(1),
        Type = (ReceiveMessageDecode.TypeEnum)reader.GetByte(2),
        SampleNo = reader.GetString(3),
        Barcode = reader.GetString(4),
        ResultJson = reader.GetString(5),
        CreateTime = reader.GetInt64(6),
        UpdateTime = reader.GetInt64(7)
    };
}
