using MySql.Data.MySqlClient;
using TeamTaskTracker.Api.Data;
using TeamTaskTracker.Api.Models;

namespace TeamTaskTracker.Api.Repositories;

public sealed class TaskRepository : ITaskRepository
{
    private const string SelectColumns = """
        SELECT t.TaskId, t.Title, t.Description, t.Priority, t.Status, t.DueDate,
               t.CreatedByUserId, creator.FullName AS CreatedByName,
               t.UpdatedByUserId, editor.FullName AS UpdatedByName,
               t.CreatedAtUtc, t.UpdatedAtUtc
        FROM Tasks t
        JOIN Users creator ON creator.UserId = t.CreatedByUserId
        LEFT JOIN Users editor ON editor.UserId = t.UpdatedByUserId
        """;

    private const string OrderByUrgency = """
        ORDER BY
            CASE
                WHEN t.Status <> 'Done' AND t.DueDate IS NOT NULL AND t.DueDate < CURRENT_DATE THEN 0
                WHEN t.Status <> 'Done' AND t.DueDate IS NOT NULL AND t.DueDate <= DATE_ADD(CURRENT_DATE, INTERVAL 2 DAY) THEN 1
                WHEN t.DueDate IS NULL THEN 3
                ELSE 2
            END,
            t.DueDate,
            t.Priority DESC,
            t.TaskId DESC;
        """;

    private readonly IDbExecutor _dbExecutor;

    public TaskRepository(IDbExecutor dbExecutor)
    {
        _dbExecutor = dbExecutor;
    }

    public async Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = SelectColumns + "\n" + OrderByUrgency;
        return await _dbExecutor.QueryAsync(sql, MapTask, cancellationToken: cancellationToken);
    }

    public Task<TaskItem?> GetByIdAsync(long taskId, CancellationToken cancellationToken = default)
    {
        var sql = SelectColumns + "\nWHERE t.TaskId = @TaskId\nLIMIT 1;";
        var parameters = new[]
        {
            new MySqlParameter("@TaskId", MySqlDbType.Int64) { Value = taskId }
        };

        return _dbExecutor.QuerySingleOrDefaultAsync(sql, MapTask, parameters, cancellationToken);
    }

    private static TaskItem MapTask(MySqlDataReader reader)
    {
        return new TaskItem
        {
            TaskId = reader.GetInt64("TaskId"),
            Title = reader.GetString("Title"),
            Description = reader.GetNullableString("Description"),
            Priority = Enum.Parse<TaskPriority>(reader.GetString("Priority"), ignoreCase: true),
            Status = Enum.Parse<TaskItemStatus>(reader.GetString("Status"), ignoreCase: true),
            DueDate = reader.GetNullableDateOnly("DueDate"),
            CreatedByUserId = reader.GetInt64("CreatedByUserId"),
            CreatedByName = reader.GetString("CreatedByName"),
            UpdatedByUserId = reader.GetNullableInt64("UpdatedByUserId"),
            UpdatedByName = reader.GetNullableString("UpdatedByName"),
            CreatedAtUtc = reader.GetDateTime("CreatedAtUtc"),
            UpdatedAtUtc = reader.GetDateTime("UpdatedAtUtc")
        };
    }
}
