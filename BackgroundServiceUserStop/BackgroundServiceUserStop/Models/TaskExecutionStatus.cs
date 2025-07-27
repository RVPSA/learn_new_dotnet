namespace BackgroundServiceUserStop.Models;

public enum TaskExecutionStatus
{
    Waiting,
    InProgress,
    Completed,
    Failed,
    Cancelled
}