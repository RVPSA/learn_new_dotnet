using BackgroundServiceUserStop.BackgroundServices.IBackground;
using BackgroundServiceUserStop.Models;

namespace BackgroundServiceUserStop.BackgroundServices;

public class BackgroundWithBackgroundService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IRunningTaskRegistry _runningTaskRegistry;
    
    public BackgroundWithBackgroundService(ILogger<BackgroundWithBackgroundService> logger, 
        IBackgroundTaskQueue backgroundTaskQueue, IServiceScopeFactory serviceScopeFactory,
        IRunningTaskRegistry runningTaskRegistry)
    {
        _logger = logger;
        _backgroundTaskQueue = backgroundTaskQueue;
        _serviceScopeFactory = serviceScopeFactory;
        _runningTaskRegistry = runningTaskRegistry;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Data Processing Background Worker started at: {time}", DateTimeOffset.Now);
        
        // Register a callback for when the host is shutting down.
        // This ensures we signal all running tasks to stop too.
        stoppingToken.Register(() =>
        {
            _logger.LogInformation("Host shutdown requested. Cancelling all running tasks.");
            _runningTaskRegistry.CancelAllTasks();
        });
        
        while (!stoppingToken.IsCancellationRequested)
        {
            ProcessRequest? request = null;
            Func<CancellationToken, ValueTask>? workItem = null;
            CancellationTokenSource? taskCts = null;
            try
            {
                //Read workitem from queue
                (request,workItem,taskCts) = await _backgroundTaskQueue.DequeueItem(stoppingToken);
                
                // Register this task's CTS with the registry ONLY IF it's not already cancelled
                if (taskCts != null && !taskCts.IsCancellationRequested)
                {
                    //_runningTaskRegistry.AddRunningTask(request.Id, taskCts);
                    _runningTaskRegistry.AddOrUpdateTaskStatus(request.Id, new TaskProgress
                    {
                        TaskId = request.Id,
                        Status = TaskExecutionStatus.InProgress,
                        ProgressPercentage = 10,
                        Message = "Starting processing...",
                        StartTime = DateTimeOffset.UtcNow,
                        EnqueuedTime = _runningTaskRegistry.GetTaskStatus(request.Id)?.EnqueuedTime ?? DateTimeOffset.UtcNow // Preserve enqueued time
                    });
                    _logger.LogInformation("Dequeued task ID: {id}. Processing...", request.Id);
                }
                else
                {
                    _logger.LogWarning("Dequeued task ID: {id} but its CancellationTokenSource was null or already cancelled. Skipping.", request?.Id);
                    taskCts?.Dispose(); // Dispose immediately if already cancelled
                    continue; // Skip processing this already-cancelled task
                }
            }
            catch (OperationCanceledException)
            {
                // This is expected when the host is shutting down
                _logger.LogInformation("Data Processing Background Worker is stopping.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while dequeueing background work item.");
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken); //Small delay
                continue; // Continue to try and dequeue
            }

            try
            {
                // Create a new scope for each work item to ensure proper disposal of scoped services (e.g., DbContext)
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    await workItem(taskCts.Token); // Execute the enqueued work
                }
                _runningTaskRegistry.AddOrUpdateTaskStatus(request.Id, new TaskProgress
                {
                    TaskId = request.Id,
                    Status = TaskExecutionStatus.Completed,
                    ProgressPercentage = 100,
                    Message = "Task completed successfully.",
                    EndTime = DateTimeOffset.UtcNow
                });
            }
            catch (OperationCanceledException)
            {
                _runningTaskRegistry.AddOrUpdateTaskStatus(request.Id, new TaskProgress
                {
                    TaskId = request.Id,
                    Status = TaskExecutionStatus.Cancelled,
                    ProgressPercentage = _runningTaskRegistry.GetTaskStatus(request.Id)?.ProgressPercentage ?? 0, // Keep last reported progress
                    Message = "Task cancelled.",
                    EndTime = DateTimeOffset.UtcNow
                });
                _logger.LogInformation("Task ID: {id} was cancelled.", request?.Id);
            }
            catch (Exception ex)
            {
                _runningTaskRegistry.AddOrUpdateTaskStatus(request.Id, new TaskProgress
                {
                    TaskId = request.Id,
                    Status = TaskExecutionStatus.Failed,
                    ProgressPercentage = _runningTaskRegistry.GetTaskStatus(request.Id)?.ProgressPercentage ?? 0, // Keep last reported progress
                    Message = $"Task failed: {ex.Message}",
                    EndTime = DateTimeOffset.UtcNow
                });
                _logger.LogError(ex, "Error executing background work item.");
            }
            finally
            {
                // Clean up the task's CTS after it completes or is cancelled/errors out
                taskCts?.Dispose();
                if (request != null)
                {
                    _runningTaskRegistry.RemoveRunningTask(request.Id);
                    _logger.LogInformation("Cleaned up resources for task ID: {id}.", request.Id);
                }
            }
        }

        _logger.LogInformation("Data Processing Background Worker stopped at: {time}", DateTimeOffset.Now);
    }
}