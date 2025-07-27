using BackgroundServiceUserStop.BackgroundServices.IBackground;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundServiceUserStop.Controllers;

[Route("[controller]/[action]")]
public class TaskStatusController : ControllerBase
{
    private readonly IRunningTaskRegistry _taskRegistry;

    public TaskStatusController(IRunningTaskRegistry taskRegistry)
    {
        _taskRegistry = taskRegistry;
    }
    [HttpGet("{taskId}")]
    public IActionResult GetTaskStatus(string taskId)
    {
        if (string.IsNullOrWhiteSpace(taskId))
        {
            return BadRequest("Task ID cannot be empty.");
        }

        var status = _taskRegistry.GetTaskStatus(taskId);
        if (status == null)
        {
            return NotFound($"Task {taskId} not found. It may not have been enqueued or was cleaned up.");
        }
        return Ok(status);
    }

    /// <summary>
    /// Gets the status of all currently tracked background tasks.
    /// </summary>
    [HttpGet("all")]
    public IActionResult GetAllTaskStatuses()
    {
        var allStatuses = _taskRegistry.GetAllTaskStatuses().OrderByDescending(t => t.EnqueuedTime);
        return Ok(allStatuses);
    }
}