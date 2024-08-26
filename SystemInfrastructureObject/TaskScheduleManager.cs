namespace Birdsoft.SecuIntegrator24.SystemInfrastructureObject.TaskScheduleManager;

using Birdsoft.SecuIntegrator24.SystemInfrastructureObject.LogManager;

using System.Timers;

/// <summary>
///     The task schedule manager.
///     任務排程管理器。
/// </summary>
public static class TaskScheduleManager
{
    /// <summary>
    ///     The status of the task
    ///     任務的狀態
    /// </summary>
    private enum TaskStatus
    {
        /// <summary>
        ///     The task is waiting to start
        ///     任務正在等待開始
        /// </summary>
        WaitingToStart,
        /// <summary>
        ///     The task is running
        ///     任務正在運行
        /// </summary>
        Running,
        /// <summary>
        ///     The task has completed
        ///     任務已完成
        /// </summary>
        Completed,
        /// <summary>
        ///     The task has been canceled
        ///     任務已取消
        /// </summary>
        Cancelled,
        /// <summary>
        ///     The task has faulted
        ///     任務已失敗
        /// </summary>
        Faulted
    }

    private class TaskWrapper
    {
        /// <summary>
        ///     The task
        ///     任務
        /// </summary>
        public IBackgroundTask Task { get; set; }

        /// <summary>
        ///     The schedule
        ///     排程
        /// </summary>
        public BackgroundSchedule Schedule { get; set; }

        /// <summary>
        ///     The precondition
        ///     前置條件
        /// </summary>
        public Precondition? Precondition { get; set; }

        /// <summary>
        ///     The status of the task
        ///     任務狀態
        /// </summary>
        public TaskStatus Status { get; set; } = TaskStatus.WaitingToStart;

        private Timer _timer = new Timer { AutoReset = false };
        private DateTime _nextRunTime;      // Keeps the next execution time calculated by CalculateNextRunTime, 保留CalculateNextRunTime計算的下一次執行時間

        /// <summary>
        ///     Constructor
        ///     建構函數
        /// </summary>
        /// <param name="task"></param>
        /// <param name="schedule"></param>
        /// <param name="precondition"></param>
        public TaskWrapper(IBackgroundTask task, BackgroundSchedule schedule, Precondition? precondition)
        {
            Task = task;
            Schedule = schedule;
            Precondition = precondition;
            _timer.Elapsed += TimerElapsed;
        }

        /// <summary>
        ///     Start the task
        ///     開始任務
        /// </summary>
        public void Start()
        {
            // Schedule next run time and start timer, 計劃下一次執行時間並啟動計時器
            ScheduleNextRun();      
        }

        /// <summary>
        ///     Schedule next run time and start timer
        ///     計劃下一次執行時間並啟動計時器
        /// </summary>
        private void ScheduleNextRun()
        {
            try
            {
                DateTime nextRunTime = CalculateNextRunTime();
                if (nextRunTime == DateTime.MaxValue)
                {
                    LogManager.Log(LogManager.LogLevel.Error, $"TaskWrapper.ScheduleNextRun faulted: CalculateNextRunTime returned DateTime.MaxValue");
                    
                    Status = TaskStatus.Faulted;

                    return;
                }

                _nextRunTime = nextRunTime;
                var interval = (nextRunTime - DateTime.Now).TotalMilliseconds;
                if (interval > 3600000)
                {
                    interval = 3600000;
                }

                SetTimer(interval);
            }
            catch (Exception ex)
            {
                Status = TaskStatus.Faulted;
                LogManager.Log(LogManager.LogLevel.Error, $"TaskWrapper.ScheduleNextRun faulted: {ex.Message}");
            }
        }

