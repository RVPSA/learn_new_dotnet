using DotNetBackgroundService.BackgroundServices.IBackground;

namespace DotNetBackgroundService.BackgroundServices;

public class BackgroundWithBackgroundService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public BackgroundWithBackgroundService(ILogger<BackgroundWithBackgroundService> logger, 
        IBackgroundTaskQueue backgroundTaskQueue, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _backgroundTaskQueue = backgroundTaskQueue;
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Data Processing Background Worker started at: {time}", DateTimeOffset.Now);
        while (!stoppingToken.IsCancellationRequested)
        {
            Func<CancellationToken, ValueTask>? workItem = null;
            try
            {
                //Read workitem from queue
                workItem = await _backgroundTaskQueue.DequeueItem(stoppingToken);
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
                    await workItem(stoppingToken); // Execute the enqueued work
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing background work item.");
            }
        }

        _logger.LogInformation("Data Processing Background Worker stopped at: {time}", DateTimeOffset.Now);
    }
}