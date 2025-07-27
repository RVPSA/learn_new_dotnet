namespace BackgroundServiceUserStop.BackgroundServices.IBackground;

public interface IBackgroundTaskQueue
{
    ValueTask EnqueueItems(Func<CancellationToken, ValueTask> workItem);
    ValueTask<Func<CancellationToken, ValueTask>> DequeueItem(CancellationToken cancellationToken);

}