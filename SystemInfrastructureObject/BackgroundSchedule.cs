namespace Birdsoft.SecuIntegrator24.SystemInfrastructureObject.TaskScheduleManager;

using System;

/// <summary>
///     定義背景任務的排程時間
///     define the schedule time for background task
/// </summary>
public class BackgroundSchedule
{
    /// <summary>
    ///     每週的星期幾
    ///     Weekly days
    /// </summary>
    public List<DayOfWeek> WeeklyDays { get; set; } = new List<DayOfWeek>();

    /// <summary>
    ///     每月的日期
    ///     Monthly days
    /// </summary>
    public List<int> MonthlyDays { get; set; } = new List<int>();

    /// <summary>
    ///     每年的日期
    ///     Yearly days
    /// </summary>
    public List<DateTime> YearlyDates { get; set; }  = new List<DateTime>();

    /// <summary>
    ///     執行的時間
    ///     Execution time
    /// </summary>
    public List<DateTime> ExecutionTimes { get; set; } = new List<DateTime>();

    /// <summary>
    ///     是否啟動時執行
    ///     Run on startup
    /// </summary>
    public bool RunOnStartup { get; set; } = true;
}