        /// <summary>
        ///    Calculate next run time based on schedule
        ///    根據排程計算下一次執行時間
        /// </summary>
        /// <returns></returns>
        private DateTime CalculateNextRunTime()
        {
            try
            {
                // Calculate next run time based on schedule
                DateTime now = DateTime.Now;
                DateTime nextRunTime = DateTime.MaxValue;

                // Calculate next run time for each execution time, and find the nearly execution time
                // 計算每個執行時間的下一次執行時間，並找出最接近的執行時間
                DateTime nearestExecutionTime = DateTime.MaxValue;
                foreach (var scheduleExecutionTime in Schedule.ExecutionTimes)
                {
                    DateTime tempNextRunTime = new DateTime(now.Year, now.Month, now.Day, scheduleExecutionTime.Hour, scheduleExecutionTime.Minute, scheduleExecutionTime.Second);
                    if (tempNextRunTime < now)
                    {
                        tempNextRunTime = tempNextRunTime.AddDays(1);
                    }

                    if (tempNextRunTime < nearestExecutionTime )
                    {
                        nearestExecutionTime = tempNextRunTime;
                    }
                }

                foreach (var yearlyDate in Schedule.YearlyDates)
                {
                    // Calculate next run time for each yearly date and find the nearest one, if the date has passed, add 1 year
                    // 計算每個年度日期的下一次執行時間，找到最接近的日期，如果日期已過，則加1年
                    DateTime tempNextRunTime = new DateTime(now.Year, yearlyDate.Month, yearlyDate.Day, nearestExecutionTime.Hour, nearestExecutionTime.Minute, nearestExecutionTime.Second);
                    if (tempNextRunTime < now)
                    {
                        tempNextRunTime = tempNextRunTime.AddYears(1);
                    }

                    if (tempNextRunTime < nextRunTime)
                    {
                        nextRunTime = tempNextRunTime;
                    }
                }

                foreach (var monthlyDay in Schedule.MonthlyDays)
                {
                    // Calculate next run time for each monthly day and find the nearest one, if the day has passed, add 1 month
                    // 計算每個月份的日期的下一次執行時間，找到最接近的日期，如果日期已過，則加1個月
                    DateTime tempNextRunTime = new DateTime(now.Year, now.Month, monthlyDay, nearestExecutionTime.Hour, nearestExecutionTime.Minute, nearestExecutionTime.Second);
                    if (tempNextRunTime < now)
                    {
                        tempNextRunTime = tempNextRunTime.AddMonths(1);
                    }

                    if (tempNextRunTime < nextRunTime)
                    {
                        nextRunTime = tempNextRunTime;
                    }
                }

                foreach (var weeklyDay in Schedule.WeeklyDays)
                {
                    // Calculate next run time for each weekly day and find the nearest one, if the day has passed, add 1 week
                    // 計算每個星期的日期的下一次執行時間，找到最接近的日期，如果日期已過，則加1周
                    DateTime tempNextRunTime = now.Date;
                    while (tempNextRunTime.DayOfWeek != weeklyDay)
                    {
                        tempNextRunTime = tempNextRunTime.AddDays(1);
                    }
                    tempNextRunTime = tempNextRunTime.AddHours(nearestExecutionTime.Hour).AddMinutes(nearestExecutionTime.Minute).AddSeconds(nearestExecutionTime.Second);
                    if (tempNextRunTime > now && tempNextRunTime < nextRunTime)
                    {
                        nextRunTime = tempNextRunTime;
                    }
                }

                return nextRunTime;
            }
            catch (Exception ex)
            {
                LogManager.Log(LogManager.LogLevel.Error, $"TaskWrapper.CalculateNextRunTime faulted: {ex.Message}");

                return DateTime.MaxValue;
            }
        }

