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
