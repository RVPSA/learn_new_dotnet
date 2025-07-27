using BackgroundServiceUserStop.BackgroundServices.IBackground;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundServiceUserStop.Controllers;

[Route("[controller]/[action]")]
public class TaskCancellationController:ControllerBase
{
    private readonly ILogger _logger;
    private readonly IRunningTaskRegistry _runningTaskRegistry;
    public TaskCancellationController(ILogger<TaskCancellationController> logger, 
        IRunningTaskRegistry runningTaskRegistry)
    {
        _logger = logger;
        _runningTaskRegistry = runningTaskRegistry;
    }
    [HttpPost]
    public IActionResult CancelTask(string taskId)
    {
        if (string.IsNullOrWhiteSpace(taskId))
        {
            return BadRequest("Task ID cannot be empty.");
        }

        _logger.LogInformation("Received cancellation request for Task ID: {taskId}", taskId);

        if (_runningTaskRegistry.TryGetCancellationTokenSource(taskId, out var cts))
        {
            if (!cts.IsCancellationRequested)
            {
                cts.Cancel(); // Signal cancellation!
                _logger.LogInformation("Cancellation signal sent for Task ID: {taskId}", taskId);
                return Ok($"Cancellation signal sent for task {taskId}.");
            }
            else
            {
                _logger.LogInformation("Task ID: {taskId} was already cancelled.", taskId);
                return Conflict($"Task {taskId} was already cancelled.");
            }
        }
        else
        {
            _logger.LogWarning("Cancellation requested for unknown or completed Task ID: {taskId}", taskId);
            return NotFound($"Task {taskId} not found or already completed.");
        }
    }
}