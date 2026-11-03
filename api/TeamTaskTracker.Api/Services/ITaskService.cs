using TeamTaskTracker.Api.Contracts.Tasks;

namespace TeamTaskTracker.Api.Services;

public interface ITaskService
{
    Task<IReadOnlyList<TaskResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TaskResponse?> GetByIdAsync(long taskId, CancellationToken cancellationToken = default);
}
