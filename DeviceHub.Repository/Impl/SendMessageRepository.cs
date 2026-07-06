using DeviceHub.Model.Entities;
using DeviceHub.Model.view;
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

    public async Task<long> Insert(SendMessage entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO send_message (instrument_id, type, external_no, sample_no, barcode, status, error_message, create_time, update_time)
            VALUES (@instrument_id, @type, @external_no, @sample_no, @barcode, @status, @error_message, @create_time, @update_time)
            RETURNING id;
            """;

        var id = await DbHelper.ExecuteScalarAsync<long>(
            sql,
            [
                DbHelper.Param("@instrument_id", entity.InstrumentId),
                DbHelper.Param("@type", (byte)entity.Type),
                DbHelper.Param("@external_no", entity.ExternalNo),
                DbHelper.Param("@sample_no", entity.SampleNo),
                DbHelper.Param("@barcode", entity.Barcode),
                DbHelper.Param("@status", (byte)entity.Status),
                DbHelper.Param("@error_message", entity.ErrorMessage),
                DbHelper.Param("@create_time", entity.CreateTime),
                DbHelper.Param("@update_time", entity.UpdateTime)
            ],
            cancellationToken);

        return id;
    }

    public async Task<bool> Update(SendMessage entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE send_message
            SET instrument_id = @instrument_id,
                type = @type,
                external_no = @external_no,
                sample_no = @sample_no,
                barcode = @barcode,
                status = @status,
                error_message = @error_message,
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
                DbHelper.Param("@external_no", entity.ExternalNo),
                DbHelper.Param("@sample_no", entity.SampleNo),
                DbHelper.Param("@barcode", entity.Barcode),
                DbHelper.Param("@status", (byte)entity.Status),
                DbHelper.Param("@error_message", entity.ErrorMessage),
                DbHelper.Param("@create_time", entity.CreateTime),
                DbHelper.Param("@update_time", entity.UpdateTime)
            ],
            cancellationToken);

        return rows > 0;
    }

    public async Task<bool> DeleteById(long id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM send_message WHERE id = @id;";

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [DbHelper.Param("@id", id)],
            cancellationToken);

        return rows > 0;
    }

    public Task<SendMessage?> GetById(long id, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            SelectColumns + " WHERE id = @id;",
            Map,
            [DbHelper.Param("@id", id)],
            cancellationToken);

    public async Task<IReadOnlyList<SendMessage>> GetAll(CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            SelectColumns + " ORDER BY id;",
            Map,
            cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<SendMessage>> GetByStatus(SendMessage.StatusEnum status, CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            SelectColumns + " WHERE status = @status ORDER BY id;",
            Map,
            [DbHelper.Param("@status", (byte)status)],
            cancellationToken);

    public async Task<int> findCount(long instrumentId, SendMessage.StatusEnum? status,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, CancellationToken cancellationToken = default)
    {
        var (whereClause, parameters) = BuildWhereConditions(instrumentId, status, barcode, sampleNo, createTimeStart, createTimeEnd);
        var sql = $"""
            SELECT COUNT(*)
            FROM send_message a
            {whereClause};
            """;

        var count = await DbHelper.ExecuteScalarAsync<long>(sql, parameters, cancellationToken);
        return (int)count;
    }

    public async Task<List<SendMessageView>> findPageDesc(long instrumentId, SendMessage.StatusEnum? status,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex, CancellationToken cancellationToken = default)
    {
        var (whereClause, parameters) = BuildWhereConditions(instrumentId, status, barcode, sampleNo, createTimeStart, createTimeEnd);
        parameters.Add(DbHelper.Param("@page_size", pageSize));
        parameters.Add(DbHelper.Param("@offset", Math.Max(0, (pageIndex - 1) * pageSize)));

        var sql = $"""
            SELECT a.status, b.send_json, c.send_content, a.barcode, a.sample_no, a.create_time, a.error_message
            FROM send_message a
            INNER JOIN send_message_large b ON a.id = b.send_message_id
            LEFT JOIN send_message_encoder c ON a.id = c.send_message_id
            {whereClause}
            ORDER BY a.create_time DESC
            LIMIT @page_size OFFSET @offset;
            """;

        return await DbHelper.QueryAsync(sql, MapView, parameters, cancellationToken);
    }

    private static (string WhereClause, List<SqliteParameter> Parameters) BuildWhereConditions(
        long instrumentId,
        SendMessage.StatusEnum? status,
        string barcode,
        string sampleNo,
        long createTimeStart,
        long createTimeEnd)
    {
        var conditions = new List<string>
        {
            "a.instrument_id = @instrument_id"
        };
        var parameters = new List<SqliteParameter>
        {
            DbHelper.Param("@instrument_id", instrumentId)
        };

        if (status.HasValue)
        {
            conditions.Add("a.status = @status");
            parameters.Add(DbHelper.Param("@status", (byte)status.Value));
        }

        conditions.Add("a.create_time >= @create_time_start");
        parameters.Add(DbHelper.Param("@create_time_start", createTimeStart));
        conditions.Add("a.create_time <= @create_time_end");
        parameters.Add(DbHelper.Param("@create_time_end", createTimeEnd));

        if (!string.IsNullOrWhiteSpace(barcode))
        {
            conditions.Add("a.barcode = @barcode");
            parameters.Add(DbHelper.Param("@barcode", barcode));
        }

        if (!string.IsNullOrWhiteSpace(sampleNo))
        {
            conditions.Add("a.sample_no = @sample_no");
            parameters.Add(DbHelper.Param("@sample_no", sampleNo));
        }

        return ("WHERE " + string.Join(" AND ", conditions), parameters);
    }

    private const string SelectColumns =
        "SELECT id, instrument_id, type, external_no, sample_no, barcode, status, error_message, create_time, update_time FROM send_message";

    private static SendMessage Map(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt64(0),
        InstrumentId = reader.GetInt64(1),
        Type = (SendMessage.TypeEnum)reader.GetByte(2),
        ExternalNo = reader.GetString(3),
        SampleNo = reader.GetString(4),
        Barcode = reader.GetString(5),
        Status = (SendMessage.StatusEnum)reader.GetByte(6),
        ErrorMessage = reader.GetString(7),
        CreateTime = reader.GetInt64(8),
        UpdateTime = reader.GetInt64(9)
    };

    private static SendMessageView MapView(SqliteDataReader reader) => new()
    {
        Status = (SendMessage.StatusEnum)reader.GetByte(0),
        SendJson = reader.GetString(1),
        SendContent = reader.IsDBNull(2) ? string.Empty : Convert.ToBase64String(reader.GetFieldValue<byte[]>(2)),
        Barcode = reader.GetString(3),
        SampleNo = reader.GetString(4),
        CreateTime = reader.GetInt64(5),
        ErrorMessage = reader.GetString(6)
    };
}
