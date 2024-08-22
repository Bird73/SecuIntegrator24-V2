namespace Birdsoft.SecuIntegrator24.SystemInfrastructureObject.TaskScheduleManager;

/// <summary>
///     Interface for background tasks.
///     背景任務的介面。
/// </summary>
public interface IBackgroundTask : INameable
{
    /// <summary>
    ///     Start the task and implement the logic to run the task.
    ///     開始執行任務並實現運行任務的邏輯。
    /// </summary>
    /// <param name="cancellationToken"></param>
    public Task Start(CancellationToken cancellationToken);
}

/// <summary>
///     Interface for objects that have a name.
///     具有名稱的對象的介面。
/// </summary>
public interface INameable
{
    public string Name { get; }
}


