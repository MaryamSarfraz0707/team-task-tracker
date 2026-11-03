using TeamTaskTracker.Api.Contracts.Tasks;
using TeamTaskTracker.Api.Models;
using TeamTaskTracker.Api.Repositories;

namespace TeamTaskTracker.Api.Services;

public sealed class TaskService : ITaskService
{
    private const int DueSoonDays = 2;

    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IReadOnlyList<TaskResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tasks = await _taskRepository.GetAllAsync(cancellationToken);
        return tasks.Select(ToResponse).ToList();
    }

    public async Task<TaskResponse?> GetByIdAsync(long taskId, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);
        return task is null ? null : ToResponse(task);
    }

    private static TaskResponse ToResponse(TaskItem task)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var isDone = task.Status == TaskItemStatus.Done;
        var isOverdue = !isDone && task.DueDate.HasValue && task.DueDate.Value < today;
        var isDueSoon = !isDone && task.DueDate.HasValue && !isOverdue
                        && task.DueDate.Value <= today.AddDays(DueSoonDays);

        return new TaskResponse(
            task.TaskId,
            task.Title,
            task.Description,
            task.Priority,
            task.Status,
            task.DueDate,
            task.CreatedByUserId,
            task.CreatedByName,
            task.UpdatedByUserId,
            task.UpdatedByName,
            task.CreatedAtUtc,
            task.UpdatedAtUtc,
            isOverdue,
            isDueSoon,
            GetUrgencyLabel(task, isDone, isOverdue, isDueSoon));
    }

    private static string GetUrgencyLabel(TaskItem task, bool isDone, bool isOverdue, bool isDueSoon)
    {
        if (isDone)
        {
            return "Done";
        }

        if (isOverdue)
        {
            return "Overdue";
        }

        if (isDueSoon)
        {
            return "Due soon";
        }

        return task.DueDate.HasValue ? "On track" : "No due date";
    }
}
