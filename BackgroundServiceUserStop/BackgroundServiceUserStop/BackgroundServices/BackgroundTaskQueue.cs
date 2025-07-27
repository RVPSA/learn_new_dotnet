using System.Threading.Channels;
using BackgroundServiceUserStop.BackgroundServices.IBackground;
using BackgroundServiceUserStop.Models;

namespace BackgroundServiceUserStop.BackgroundServices;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<(ProcessRequest, Func<CancellationToken, ValueTask>, CancellationTokenSource)> _queue;

    public BackgroundTaskQueue()
    {
        var options = new UnboundedChannelOptions()
        {
            SingleReader = false, //Avoid using multiple background process
            SingleWriter = false
        };
        //Create channel to store workitems
        _queue = Channel.CreateUnbounded<(ProcessRequest, Func<CancellationToken, ValueTask>, CancellationTokenSource)>(options);
    }

    //Enqueue Itemes
    public async ValueTask EnqueueItems(ProcessRequest request,Func<CancellationToken, ValueTask> workItem,
        CancellationTokenSource cts)
    {
        if(request.Id == null)
            throw new ArgumentNullException(nameof(request.Id));
        if (workItem is null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }
        if (cts is null)
        {
            throw new ArgumentNullException(nameof(cts));
        }
        
        await _queue.Writer.WriteAsync((request, workItem, cts));
    }
    //Dequeue Items using background service
    public async ValueTask<(ProcessRequest,Func<CancellationToken, ValueTask>,CancellationTokenSource)> 
        DequeueItem(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}