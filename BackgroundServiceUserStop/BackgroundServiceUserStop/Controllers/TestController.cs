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
        // We set a timeout here, so if the task isn't cancelled manually,
        // it will eventually time out after, say, 5 minutes.
        var taskCts = new CancellationTokenSource();
        _runningTaskRegistry.AddRunningTask(request.Id,taskCts);
            await _taskQueue.EnqueueItems(request,async cancellationToken =>
            {
                await Task.Delay(20000);
                _logger.LogInformation("[Worker] Starting processing for data ID: {id}", request.Id);
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogInformation("[Worker] Validating data for ID: {id} - Data: {data}", request.Id, request.DataToValidate);
                if (request.DataToValidate.Length < 5)
                {
                    _logger.LogWarning("[Worker] Validation failed for ID: {id} - Data too short.", request.Id);
                    return;
                }
                _logger.LogInformation("[Worker] Validation successful for ID: {id}", request.Id);
                await Task.Delay(100, cancellationToken);

                // --- Simulate Database Operation ---
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogInformation("[Worker] Performing DB operation for ID: {id}", request.Id);
                await Task.Delay(500, cancellationToken); // Simulate async DB save
                _logger.LogInformation("[Worker] DB operation completed for ID: {id}", request.Id);


                // --- Simulate Sending Messages / Logging ---
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogInformation("[Worker] Sending notification message for ID: {id}", request.Id);
                await Task.Delay(200, cancellationToken);
                _logger.LogInformation("[Worker] Notification message sent for ID: {id}", request.Id);

                _logger.LogInformation("[Worker] Finished processing for data ID: {id}", request.Id);
            },taskCts);

            _logger.LogInformation("Data ID: {id} successfully enqueued. Returning 'Accepted'.", request.Id);
            return Accepted($"/api/status/{request.Id}"); // Return 202 Accepted, optionally with a status URL
    }
}