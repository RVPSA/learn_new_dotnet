using DotNetBackgroundService.BackgroundServices.IBackground;
using DotNetBackgroundService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBackgroundService.Controllers;

[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IBackgroundTaskQueue _taskQueue;

    public TestController(ILogger<TestController> logger, IBackgroundTaskQueue taskQueue)
    {
        _logger = logger;
        _taskQueue = taskQueue;
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
            await _taskQueue.EnqueueItems(async cancellationToken =>
            {
                await Task.Delay(10000);
                _logger.LogInformation("[Worker] Starting processing for data ID: {id}", request.Id);
                
                _logger.LogInformation("[Worker] Validating data for ID: {id} - Data: {data}", request.Id, request.DataToValidate);
                if (request.DataToValidate.Length < 5)
                {
                    _logger.LogWarning("[Worker] Validation failed for ID: {id} - Data too short.", request.Id);
                    return;
                }
                _logger.LogInformation("[Worker] Validation successful for ID: {id}", request.Id);
                await Task.Delay(100, cancellationToken);

                // --- Simulate Database Operation ---
                _logger.LogInformation("[Worker] Performing DB operation for ID: {id}", request.Id);
                await Task.Delay(500, cancellationToken); // Simulate async DB save
                _logger.LogInformation("[Worker] DB operation completed for ID: {id}", request.Id);


                // --- Simulate Sending Messages / Logging ---
                _logger.LogInformation("[Worker] Sending notification message for ID: {id}", request.Id);
                await Task.Delay(200, cancellationToken);
                _logger.LogInformation("[Worker] Notification message sent for ID: {id}", request.Id);

                _logger.LogInformation("[Worker] Finished processing for data ID: {id}", request.Id);
            });

            _logger.LogInformation("Data ID: {id} successfully enqueued. Returning 'Accepted'.", request.Id);
            return Accepted($"/api/status/{request.Id}"); // Return 202 Accepted, optionally with a status URL
    }
}