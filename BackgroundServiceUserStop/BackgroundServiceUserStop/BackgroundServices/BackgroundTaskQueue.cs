using System.Threading.Channels;
using BackgroundServiceUserStop.BackgroundServices.IBackground;

namespace BackgroundServiceUserStop.BackgroundServices;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    public BackgroundTaskQueue()
    {
        var options = new UnboundedChannelOptions()
        {
            SingleReader = false, //Avoid using multiple background process
            SingleWriter = false
        };
        //Create channel to store workitems
        _queue = Channel.CreateUnbounded<Func<CancellationToken, ValueTask>>(options);
    }

    //Enqueue Itemes
    public async ValueTask EnqueueItems(Func<CancellationToken, ValueTask> workItem)
    {
        if (workItem is null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }
        await _queue.Writer.WriteAsync(workItem);
    }
    //Dequeue Items using background service
    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueItem(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}