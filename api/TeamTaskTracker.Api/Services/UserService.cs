using TeamTaskTracker.Api.Contracts.Users;
using TeamTaskTracker.Api.Models;
using TeamTaskTracker.Api.Repositories;

namespace TeamTaskTracker.Api.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(ToResponse).ToList();
    }

    public async Task<UserResponse?> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        return user is null ? null : ToResponse(user);
    }

    private static UserResponse ToResponse(User user)
    {
        return new UserResponse(
            user.UserId,
            user.FullName,
            user.Email,
            user.IsActive,
            user.CreatedAtUtc,
            user.UpdatedAtUtc);
    }
}
