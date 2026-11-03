using Microsoft.AspNetCore.Mvc;
using TeamTaskTracker.Api.Contracts.Tasks;
using TeamTaskTracker.Api.Services;

namespace TeamTaskTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TaskResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var tasks = await _taskService.GetAllAsync(cancellationToken);
        return Ok(tasks);
    }

    [HttpGet("{taskId:long}")]
    public async Task<ActionResult<TaskResponse>> GetById(long taskId, CancellationToken cancellationToken)
    {
        var task = await _taskService.GetByIdAsync(taskId, cancellationToken);
        return task is null ? NotFound() : Ok(task);
    }
}
