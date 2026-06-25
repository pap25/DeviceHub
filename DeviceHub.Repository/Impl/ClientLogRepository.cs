using DeviceHub.Model.Entities;
using DeviceHub.Model.Enums;
using DeviceHub.Repository.Interfaces;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository.Repositories;

/// <summary>
/// 仪器通信日志表数据访问
/// </summary>
public class ClientLogRepository : IClientLogRepository
{
    public async Task<long> InsertAsync(ClientLog entity, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO client_log (type, level, message, create_time)
            VALUES (@type, @level, @message, @create_time)
            RETURNING id;
            """;

        var id = await DbHelper.ExecuteScalarAsync<long>(
            sql,
            [
                DbHelper.Param("@type", entity.Type),
                DbHelper.Param("@level", (byte)entity.Level),
                DbHelper.Param("@message", entity.Message),
                DbHelper.Param("@create_time", entity.CreateTime)
            ],
            cancellationToken);

        return id;
    }

    public async Task<bool> UpdateAsync(ClientLog entity, CancellationToken cancellationToken = default)
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
                DbHelper.Param("@type", entity.Type),
                DbHelper.Param("@level", (byte)entity.Level),
                DbHelper.Param("@message", entity.Message),
                DbHelper.Param("@create_time", entity.CreateTime)
            ],
            cancellationToken);

        return rows > 0;
    }

    public async Task<bool> DeleteByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM client_log WHERE id = @id;";

        var rows = await DbHelper.ExecuteNonQueryAsync(
            sql,
            [DbHelper.Param("@id", id)],
            cancellationToken);

        return rows > 0;
    }

    public Task<ClientLog?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        DbHelper.QuerySingleAsync(
            SelectColumns + " WHERE id = @id;",
            Map,
            [DbHelper.Param("@id", id)],
            cancellationToken);

    public async Task<IReadOnlyList<ClientLog>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbHelper.QueryAsync(
            SelectColumns + " ORDER BY id;",
            Map,
            cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<ClientLog>> GetByLevelAsync(ClientLogLevel level, CancellationToken cancellationToken = default) =>
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
        Type = reader.GetByte(1),
        Level = (ClientLogLevel)reader.GetByte(2),
        Message = reader.GetString(3),
        CreateTime = reader.GetInt64(4)
    };
}
