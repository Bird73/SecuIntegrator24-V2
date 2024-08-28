namespace Birdsoft.SecuIntegrator24.Tests;

using Birdsoft.SecuIntegrator24.SystemInfrastructureObject.TaskScheduleManager;

using Xunit;
using FluentAssertions;

/// <summary>
///     Test class for TaskScheduleManager.
/// </summary>
public class TC0202_TaskScheduleManager
{
    /// <summary>
    ///     Test method to register and execute a task on schedule.
    ///     註冊並在排程時間執行測試任務。
    /// </summary>
    [Fact]
    public void RegistersAndExecutesOnSchedule()
    {
        // Arrange
        var testFlag = new TestFlag();                                          // Flag for testing if the task is executed, 用於測試任務是否已執行

        var task = new TestTask1(testFlag);                                     // Test task for testing TaskScheduleManager, 用於測試TaskScheduleManager的測試任務
        
        // Create a background schedule that runs the task on the current day and time after 1 minute, 創建一個背景計劃，在1分鐘後的當天和時間運行任務
        var backgroundSchedule = new BackgroundSchedule();

        var today = DateTime.Today.Day;
        backgroundSchedule.MonthlyDays.Add(today);
        backgroundSchedule.ExecutionTimes.Add(DateTime.Now.AddMinutes(1));

        TaskScheduleManager.RegisterTask(task, backgroundSchedule, null);       // Register the task and no precondition, 註冊任務並無前提條件

        // Act
        TaskScheduleManager.StartAllTasks();

        Thread.Sleep(65000);        // wait for 65 seconds(over the 1 minute mark), 等待65秒(超過1分鐘標記)

        // Assert
        lock (testFlag)
        {
            Assert.True(testFlag.isTestTaskExecuted);       // assert that the task is executed, 確認任務是否已執行
        }
    }

    /// <summary>
    ///     Test method to cancel a task.
    ///     取消任務的測試方法。
    /// </summary>
    [Fact]
    public void CancelsTask()
    {
        // Arrange
        var testFlag = new TestFlag();                                          // Flag for testing if the task is executed, 用於測試任務是否已執行

        var task = new TestTask1(testFlag);
 
        // Create a background schedule that runs the task on the current day and time after 1 minute, 創建一個背景計劃，在1分鐘後的當天和時間運行任務
        var backgroundSchedule = new BackgroundSchedule();

        var today = DateTime.Today.Day;
        backgroundSchedule.MonthlyDays.Add(today);
        backgroundSchedule.ExecutionTimes.Add(DateTime.Now.AddMinutes(1));

        TaskScheduleManager.RegisterTask(task, backgroundSchedule, null);       // Register the task and no precondition, 註冊任務並無前提條件

        // Act
        TaskScheduleManager.StartAllTasks();

        Thread.Sleep(80000);                                // wait for 80 seconds, 等待80秒

        TaskScheduleManager.StopAllTasks();                 // Cancel the task, 取消任務

        // Assert
        lock (testFlag)
        {
            
            testFlag.isTestTaskCancelled.Should().BeTrue();     // assert that the task is cancelled, 確認任務是否已取消
        }
    }

    /// <summary>
    ///     Test method to register and execute tasks with precondition.
    ///     註冊並執行具有前提條件的任務的測試方法。
    /// </summary>
    [Fact]
    public void RegistersAndExecutesTasksWithPrecondition()
    {
        // Arrange
        var testFlag1 = new TestFlag();                                          // Flag for testing if TestTask1 is executed
        var testFlag2 = new TestFlag();                                          // Flag for testing if TestTask2 is executed

        var task1 = new TestTask1(testFlag1);                                    // Test task 1
        var task2 = new TestTask2(testFlag2);                                    // Test task 2

        // Create two background schedules that run the tasks at the same time after 1 minute, 創建兩個背景計劃，在1分鐘後的同一時間運行任務
        var startTime = DateTime.Now.AddMinutes(1);
        var backgroundSchedule1 = new BackgroundSchedule();
        var today = DateTime.Today.Day;
        backgroundSchedule1.MonthlyDays.Add(today);
        backgroundSchedule1.ExecutionTimes.Add(startTime);

        var backgroundSchedule2 = new BackgroundSchedule();
        backgroundSchedule2.MonthlyDays.Add(today);
        backgroundSchedule2.ExecutionTimes.Add(startTime);
        
        // Create a precondition that requires TestTask1 to be completed, 創建一個需要完成TestTask1的前提條件
        var precondition = new Precondition
        {
            TaskNames = new List<string> { "TestTask1" },
            Type = PreconditionType.AllTasksCompleted
        };

        TaskScheduleManager.RegisterTask(task1, backgroundSchedule1, null);                 // Register TestTask1
        TaskScheduleManager.RegisterTask(task2, backgroundSchedule2, precondition);         // Register TestTask2 with precondition

        // Act
        TaskScheduleManager.StartAllTasks();

        Thread.Sleep(90000);        // wait for 90 seconds, 等待90秒

        // Assert
        lock (testFlag1)
        {
            testFlag1.isTestTaskExecuted.Should().BeTrue();         // assert that TestTask1 is executed, 確認TestTask1是否已執行
            testFlag1.TaskEndTime.Should().BeNull();                // assert that TestTask1 is not cancelled, 確認TestTask1未取消
        
            lock (testFlag2)
            {
                testFlag2.isTestTaskExecuted.Should().BeFalse();    // assert that TestTask2 is not executed, 確認TestTask2未執行
            }
        }

        Thread.Sleep(180000);            // wait for 180 seconds, 等待180秒
        lock (testFlag1)
        {
            testFlag1.TaskEndTime.Should().NotBeNull();         // assert that TestTask1 is completed, 確認TestTask1已完成
            lock (testFlag2)
            {
                testFlag2.isTestTaskExecuted.Should().BeTrue();     // assert that TestTask2 is executed, 確認TestTask2已執行
                testFlag1.TaskEndTime.Should().NotBeNull();         // assert that TestTask1 is completed
                testFlag2.TaskStartTime.Value.Should().BeOnOrAfter(testFlag1.TaskEndTime.Value);       // assert that TestTask2 starts after TestTask1, 確認TestTask2在TestTask1之後開始
                
                // Assert.True(testFlag2.TaskStartTime >= testFlag1.TaskEndTime);       // assert that TestTask2 starts after TestTask1
            }
        }

        Thread.Sleep(120000);            // wait for 120 seconds, 等待120秒
        lock (testFlag2)
        {
            testFlag2.TaskEndTime.Should().NotBeNull();         // assert that TestTask2 is completed, 確認TestTask2已完成
        }
    }
}

