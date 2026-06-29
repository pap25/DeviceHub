using DeviceHub.Model.Entities;
using DeviceHub.Repository.Interfaces;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository.Repositories;

/// <summary>
/// 仪器通信日志表数据访问
/// </summary>
public class ClientLogRepository : IClientLogRepository
{
    private static readonly ClientLogRepository _instance = new();
    public static ClientLogRepository Instance => _instance;
    private ClientLogRepository()
    {
    }

    public async Task<long> Insert(ClientLog entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO client_log (type, level, message, create_time)
            VALUES (@type, @level, @message, @create_time)
            RETURNING id;
            """;

        var id = await DbHelper.ExecuteScalarAsync<long>(
            sql,
            [
                DbHelper.Param("@type", (byte)entity.Type),
                DbHelper.Param("@level", (byte)entity.Level),
                DbHelper.Param("@message", entity.Message),
                DbHelper.Param("@create_time", entity.CreateTime)
            ],
            cancellationToken);

        return id;
    }

    public async Task<bool> Update(ClientLog entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE client_log
            SET type = @type,
                level = @level,
                message = @message,
                create_time = @create_time
            WHERE id = @id;
            """;

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [
                DbHelper.Param("@id", entity.Id),
                DbHelper.Param("@type", (byte)entity.Type),
                DbHelper.Param("@level", (byte)entity.Level),
                DbHelper.Param("@message", entity.Message),
                DbHelper.Param("@create_time", entity.CreateTime)
            ],
            cancellationToken);

        return rows > 0;
    }

    public async Task<bool> DeleteById(long id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM client_log WHERE id = @id;";

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [DbHelper.Param("@id", id)],
            cancellationToken);

        return rows > 0;
    }

    public Task<ClientLog?> GetById(long id, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            SelectColumns + " WHERE id = @id;",
            Map,
            [DbHelper.Param("@id", id)],
            cancellationToken);

    public async Task<IReadOnlyList<ClientLog>> GetAll(CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            SelectColumns + " ORDER BY id;",
            Map,
            cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<ClientLog>> GetByType(ClientLog.TypeEnum type, CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            SelectColumns + " WHERE type = @type ORDER BY id;",
            Map,
            [DbHelper.Param("@type", (byte)type)],
            cancellationToken);

    public async Task<IReadOnlyList<ClientLog>> GetByLevel(ClientLog.LevelEnum level, CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            SelectColumns + " WHERE level = @level ORDER BY id;",
            Map,
            [DbHelper.Param("@level", (byte)level)],
            cancellationToken);

    public async Task<int> findCount(ClientLog.LevelEnum? level, ClientLog.TypeEnum? type,
        string message, long createTimeStart, long createTimeEnd, CancellationToken cancellationToken = default)
    {
        var (whereClause, parameters) = BuildWhereConditions(level, type, message, createTimeStart, createTimeEnd);
        var sql = $"""
            SELECT COUNT(*)
            FROM client_log a
            {whereClause};
            """;

        var count = await DbHelper.ExecuteScalarAsync<long>(sql, parameters, cancellationToken);
        return (int)count;
    }

    public async Task<List<ClientLog>> findPageDesc(ClientLog.LevelEnum? level, ClientLog.TypeEnum? type,
        string message, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex, CancellationToken cancellationToken = default)
    {
        var (whereClause, parameters) = BuildWhereConditions(level, type, message, createTimeStart, createTimeEnd);
        parameters.Add(DbHelper.Param("@page_size", pageSize));
        parameters.Add(DbHelper.Param("@offset", Math.Max(0, (pageIndex - 1) * pageSize)));

        var sql = $"""
            SELECT *
            FROM client_log a
            {whereClause}
            ORDER BY a.create_time DESC
            LIMIT @page_size OFFSET @offset;
            """;

        return await DbHelper.QueryAsync(sql, Map, parameters, cancellationToken);
    }

    private static (string WhereClause, List<SqliteParameter> Parameters) BuildWhereConditions(
        ClientLog.LevelEnum? level,
        ClientLog.TypeEnum? type,
        string message,
        long createTimeStart,
        long createTimeEnd)
    {
        var conditions = new List<string>();
        var parameters = new List<SqliteParameter>();

        if (level.HasValue)
        {
            conditions.Add("a.level = @level");
            parameters.Add(DbHelper.Param("@level", (byte)level.Value));
        }

        if (type.HasValue)
        {
            conditions.Add("a.type = @type");
            parameters.Add(DbHelper.Param("@type", (byte)type.Value));
        }

        if (!string.IsNullOrWhiteSpace(message))
        {
            conditions.Add("a.message LIKE @message");
            parameters.Add(DbHelper.Param("@message", $"%{message.Trim()}%"));
        }

        conditions.Add("a.create_time >= @create_time_start");
        parameters.Add(DbHelper.Param("@create_time_start", createTimeStart));
        conditions.Add("a.create_time <= @create_time_end");
        parameters.Add(DbHelper.Param("@create_time_end", createTimeEnd));

        return ("WHERE " + string.Join(" AND ", conditions), parameters);
    }

    private const string SelectColumns =
        "SELECT id, type, level, message, create_time FROM client_log";

    private static ClientLog Map(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt64(0),
        Type = (ClientLog.TypeEnum)reader.GetByte(1),
        Level = (ClientLog.LevelEnum)reader.GetByte(2),
        Message = reader.GetString(3),
        CreateTime = reader.GetInt64(4)
    };
}
