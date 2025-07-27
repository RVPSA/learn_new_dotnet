namespace DotNetBackgroundService.BackgroundServices;

public class BackgroundWithIHosted(ILogger<BackgroundWithIHosted> logger) : IHostedService
{
    private readonly ILogger _logger = logger;
    private Timer _timer;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(_TimerCallback,null,TimeSpan.FromSeconds(10),TimeSpan.FromSeconds(10));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }

    private void _TimerCallback(object? state)
    {
        _logger.LogInformation($"This message is coming from background service {Guid.NewGuid().ToString()}");
    }
}