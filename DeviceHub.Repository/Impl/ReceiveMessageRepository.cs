using DeviceHub.Model.Entities;
using DeviceHub.Model.view;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository.Repositories;

/// <summary>
/// 接收仪器消息队列表数据访问
/// </summary>
public class ReceiveMessageRepository : IReceiveMessageRepository
{
    private static readonly ReceiveMessageRepository _instance = new();
    public static ReceiveMessageRepository Instance => _instance;
    private ReceiveMessageRepository()
    {
    }

    public Task<long> Insert(ReceiveMessage entity, CancellationToken cancellationToken = default) =>
        Insert(entity, null, null, cancellationToken);

    public async Task<long> Insert(
        ReceiveMessage entity,
        SqliteConnection? connection,
        SqliteTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO receive_message (instrument_id, status, error_message, create_time, update_time)
            VALUES (@instrument_id, @status, @error_message, @create_time, @update_time)
            RETURNING id;
            """;

        var parameters = new SqliteParameter[]
        {
            DbHelper.Param("@instrument_id", entity.InstrumentId),
            DbHelper.Param("@status", (byte)entity.Status),
            DbHelper.Param("@error_message", entity.ErrorMessage),
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

    public async Task<bool> Update(ReceiveMessage entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE receive_message
            SET instrument_id = @instrument_id,
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
                DbHelper.Param("@status", (byte)entity.Status),
                DbHelper.Param("@error_message", entity.ErrorMessage),
                DbHelper.Param("@create_time", entity.CreateTime),
                DbHelper.Param("@update_time", entity.UpdateTime)
            ],
            cancellationToken);

        return rows > 0;
    }

    public async Task<bool> UpdateStatusAndErrorMessageAndUpdateTimeById(
        long id,
        ReceiveMessage.StatusEnum status,
        string errorMessage,
        long updateTime,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE receive_message
            SET status = @status,
                error_message = @error_message,
                update_time = @update_time
            WHERE id = @id;
            """;

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [
                DbHelper.Param("@id", id),
                DbHelper.Param("@status", (byte)status),
                DbHelper.Param("@error_message", errorMessage),
                DbHelper.Param("@update_time", updateTime)
            ],
            cancellationToken);

