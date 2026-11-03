using System.Globalization;
using MySql.Data.MySqlClient;

namespace TeamTaskTracker.Api.Data;

public sealed class MySqlDbExecutor : IDbExecutor
{
    private readonly IDbConnectionFactory _connectionFactory;

    public MySqlDbExecutor(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<T>> QueryAsync<T>(
        string sql,
        Func<MySqlDataReader, T> map,
        IEnumerable<MySqlParameter>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = CreateCommand(connection, sql, parameters);
        await using var reader =
            (MySqlDataReader)await command.ExecuteReaderAsync(cancellationToken);

        var results = new List<T>();
        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(map(reader));
        }

        return results;
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(
        string sql,
        Func<MySqlDataReader, T> map,
        IEnumerable<MySqlParameter>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        var results = await QueryAsync(sql, map, parameters, cancellationToken);
        return results.Count == 0 ? default : results[0];
    }

    public async Task<int> ExecuteAsync(
        string sql,
        IEnumerable<MySqlParameter>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = CreateCommand(connection, sql, parameters);
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<T?> ExecuteScalarAsync<T>(
        string sql,
        IEnumerable<MySqlParameter>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = CreateCommand(connection, sql, parameters);
        var value = await command.ExecuteScalarAsync(cancellationToken);

        if (value is null or DBNull)
        {
            return default;
        }

        return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
    }

    private static MySqlCommand CreateCommand(
        MySqlConnection connection,
        string sql,
        IEnumerable<MySqlParameter>? parameters)
    {
        var command = new MySqlCommand(sql, connection);

        if (parameters is not null)
        {
            command.Parameters.AddRange(parameters.ToArray());
        }

        return command;
    }
}