        /// <summary>
        ///     Timer elapsed event
        ///     計時器過期事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TimerElapsed(object? sender, ElapsedEventArgs e)
        {
            try
            {
                if (Status == TaskStatus.WaitingToStart && !_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    // If the scheduled time has not arrived, set timer to the remaining time, 如果計劃的時間尚未到來，將計時器設置為剩餘時間
                    if (DateTime.Now < _nextRunTime)
                    {
                        // Set timer to the remaining time. if the remaining time is more than 15 minutes, set timer to 15 minutes.
                        // 設置計時器為剩餘時間。如果剩餘時間超過15分鐘，則將計時器設置為15分鐘。
                        var interval = (_nextRunTime - DateTime.Now).TotalMilliseconds;
                        if (interval > 900000)
                        {
                            interval = 900000;
                        }
                        SetTimer(interval);
                        return;
                    }

                    // Check preconditions, 檢查前置條件
                    if (Precondition != null)
                    {
                        if (!CheckPreconditions(Precondition))
                        {
                            // If preconditions are not met, set timer to 1 minute, 如果前置條件未滿足，將計時器設置為1分鐘
                            SetTimer(60000);
                            return;
                        }
                    }

                    Status = TaskStatus.Running;
                    try 
                    {
                        await Task.Start(_cancellationTokenSource.Token);

                        if (Status != TaskStatus.Cancelled && Status != TaskStatus.Faulted)
                        {
                            DateTime nextRunTime = CalculateNextRunTime();
                            if (nextRunTime == DateTime.MaxValue)
                            {
                                Status = TaskStatus.Faulted;

                                LogManager.Log(LogManager.LogLevel.Error, $"TaskWrapper.TimerElapsed faulted: CalculateNextRunTime returned DateTime.MaxValue");

                                return;
                            }

                            if (nextRunTime.Date > DateTime.Now.Date)
                            {
                                // Schedule next run time to tomorrow 00:00:00, 計劃下一次執行時間為明天00:00:00
                                nextRunTime = DateTime.Today.AddDays(1);
                                Status = TaskStatus.Completed;
                            }
                            else if (nextRunTime.Date == DateTime.Today)
                            {
                                // There is still time left today, schedule next run time to the calculated time, 今天還有時間，將下一次執行時間安排到計算的時間
                                Status = TaskStatus.WaitingToStart;
                            }

                            _nextRunTime = nextRunTime;

                            // Set timer to next run time, if the time is more than 15 minutes, set timer to 15 minutes.
                            // 如果時間超過15分鐘，則將計時器設置為15分鐘。
                            var interval = (nextRunTime - DateTime.Now).TotalMilliseconds;
                            if (interval > 900000)
                            {
                                interval = 900000;
                            }

                            SetTimer(interval);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Status = TaskStatus.Cancelled;
                    }
                    catch (Exception ex)
                    {
                        Status = TaskStatus.Faulted;
                        LogManager.Log(LogManager.LogLevel.Error, $"TaskWrapper.TimerElapsed faulted: {ex.Message}");
                    }
                }
                else if (Status == TaskStatus.Completed)
                {
                    ScheduleNextRun();
                }
            }
            catch (Exception ex)
            {
                Status = TaskStatus.Faulted;
                LogManager.Log(LogManager.LogLevel.Error, $"TaskWrapper.TimerElapsed faulted: {ex.Message}");
            }
        }

        /// <summary>
        ///     Set timer
        ///     設置計時器
        /// </summary>
        /// <param name="interval"></param>
        private void SetTimer(double interval)
        {
            _timer.Interval = interval;
            _timer.Start();
        }
    }

    private static List<TaskWrapper> _tasks = new List<TaskWrapper>();

    private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public static void RegisterTask(IBackgroundTask task, BackgroundSchedule schedule, Precondition? precondition)
    {
        _tasks.Add(new TaskWrapper(task, schedule, precondition));
    }

    public static void StartAllTasks()
    {
        foreach (var task in _tasks)
        {
            try
            {
                task.Start();
            }
            catch (Exception ex)
            {
                task.Status = TaskStatus.Faulted;
                LogManager.Log(LogManager.LogLevel.Error, $"TaskScheduleManager.StartAllTasks faulted: {ex.Message}");
            }
        }
    }

    /// <summary>
    ///     Stop all tasks
    ///     停止所有任務
    /// </summary>
    public static void StopAllTasks()
    {
        _cancellationTokenSource.Cancel();

        DateTime waitUntil = DateTime.Now.AddSeconds(5);

        bool allTasksStopped = false;
        while (!allTasksStopped && DateTime.Now < waitUntil)
        {
            allTasksStopped = true;
            foreach (var task in _tasks)
            {
                if (task.Status == TaskStatus.Running)
                {
                    allTasksStopped = false;
                    break;
                }
            }

            Thread.Sleep(200);
        }
    }

    /// <summary>
    ///     Check preconditions
    ///     檢查前置條件
    /// </summary>
    /// <param name="precondition"></param>
    /// <returns></returns>
    private static bool CheckPreconditions(Precondition precondition)
    {
        try
        {
            // Get tasks to check based on task names in precondition, 根據前置條件中的任務名稱獲取要檢查的任務
            var tasksToCheck = _tasks.Where(t => precondition.TaskNames.Contains(t.Task.Name)).ToList();
            bool conditionMet = precondition.Type switch
            {
                PreconditionType.AllTasksCompleted => tasksToCheck.All(t => t.Status == TaskStatus.Completed),
                PreconditionType.AnyTaskCompleted => tasksToCheck.Any(t => t.Status == TaskStatus.Completed),
                _ => false
            };

            return conditionMet;
        }
        catch (Exception ex)
        {
            LogManager.Log(LogManager.LogLevel.Error, $"TaskScheduleManager.CheckPreconditions faulted: {ex.Message}");

            return false;
        }
    }
}