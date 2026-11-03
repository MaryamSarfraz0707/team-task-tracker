using TeamTaskTracker.Api.Models;

namespace TeamTaskTracker.Api.Repositories;

public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TaskItem?> GetByIdAsync(long taskId, CancellationToken cancellationToken = default);
}
