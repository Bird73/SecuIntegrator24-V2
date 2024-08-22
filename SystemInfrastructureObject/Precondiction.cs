namespace Birdsoft.SecuIntegrator24.SystemInfrastructureObject.TaskScheduleManager;

/// <summary>
/// 定義執行任務前必須滿足的先決條件類型
/// Defines the types of preconditions that must be met before a task can be executed.
/// </summary>
public enum PreconditionType
{
    /// <summary>
    /// 所有指定的任務必須完成
    /// All specified tasks must be completed
    /// </summary>
    AllTasksCompleted,

    /// <summary>
    /// 任何一個指定的任務完成即可
    /// Any one of the specified tasks may be completed
    /// </summary>
    AnyTaskCompleted
}



/// <summary>
///     定義執行任務前必須滿足的先決條件
///     Defines the preconditions that must be met before a task can be executed.
/// </summary>
public class Precondition
{
    /// <summary>
    ///     指定須符合條件的任務名稱
    ///     Specifies the names of the tasks that must meet the conditions
    /// </summary>
    public required List<string> TaskNames { get; set; }

    /// <summary>
    ///     指定先決條件類型, 預設為所有指定的任務必須完成
    ///     Specifies the type of precondition, default is that all specified tasks must be completed
    /// </summary>
    public required PreconditionType Type { get; set; }

    public Precondition()
    {
        TaskNames = new List<string>();
        Type = PreconditionType.AllTasksCompleted;
    }

    /// <summary>
    ///     Constructor for Precondition.
    ///     Precondition 的建構函數。
    /// </summary>
    /// <param name="taskNames"></param>
    /// <param name="type"></param>
    public Precondition(List<string> taskNames, PreconditionType type)
    {
        TaskNames = taskNames;
        Type = type;
    }
}