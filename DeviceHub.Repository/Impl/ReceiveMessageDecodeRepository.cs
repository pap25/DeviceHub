using DeviceHub.Model.Entities;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository.Repositories;

/// <summary>
/// 接收仪器消息解码结果表数据访问
/// </summary>
public class ReceiveMessageDecodeRepository : IReceiveMessageDecodeRepository
{
    private static readonly IReceiveMessageDecodeRepository _instance = new ReceiveMessageDecodeRepository();
    public static IReceiveMessageDecodeRepository Instance => _instance;
    private ReceiveMessageDecodeRepository()
    {
    }

    public Task<long> Insert(ReceiveMessageDecode entity, CancellationToken cancellationToken = default) =>
        Insert(entity, null, null, cancellationToken);

    public async Task<long> Insert(
        ReceiveMessageDecode entity,
        SqliteConnection? connection,
        SqliteTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO receive_message_decode (receive_message_id, external_no, type, sample_no, barcode, result_json, create_time, update_time)
            VALUES (@receive_message_id, @external_no, @type, @sample_no, @barcode, @result_json, @create_time, @update_time)
            RETURNING id;
            """;

        var parameters = new SqliteParameter[]
        {
            DbHelper.Param("@receive_message_id", entity.ReceiveMessageId),
            DbHelper.Param("@external_no", entity.ExternalNo),
            DbHelper.Param("@type", (byte)entity.Type),
            DbHelper.Param("@sample_no", entity.SampleNo),
            DbHelper.Param("@barcode", entity.Barcode),
            DbHelper.Param("@result_json", entity.ResultJson),
            DbHelper.Param("@create_time", entity.CreateTime),
            DbHelper.Param("@update_time", entity.UpdateTime)
        };

        if (connection is not null && transaction is not null)
        {
            return await DbHelper.ExecuteScalarAsync<long>(
                connection, transaction, sql, parameters, cancellationToken);
        }

        return await DbHelper.ExecuteScalarAsync<long>(sql, parameters, cancellationToken);
    }

    public Task InsertForUpdateByReceiveMessageId(ReceiveMessageDecode entity, CancellationToken cancellationToken = default) =>
        InsertForUpdateByReceiveMessageId(entity, null, null, cancellationToken);

    public async Task InsertForUpdateByReceiveMessageId(
        ReceiveMessageDecode entity,
        SqliteConnection? connection,
        SqliteTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO receive_message_decode (receive_message_id, external_no, type, sample_no, barcode, result_json, create_time, update_time)
            VALUES (@receive_message_id, @external_no, @type, @sample_no, @barcode, @result_json, @create_time, @update_time)
            ON CONFLICT(receive_message_id) DO UPDATE SET
                external_no = excluded.external_no,
                type = excluded.type,
                sample_no = excluded.sample_no,
                barcode = excluded.barcode,
                result_json = excluded.result_json,
                update_time = excluded.update_time;
            """;

        var parameters = new SqliteParameter[]
        {
            DbHelper.Param("@receive_message_id", entity.ReceiveMessageId),
            DbHelper.Param("@external_no", entity.ExternalNo),
            DbHelper.Param("@type", (byte)entity.Type),
            DbHelper.Param("@sample_no", entity.SampleNo),
            DbHelper.Param("@barcode", entity.Barcode),
            DbHelper.Param("@result_json", entity.ResultJson),
            DbHelper.Param("@create_time", entity.CreateTime),
            DbHelper.Param("@update_time", entity.UpdateTime)
        };

        if (connection is not null && transaction is not null)
        {
            await DbHelper.ExecuteNonQueryAsync(connection, transaction, sql, parameters, cancellationToken);
            return;
        }

        await DbHelper.ExecuteNonQueryAsync(sql, parameters, cancellationToken);
    }

    public async Task<bool> Update(ReceiveMessageDecode entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE receive_message_decode
            SET receive_message_id = @receive_message_id,
                external_no = @external_no,
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
                DbHelper.Param("@external_no", entity.ExternalNo),
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

    public async Task<bool> DeleteById(long id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM receive_message_decode WHERE id = @id;";

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [DbHelper.Param("@id", id)],
            cancellationToken);

        return rows > 0;
    }

    public Task<ReceiveMessageDecode?> GetById(long id, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            SelectColumns + " WHERE id = @id;",
            Map,
            [DbHelper.Param("@id", id)],
            cancellationToken);

    public Task<ReceiveMessageDecode?> GetByReceiveMessageId(long receiveMessageId, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            SelectColumns + " WHERE receive_message_id = @receive_message_id;",
            Map,
            [DbHelper.Param("@receive_message_id", receiveMessageId)],
            cancellationToken);

    public async Task<IReadOnlyList<ReceiveMessageDecode>> GetAll(CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            SelectColumns + " ORDER BY id;",
            Map,
            cancellationToken: cancellationToken);

    private const string SelectColumns =
        "SELECT id, receive_message_id, external_no, type, sample_no, barcode, result_json, create_time, update_time FROM receive_message_decode";

    private static ReceiveMessageDecode Map(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt64(0),
        ReceiveMessageId = reader.GetInt64(1),
        ExternalNo = reader.GetString(2),
        Type = (ReceiveMessageDecode.TypeEnum)reader.GetByte(3),
        SampleNo = reader.GetString(4),
        Barcode = reader.GetString(5),
        ResultJson = reader.GetString(6),
        CreateTime = reader.GetInt64(7),
        UpdateTime = reader.GetInt64(8)
    };
}