/// <summary>
///     Flag for testing if the task is executed.
/// </summary>
public class TestFlag
{
    /// <summary>
    ///     Flag for testing if the task is executed.
    ///     用於測試任務是否已執行的旗標。
    /// </summary>
    public bool isTestTaskExecuted { get; set; } = false;

    /// <summary>
    ///     Flag for testing if the task is cancelled.
    ///     用於測試任務是否已取消的旗標。
    /// </summary>
    public bool isTestTaskCancelled { get; set; } = false;

    /// <summary>
    ///     Start time of the task.
    ///     任務的開始時間。
    /// </summary>
    public DateTime? TaskStartTime { get; set; }

    /// <summary>
    ///     End time of the task.
    ///     任務的結束時間。
    /// </summary>
    public DateTime? TaskEndTime { get; set; }
}

/// <summary>
///     Test task for testing TaskScheduleManager.
/// </summary>
public class TestTask1 : IBackgroundTask
{
    /// <summary>
    ///     Name of the task.
    ///     任務的名稱。
    /// </summary>
    public string Name => "TestTask1";

    /// <summary>
    ///     Flag for testing
    ///     用於測試的旗標
    /// </summary>
    private TestFlag _testFlag;

    /// <summary>
    ///     Constructor for TestTask1.
    ///     TestTask1的建構函數。
    /// </summary>
    /// <param name="testFlag"></param>
    public TestTask1(TestFlag testFlag)
    {
        _testFlag = testFlag;
    }

    /// <summary>
    ///     Start the task and implement the logic to run the task.
    ///     開始執行任務並實現運行任務的邏輯。
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task Start(CancellationToken token)
    {
        // Set the flag for the task is executed and start time, 設置任務已執行的旗標和開始時間
        lock (_testFlag)
        {
            _testFlag.isTestTaskExecuted = true;
            _testFlag.TaskStartTime = DateTime.Now;
        }

        // Run the task for 1 minute, 在1分鐘內運行任務
        DateTime endTime = DateTime.Now.AddMinutes(2);
        while (DateTime.Now < endTime && !token.IsCancellationRequested)
        {
            // Do nothing, 不做任何事
            await Task.Delay(1000);
        }

        // Set the flag if the task is cancelled and end time, 設置任務是否已取消的旗標和結束時間
        lock (_testFlag)
        {
            _testFlag.isTestTaskCancelled = token.IsCancellationRequested;
            _testFlag.TaskEndTime = DateTime.Now;
        }
    }
}

public class TestTask2 : IBackgroundTask
{
    public string Name => "TestTask2";

    private TestFlag _testFlag;

    public TestTask2(TestFlag testFlag)
    {
        _testFlag = testFlag;
    }

    public async Task Start(CancellationToken token)
    {
        lock (_testFlag)
        {
            _testFlag.isTestTaskExecuted = true;
            _testFlag.TaskStartTime = DateTime.Now;
        }

        DateTime endTime = DateTime.Now.AddMinutes(2);
        while (DateTime.Now < endTime && !token.IsCancellationRequested)
        {
            // Do nothing, 不做任何事
            await Task.Delay(1000);
        }

        lock (_testFlag)
        {
            _testFlag.isTestTaskCancelled = token.IsCancellationRequested;
            _testFlag.TaskEndTime = DateTime.Now;
        }
    }
}