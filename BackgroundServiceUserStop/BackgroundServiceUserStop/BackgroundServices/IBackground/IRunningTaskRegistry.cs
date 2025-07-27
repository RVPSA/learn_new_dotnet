namespace BackgroundServiceUserStop.BackgroundServices.IBackground;

public interface IRunningTaskRegistry
{
    void AddRunningTask(string taskId, CancellationTokenSource cts);
    bool TryGetCancellationTokenSource(string taskId, out CancellationTokenSource? cts);
    void RemoveRunningTask(string taskId);
    void CancelAllTasks(); // For host shutdown
}