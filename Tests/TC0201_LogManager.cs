namespace Birdsoft.SecuIntegrator24.Tests;

using Birdsoft.SecuIntegrator24.SystemInfrastructureObject;

using System.Text.Json;
using Xunit;

/// <summary>
///     Unit Test for LogManager
///     日誌管理器測試
/// </summary>
public class TC0201_LogManager
{
    /// <summary>
    ///     Test if the log is written
    ///     測試是否寫入日誌
    /// </summary>
    [Fact]
    public void Test_Log()
    {
        // Arrange
        var level = LogManager.LogLevel.Information;
        var message = $"Test message : {Guid.NewGuid()}";       // Generate a unique message, 產生一個獨特的訊息
        var exception = new Exception("Test exception");

        var logEventTriggered = false;

        LogManager.Logged += (sender, e) =>
        {
            // Assert that event arguments are correct, 確認事件引數正確
            var logEntry = e;
            Assert.Equal(level, logEntry.Level);
            Assert.Equal(message, logEntry.Message);
            Assert.Equal(exception, logEntry.Exception);

            logEventTriggered = true;
        };

        // Act
        LogManager.Log(level, message, exception);

        Thread.Sleep(1000);     // Wait for the event to be triggered, 等待事件被觸發

        Assert.True(logEventTriggered); // Assert that the event was triggered, 確認事件被觸發

        // Assert that the log was written
        var filePath = $"Logs\\Log_{DateTime.Now:yyyyMMdd}.json";
        List<LogEntry> logEntries;
        try
        {
            var fileContent = File.ReadAllText(filePath);
            logEntries = JsonSerializer.Deserialize<List<LogEntry>>(fileContent);
        }
        catch (Exception ex)
        {
            Assert.True(false, $"Failed to deserialize log file. Exception: {ex.Message}");
            return;     // Early exit if deserialization fails, 如果反序列化失敗，提前退出
        }
        
        Assert.NotNull(logEntries);     // Assert that the log file exists, 確認檔案存在

        bool isEntryExits = logEntries.Any(e => e.Level == level && e.Message == message && e.Exception == exception);
        Assert.True(isEntryExits);      // Assert that the log entry exists, 確認 LogEntry 存在
    }

    /// <summary>
    ///     Test if old logs are cleaned up
    ///     測試是否清理舊日誌
    /// </summary>
    [Fact]
    public void Test_CleanUpOldLogs()
    {
        // Arrange
        uint retentionDays = 30;
        string logDirectory = "Logs";

        // Check if the Logs directory exists, 檢查 Logs 目錄是否存在
        if (!Directory.Exists(logDirectory))
            Directory.CreateDirectory(logDirectory);

        // Create old log files, 創建舊的日誌文件
        List<string> filePathList = new List<string>();
        DateTime currentDate = DateTime.Now;
        for (int i = 0; i < retentionDays + 10; i++)
        {
            var date = currentDate.AddDays(-i);
            var filePath = Path.Combine(logDirectory, $"Log_{date:yyyyMMdd}.json");
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
                filePathList.Add(filePath);
            }

            Assert.True(File.Exists(filePath));         // Assert that the log file exists, 確認檔案存在
        }

        // Act
        LogManager.CleanUpOldLogs(retentionDays);

        // Assert that old logs were cleaned up, 確認舊日誌已清理
        for (int i = 0; i < retentionDays + 10; i++)
        {
            var date = currentDate.AddDays(-i);
            var filePath = Path.Combine(logDirectory, $"Log_{date:yyyyMMdd}.json");

            if (i >= retentionDays)
            {
                Assert.False(File.Exists(filePath));        // Assert that the log file does not exist, 確認檔案不存在
            }
            else
            {
                Assert.True(File.Exists(filePath));         // Assert that recent logs still exist, 確認最近的日誌仍然存在
            }
        }

        // Clean up the test files, 清理測試文件
        foreach (var filePath in filePathList)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}