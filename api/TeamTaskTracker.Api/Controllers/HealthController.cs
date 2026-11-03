using Microsoft.AspNetCore.Mvc;
using TeamTaskTracker.Api.Data;

namespace TeamTaskTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    private readonly IDbExecutor _dbExecutor;

    public HealthController(IDbExecutor dbExecutor)
    {
        _dbExecutor = dbExecutor;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "ok",
            timestampUtc = DateTime.UtcNow
        });
    }

    [HttpGet("database")]
    public async Task<IActionResult> GetDatabase(CancellationToken cancellationToken)
    {
        var result = await _dbExecutor.ExecuteScalarAsync<int>("SELECT 1", cancellationToken: cancellationToken);

        return Ok(new
        {
            status = result == 1 ? "ok" : "unexpected",
            database = "TeamTaskTracker"
        });
    }
}
