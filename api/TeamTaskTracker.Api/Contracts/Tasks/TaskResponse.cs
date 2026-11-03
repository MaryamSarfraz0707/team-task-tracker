using TeamTaskTracker.Api.Models;

namespace TeamTaskTracker.Api.Contracts.Tasks;

public sealed record TaskResponse(
    long TaskId,
    string Title,
    string? Description,
    TaskPriority Priority,
    TaskItemStatus Status,
    DateOnly? DueDate,
    long CreatedByUserId,
    string CreatedByName,
    long? UpdatedByUserId,
    string? UpdatedByName,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    bool IsOverdue,
    bool IsDueSoon,
    string UrgencyLabel);
