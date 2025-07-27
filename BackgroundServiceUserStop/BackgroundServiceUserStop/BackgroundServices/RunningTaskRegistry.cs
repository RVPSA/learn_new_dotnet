using System.Collections.Concurrent;
using BackgroundServiceUserStop.BackgroundServices.IBackground;

namespace BackgroundServiceUserStop.BackgroundServices;

public class RunningTaskRegistry : IRunningTaskRegistry
    {
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _runningTasks = 
            new ConcurrentDictionary<string, CancellationTokenSource>();
        private readonly ILogger<RunningTaskRegistry> _logger;

        public RunningTaskRegistry(ILogger<RunningTaskRegistry> logger)
        {
            _logger = logger;
        }

        public void AddRunningTask(string taskId, CancellationTokenSource cts)
        {
            if (_runningTasks.TryAdd(taskId, cts))
            {
                _logger.LogDebug("Task {taskId} added to registry.", taskId);
            }
            else
            {
                _logger.LogWarning("Failed to add task {taskId} to registry. Already exists?", taskId);
            }
        }

        public bool TryGetCancellationTokenSource(string taskId, out CancellationTokenSource? cts)
        {
            return _runningTasks.TryGetValue(taskId, out cts);
        }

        public void RemoveRunningTask(string taskId)
        {
            if (_runningTasks.TryRemove(taskId, out var cts))
            {
                _logger.LogDebug("Task {taskId} removed from registry.", taskId);
            }
            else
            {
                _logger.LogWarning("Failed to remove task {taskId} from registry. Not found?", taskId);
            }
        }

        public void CancelAllTasks()
        {
            _logger.LogInformation("Cancelling all {count} registered tasks.", _runningTasks.Count);
            foreach (var kvp in _runningTasks)
            {
                try
                {
                    if (!kvp.Value.IsCancellationRequested)
                    {
                        kvp.Value.Cancel();
                        _logger.LogDebug("Signaled cancellation for task {taskId}.", kvp.Key);
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "Error cancelling task {taskId} in registry.", kvp.Key);
                }
            }
        }
    }