using TeamTaskTracker.Api.Contracts.Users;

namespace TeamTaskTracker.Api.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserResponse?> GetByIdAsync(long userId, CancellationToken cancellationToken = default);
}