        return rows > 0;
    }

    public async Task<int> UpdateStatusAndErrorMessageAndUpdateTimeByIds(
        IReadOnlyList<long> ids,
        ReceiveMessage.StatusEnum status,
        string errorMessage,
        long updateTime,
        CancellationToken cancellationToken = default)
    {
        if (ids.Count == 0)
        {
            return 0;
        }

        var parameters = new List<SqliteParameter>(ids.Count + 3)
        {
            DbHelper.Param("@status", (byte)status),
            DbHelper.Param("@error_message", errorMessage),
            DbHelper.Param("@update_time", updateTime)
        };

        var idParams = new string[ids.Count];
        for (int i = 0; i < ids.Count; i++)
        {
            string paramName = $"@id{i}";
            idParams[i] = paramName;
            parameters.Add(DbHelper.Param(paramName, ids[i]));
        }

        string sql = $"""
            UPDATE receive_message
            SET status = @status,
                error_message = @error_message,
                update_time = @update_time
            WHERE id IN ({string.Join(", ", idParams)});
            """;

        return await DbHelper.ExecuteNonQueryAsync(sql, parameters.ToArray(), cancellationToken);
    }

    public Task<bool> UpdateStatusAndUpdateTimeById(
        long id,
        ReceiveMessage.StatusEnum status,
        long updateTime,
        CancellationToken cancellationToken = default) =>
        UpdateStatusAndUpdateTimeById(id, status, updateTime, null, null, cancellationToken);

    public async Task<bool> UpdateStatusAndUpdateTimeById(
        long id,
        ReceiveMessage.StatusEnum status,
        long updateTime,
        SqliteConnection? connection,
        SqliteTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE receive_message
            SET status = @status,
                update_time = @update_time
            WHERE id = @id;
            """;

        var parameters = new SqliteParameter[]
        {
            DbHelper.Param("@id", id),
            DbHelper.Param("@status", (byte)status),
            DbHelper.Param("@update_time", updateTime)
        };

        int rows = connection is not null && transaction is not null
            ? await DbHelper.ExecuteNonQueryAsync(connection, transaction, sql, parameters, cancellationToken)
            : await DbHelper.ExecuteNonQueryAsync(sql, parameters, cancellationToken);

        return rows > 0;
    }

    public async Task<bool> DeleteById(long id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM receive_message WHERE id = @id;";

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [DbHelper.Param("@id", id)],
            cancellationToken);

        return rows > 0;
    }

    public Task<ReceiveMessage?> GetById(long id, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            "SELECT id, instrument_id, status, error_message, create_time, update_time FROM receive_message WHERE id = @id;",
            Map,
            [DbHelper.Param("@id", id)],
            cancellationToken);

    public async Task<IReadOnlyList<ReceiveMessage>> GetAll(CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            "SELECT id, instrument_id, status, error_message, create_time, update_time FROM receive_message ORDER BY id;",
            Map,
            cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<ReceiveMessage>> GetByInstrumentId(long instrumentId, CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            "SELECT id, instrument_id, status, error_message, create_time, update_time FROM receive_message WHERE instrument_id = @instrument_id ORDER BY id;",
            Map,
            [DbHelper.Param("@instrument_id", instrumentId)],
            cancellationToken);

    public async Task<IReadOnlyList<ReceiveMessage>> GetByStatus(ReceiveMessage.StatusEnum status, CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            "SELECT id, instrument_id, status, error_message, create_time, update_time FROM receive_message WHERE status = @status ORDER BY id;",
            Map,
            [DbHelper.Param("@status", (byte)status)],
            cancellationToken);

    public async Task<List<ReceiveMessage>> FindByInstrumentIdAndStatusOrderAsc(
        long instrumentId,
        ReceiveMessage.StatusEnum status,
        int limit,
        CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            """
            SELECT id, instrument_id, status, error_message, create_time, update_time
            FROM receive_message
            WHERE instrument_id = @instrument_id AND status = @status
            ORDER BY id ASC
            LIMIT @limit;
            """,
            Map,
            [
                DbHelper.Param("@instrument_id", instrumentId),
                DbHelper.Param("@status", (byte)status),
                DbHelper.Param("@limit", limit)
            ],
            cancellationToken);

    public async Task<int> findCount(long instrumentId, ReceiveMessage.StatusEnum? status, ReceiveMessageDecode.TypeEnum? type,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, CancellationToken cancellationToken = default)
    {
        var (whereClause, parameters) = BuildWhereConditions(instrumentId, status, type, barcode, sampleNo, createTimeStart, createTimeEnd);
        var sql = $"""
            SELECT COUNT(*)
            FROM receive_message a
            INNER JOIN receive_message_large b ON a.id = b.receive_message_id
            LEFT JOIN receive_message_decode c ON a.id = c.receive_message_id
            {whereClause};
            """;

        var count = await DbHelper.ExecuteScalarAsync<long>(sql, parameters, cancellationToken);
        return (int)count;
    }

    public async Task<List<ReceiveMessageView>> findPageDesc(long instrumentId, ReceiveMessage.StatusEnum? status, ReceiveMessageDecode.TypeEnum? type,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex, CancellationToken cancellationToken = default)
    {
        var (whereClause, parameters) = BuildWhereConditions(instrumentId, status, type, barcode, sampleNo, createTimeStart, createTimeEnd);
        parameters.Add(DbHelper.Param("@page_size", pageSize));
        parameters.Add(DbHelper.Param("@offset", Math.Max(0, (pageIndex - 1) * pageSize)));

        var sql = $"""
            SELECT a.id, a.status, b.raw_message, c.result_json, c.type, c.barcode, c.sample_no, a.create_time, a.error_message
            FROM receive_message a
            INNER JOIN receive_message_large b ON a.id = b.receive_message_id
            LEFT JOIN receive_message_decode c ON a.id = c.receive_message_id
            {whereClause}
            ORDER BY a.create_time DESC
            LIMIT @page_size OFFSET @offset;
            """;

        return await DbHelper.QueryAsync(sql, MapView, parameters, cancellationToken);
    }

    private static (string WhereClause, List<SqliteParameter> Parameters) BuildWhereConditions(
        long instrumentId,
        ReceiveMessage.StatusEnum? status,
        ReceiveMessageDecode.TypeEnum? type,
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

        if (type.HasValue)
        {
            conditions.Add("c.type = @type");
            parameters.Add(DbHelper.Param("@type", (byte)type.Value));
        }

        if (!string.IsNullOrWhiteSpace(barcode))
        {
            conditions.Add("c.barcode = @barcode");
            parameters.Add(DbHelper.Param("@barcode", barcode));
        }

        if (!string.IsNullOrWhiteSpace(sampleNo))
        {
            conditions.Add("c.sample_no = @sample_no");
            parameters.Add(DbHelper.Param("@sample_no", sampleNo));
        }

        return ("WHERE " + string.Join(" AND ", conditions), parameters);
    }

    private static ReceiveMessage Map(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt64(0),
        InstrumentId = reader.GetInt64(1),
        Status = (ReceiveMessage.StatusEnum)reader.GetByte(2),
        ErrorMessage = reader.GetString(3),
        CreateTime = reader.GetInt64(4),
        UpdateTime = reader.GetInt64(5)
    };

    private static ReceiveMessageView MapView(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt64(0),
        Status = (ReceiveMessage.StatusEnum)reader.GetByte(1),
        RawMessage = reader.IsDBNull(2) ? [] : reader.GetFieldValue<byte[]>(2),
        ResultJson = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
        Type = reader.IsDBNull(4) ? null : (ReceiveMessageDecode.TypeEnum)reader.GetByte(4),
        Barcode = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
        SampleNo = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
        CreateTime = reader.GetInt64(7),
        ErrorMessage = reader.GetString(8)
    };
}
