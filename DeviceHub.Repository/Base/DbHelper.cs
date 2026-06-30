using DeviceHub.Base.Common;
using Microsoft.Data.Sqlite;

namespace DeviceHub.Repository;

/// <summary>
/// SQLite 数据库访问辅助类
/// </summary>
public static class DbHelper
{
    private static string _connectionString = "";

    /// <summary>
    /// 当前连接字符串
    /// </summary>
    public static string ConnectionString => _connectionString;

    /// <summary>
    /// 配置数据库连接字符串
    /// </summary>
    public static void Configure(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        _connectionString = connectionString;
    }

    /// <summary>
    /// 初始化数据库并创建表结构
    /// </summary>
    public static async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var statements = SqliteSchema.CreateTablesSql
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var statement in statements)
            await ExecuteNonQueryAsync(statement, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 打开数据库连接
    /// </summary>
    public static async Task<SqliteConnection> OpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }

    /// <summary>
    /// 在事务中执行操作，成功时提交，异常时回滚
    /// </summary>
    public static async Task ExecuteInTransactionAsync(
        Func<SqliteConnection, SqliteTransaction, Task> action,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await OpenConnectionAsync(cancellationToken);
        await using var transaction = (SqliteTransaction)await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            await action(connection, transaction);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            Logger.Error(nameof(DbHelper), "事务执行失败，已回滚", ex);
            throw;
        }
    }

    /// <summary>
    /// 执行非查询 SQL
    /// </summary>
    public static async Task<int> ExecuteNonQueryAsync(
        string sql,
        IEnumerable<SqliteParameter>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await OpenConnectionAsync(cancellationToken);
            await using var command = CreateCommand(connection, sql, parameters);
            return await command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.Error(nameof(DbHelper), $"ExecuteNonQuery 失败: {sql}", ex);
            throw;
        }
    }

    /// <summary>
    /// 在指定连接和事务中执行非查询 SQL
    /// </summary>
    public static async Task<int> ExecuteNonQueryAsync(
        SqliteConnection connection,
        SqliteTransaction transaction,
        string sql,
        IEnumerable<SqliteParameter>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await using var command = CreateCommand(connection, sql, parameters, transaction);
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    /// <summary>
    /// 执行标量查询
    /// </summary>
    public static async Task<T?> ExecuteScalarAsync<T>(
        string sql,
        IEnumerable<SqliteParameter>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await OpenConnectionAsync(cancellationToken);
            await using var command = CreateCommand(connection, sql, parameters);
            var result = await command.ExecuteScalarAsync(cancellationToken);

            if (result is null or DBNull)
                return default;

            return (T)Convert.ChangeType(result, typeof(T));
        }
        catch (Exception ex)
        {
            Logger.Error(nameof(DbHelper), $"ExecuteScalar 失败: {sql}", ex);
            throw;
        }
    }

    /// <summary>
    /// 在指定连接和事务中执行标量查询
    /// </summary>
    public static async Task<T?> ExecuteScalarAsync<T>(
        SqliteConnection connection,
        SqliteTransaction transaction,
        string sql,
        IEnumerable<SqliteParameter>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await using var command = CreateCommand(connection, sql, parameters, transaction);
        var result = await command.ExecuteScalarAsync(cancellationToken);

        if (result is null or DBNull)
            return default;

        return (T)Convert.ChangeType(result, typeof(T));
    }

    /// <summary>
    /// 执行查询并映射结果集
    /// </summary>
    public static async Task<List<T>> QueryAsync<T>(
        string sql,
        Func<SqliteDataReader, T> mapper,
        IEnumerable<SqliteParameter>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await OpenConnectionAsync(cancellationToken);
            await using var command = CreateCommand(connection, sql, parameters);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var list = new List<T>();
            while (await reader.ReadAsync(cancellationToken))
                list.Add(mapper(reader));

            return list;
        }
        catch (Exception ex)
        {
            Logger.Error(nameof(DbHelper), $"Query 失败: {sql}", ex);
            throw;
        }
    }

    /// <summary>
    /// 执行查询并返回单条记录
    /// </summary>
    public static async Task<T?> QuerySingleAsync<T>(
        string sql,
        Func<SqliteDataReader, T> mapper,
        IEnumerable<SqliteParameter>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        var list = await QueryAsync(sql, mapper, parameters, cancellationToken);
        return list.Count > 0 ? list[0] : default;
    }

    /// <summary>
    /// 创建 SQL 参数
    /// </summary>
    public static SqliteParameter Param(string name, object? value) =>
        new(name, value ?? DBNull.Value);

    private static SqliteCommand CreateCommand(
        SqliteConnection connection,
        string sql,
        IEnumerable<SqliteParameter>? parameters,
        SqliteTransaction? transaction = null)
    {
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.Transaction = transaction;

        if (parameters is not null)
        {
            foreach (var parameter in parameters)
                command.Parameters.Add(parameter);
        }

        return command;
    }
}
