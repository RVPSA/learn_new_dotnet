using BackgroundServiceUserStop.BackgroundServices.IBackground;
using BackgroundServiceUserStop.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundServiceUserStop.Controllers;

[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IRunningTaskRegistry _runningTaskRegistry;

    public TestController(ILogger<TestController> logger, IBackgroundTaskQueue taskQueue,IRunningTaskRegistry runningTaskRegistry)
    {
        _logger = logger;
        _taskQueue = taskQueue;
        _runningTaskRegistry = runningTaskRegistry;
    }
    [HttpGet]
    public string Test()
    {
        
        return "Successfully established";
    }

    [HttpPost]
    public async Task<IActionResult> TestBackGround([FromBody] ProcessRequest request)
    {
        _logger.LogInformation("API received request for data ID: {id}", request.Id);
            
        // Create a CancellationTokenSource for THIS specific task

        var taskCts = new CancellationTokenSource();
        _runningTaskRegistry.AddRunningTask(request.Id,taskCts);
        UpdateTaskStatus(request.Id, 0, "Task enqueued, waiting for processing.");
        
            await _taskQueue.EnqueueItems(request,async cancellationToken =>
            {
                await Task.Delay(20000);
                
                UpdateTaskStatus(request.Id, 30, "Start Validation");
                
                _logger.LogInformation("[Worker] Starting processing for data ID: {id}", request.Id);
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogInformation("[Worker] Validating data for ID: {id} - Data: {data}", request.Id, request.DataToValidate);
                if (request.DataToValidate.Length < 5)
                {
                    _logger.LogWarning("[Worker] Validation failed for ID: {id} - Data too short.", request.Id);
                    return;
                }
                _logger.LogInformation("[Worker] Validation successful for ID: {id}", request.Id);
                await Task.Delay(1000, cancellationToken);
                
                UpdateTaskStatus(request.Id, 60, "Validation success");
                
                
                // --- Simulate Database Operation ---
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogInformation("[Worker] Performing DB operation for ID: {id}", request.Id);
                await Task.Delay(5000, cancellationToken); // Simulate async DB save
                _logger.LogInformation("[Worker] DB operation completed for ID: {id}", request.Id);
                
                UpdateTaskStatus(request.Id, 80, "DB Operation success");

                // --- Simulate Sending Messages / Logging ---
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogInformation("[Worker] Sending notification message for ID: {id}", request.Id);
                await Task.Delay(200, cancellationToken);
                _logger.LogInformation("[Worker] Notification message sent for ID: {id}", request.Id);

                UpdateTaskStatus(request.Id, 90, "Message sent");

                _logger.LogInformation("[Worker] Finished processing for data ID: {id}", request.Id);
            },taskCts);

            _logger.LogInformation("Data ID: {id} successfully enqueued. Returning 'Accepted'.", request.Id);
            return Accepted($"/api/status/{request.Id}"); // Return 202 Accepted, optionally with a status URL
    }

    private void UpdateTaskStatus(string taskId,int amount, string message)
    {
        var initialProgress4 = new TaskProgress
        {
            TaskId = taskId,
            Status = TaskExecutionStatus.Waiting,
            ProgressPercentage = amount,
            Message = message,
            EnqueuedTime = DateTimeOffset.UtcNow
        };
        _runningTaskRegistry.AddOrUpdateTaskStatus(taskId, initialProgress4);
    }
}