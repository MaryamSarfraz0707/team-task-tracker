using MySql.Data.MySqlClient;
using TeamTaskTracker.Api.Data;
using TeamTaskTracker.Api.Models;

namespace TeamTaskTracker.Api.Repositories;

public sealed class UserRepository : IUserRepository
{
    private const string SelectColumns = """
        SELECT UserId, FullName, Email, IsActive, CreatedAtUtc, UpdatedAtUtc
        FROM Users
        """;

    private readonly IDbExecutor _dbExecutor;

    public UserRepository(IDbExecutor dbExecutor)
    {
        _dbExecutor = dbExecutor;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = SelectColumns + "\nORDER BY FullName;";
        return await _dbExecutor.QueryAsync(sql, MapUser, cancellationToken: cancellationToken);
    }

    public Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var sql = SelectColumns + "\nWHERE UserId = @UserId\nLIMIT 1;";
        var parameters = new[]
        {
            new MySqlParameter("@UserId", MySqlDbType.Int64) { Value = userId }
        };

        return _dbExecutor.QuerySingleOrDefaultAsync(sql, MapUser, parameters, cancellationToken);
    }

    private static User MapUser(MySqlDataReader reader)
    {
        return new User
        {
            UserId = reader.GetInt64("UserId"),
            FullName = reader.GetString("FullName"),
            Email = reader.GetString("Email"),
            IsActive = reader.GetBoolean("IsActive"),
            CreatedAtUtc = reader.GetDateTime("CreatedAtUtc"),
            UpdatedAtUtc = reader.GetDateTime("UpdatedAtUtc")
        };
    }
}
