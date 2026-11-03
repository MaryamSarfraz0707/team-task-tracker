using TeamTaskTracker.Api.Models;

namespace TeamTaskTracker.Api.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default);
}
