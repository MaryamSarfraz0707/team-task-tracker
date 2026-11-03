using MySql.Data.MySqlClient;

namespace TeamTaskTracker.Api.Data;

public interface IDbExecutor
{
    Task<List<T>> QueryAsync<T>(
        string sql,
        Func<MySqlDataReader, T> map,
        IEnumerable<MySqlParameter>? parameters = null,
        CancellationToken cancellationToken = default);

    Task<T?> QuerySingleOrDefaultAsync<T>(
        string sql,
        Func<MySqlDataReader, T> map,
        IEnumerable<MySqlParameter>? parameters = null,
        CancellationToken cancellationToken = default);

    Task<int> ExecuteAsync(
        string sql,
        IEnumerable<MySqlParameter>? parameters = null,
        CancellationToken cancellationToken = default);

    Task<T?> ExecuteScalarAsync<T>(
        string sql,
        IEnumerable<MySqlParameter>? parameters = null,
        CancellationToken cancellationToken = default);
}
