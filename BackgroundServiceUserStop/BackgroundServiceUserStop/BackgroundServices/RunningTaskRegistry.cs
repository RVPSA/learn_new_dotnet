using System.Collections.Concurrent;
using BackgroundServiceUserStop.BackgroundServices.IBackground;
using BackgroundServiceUserStop.Models;

namespace BackgroundServiceUserStop.BackgroundServices;

public class RunningTaskRegistry : IRunningTaskRegistry
    {
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _runningTasks = 
            new ConcurrentDictionary<string, CancellationTokenSource>();
        // Stores TaskProgress for status reporting
        private readonly ConcurrentDictionary<string, TaskProgress> _taskStatuses = 
            new ConcurrentDictionary<string, TaskProgress>();
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
                        UpdateTaskStatusInternal(kvp.Key, status =>
                        {
                            status.Status = TaskExecutionStatus.Cancelled;
                            status.Message = "Task cancelled by host shutdown.";
                            status.EndTime = DateTimeOffset.UtcNow;
                        });
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "Error cancelling task {taskId} in registry.", kvp.Key);
                }
            }
        }
        
        // --- Status Management ---
        public void AddOrUpdateTaskStatus(string taskId, TaskProgress progress)
        {
            _taskStatuses.AddOrUpdate(taskId, progress,
                (key, existingProgress) => 
                {
                    existingProgress.Status = progress.Status;
                    existingProgress.ProgressPercentage = progress.ProgressPercentage;
                    existingProgress.Message = progress.Message;
                    existingProgress.StartTime = progress.StartTime ?? existingProgress.StartTime;
                    existingProgress.EndTime = progress.EndTime ?? existingProgress.EndTime;
                    return existingProgress;
                });
            _logger.LogTrace("Task {taskId} status updated to {status} ({progress}%): {message}", taskId, progress.Status, progress.ProgressPercentage, progress.Message);
        }

        public TaskProgress? GetTaskStatus(string taskId)
        {
            _taskStatuses.TryGetValue(taskId, out var progress);
            return progress;
        }

        public IEnumerable<TaskProgress> GetAllTaskStatuses()
        {
            return _taskStatuses.Values;
        }

        // Internal helper to update status gracefully
        private void UpdateTaskStatusInternal(string taskId, Action<TaskProgress> updateAction)
        {
             _taskStatuses.AddOrUpdate(taskId,
                 (key) => {
                     _logger.LogWarning("Attempted to update status for non-existent task {taskId}. Creating new entry.", taskId);
                     var newProgress = new TaskProgress { TaskId = taskId, EnqueuedTime = DateTimeOffset.UtcNow };
                     updateAction(newProgress);
                     return newProgress;
                 },
                 // Update method for existing key
                 (key, existingProgress) => {
                     updateAction(existingProgress);
                     return existingProgress;
                 }
             );
        }
    }