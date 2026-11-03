namespace TeamTaskTracker.Api.Contracts.Users;

public sealed record UserResponse(
    long UserId,
    string FullName,
    string Email,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
