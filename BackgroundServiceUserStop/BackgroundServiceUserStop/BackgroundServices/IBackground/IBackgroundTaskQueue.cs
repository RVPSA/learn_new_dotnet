using BackgroundServiceUserStop.Models;

namespace BackgroundServiceUserStop.BackgroundServices.IBackground;

public interface IBackgroundTaskQueue
{
    ValueTask EnqueueItems(ProcessRequest request,Func<CancellationToken, ValueTask> workItem,CancellationTokenSource cts);
    ValueTask<(ProcessRequest request,Func<CancellationToken, ValueTask>,CancellationTokenSource cts)> 
        DequeueItem(CancellationToken cancellationToken);

}