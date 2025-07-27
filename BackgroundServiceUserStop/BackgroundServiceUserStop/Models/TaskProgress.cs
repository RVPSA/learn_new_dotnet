namespace BackgroundServiceUserStop.Models;

public class TaskProgress
{
    public string TaskId { get; set; } = string.Empty;
    public TaskExecutionStatus Status { get; set; }
    public int ProgressPercentage { get; set; } // 0-100
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset EnqueuedTime { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public TimeSpan? Duration => StartTime.HasValue && EndTime.HasValue ? (EndTime.Value - StartTime.Value) : (TimeSpan?)null;
}