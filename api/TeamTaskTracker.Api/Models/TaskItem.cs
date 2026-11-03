namespace TeamTaskTracker.Api.Models;

public sealed class TaskItem
{
    public long TaskId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public TaskPriority Priority { get; init; }
    public TaskItemStatus Status { get; init; }
    public DateOnly? DueDate { get; init; }
    public long CreatedByUserId { get; init; }
    public string CreatedByName { get; init; } = string.Empty;
    public long? UpdatedByUserId { get; init; }
    public string? UpdatedByName { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
}
