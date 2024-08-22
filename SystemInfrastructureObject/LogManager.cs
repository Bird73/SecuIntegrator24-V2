namespace Birdsoft.SecuIntegrator24.SystemInfrastructureObject.LogManager;

using Microsoft.Extensions.Logging;
using System.Text.Json;

/// <summary>
///     Log manager for logging information, warnings, and errors.
///     用於記錄信息、警告和錯誤的日誌管理器。
/// </summary>
public static class LogManager
{
    /// <summary>
    ///     Log level : Information, Warning, Error
    ///     Copy from Microsoft.Extensions.Logging.LogLevel, removed Trace, Debug, Critical, None
    ///     從 Microsoft.Extensions.Logging.LogLevel 複製過來的 LogLevel, 刪除了 Trace, Debug, Critical, None
    /// </summary>
    public enum LogLevel
    {
        //
        // 摘要:
        //     Logs that contain the most detailed messages. These messages may contain sensitive
        //     application data. These messages are disabled by default and should never be
        //     enabled in a production environment.
        // Trace,
        //
        // 摘要:
        //     Logs that are used for interactive investigation during development. These logs
        //     should primarily contain information useful for debugging and have no long-term
        //     value.
        // Debug,
        //
        // 摘要:
        //     Logs that track the general flow of the application. These logs should have long-term
        //     value.
        Information = 2,
        //
        // 摘要:
        //     Logs that highlight an abnormal or unexpected event in the application flow,
        //     but do not otherwise cause the application execution to stop.
        Warning = 3,
        //
        // 摘要:
        //     Logs that highlight when the current flow of execution is stopped due to a failure.
        //     These should indicate a failure in the current activity, not an application-wide
        //     failure.
        Error = 4,
        //
        // 摘要:
        //     Logs that describe an unrecoverable application or system crash, or a catastrophic
        //     failure that requires immediate attention.
        // Critical,
        //
        // 摘要:
        //     Not used for writing log messages. Specifies that a logging category should not
        //     write any messages.
        // None
    }

    /// <summary>
    ///     Directory to store logs
    ///     用於存儲日誌的目錄
    /// </summary>
    private static readonly string LogDirectory = "Logs";

    /// <summary>
    ///     Lock object for thread safety
    ///     用於執行緒安全的鎖定物件
    /// </summary>
    private static readonly object _lock = new object();

    /// <summary>
    ///     List of log entries
    ///     日誌條目列表
    /// </summary>
    private static List<LogEntry> _logEntries = new();

    /// <summary>
    ///     Log event, raised when a log is written
    ///     日誌事件, 當日誌被寫入時觸發
    /// </summary>
    public static EventHandler<LogEntry>? Logged;

    /// <summary>
    ///     Write a log
    ///     寫入日誌
    /// </summary>
    /// <param name="level"></param>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public static void Log(LogLevel level, string message, Exception? exception = null)
    {
        var logEntry = new LogEntry
        {
            Level = level,
            Message = message,
            Exception = exception,
            TimeStamp = DateTime.Now
        };

        lock (_lock)
        {
            _logEntries.Add(logEntry);
            SaveLogToFile(logEntry);
        }

        // Raise the Logged event, 觸發 Logged 事件
        Logged?.Invoke(null, logEntry);
    }

    /// <summary>
    ///     Write a log
    ///     寫入日誌
    /// </summary>
    /// <param name="logEntry"></param>
    private static void SaveLogToFile(LogEntry logEntry)
    {
        lock (_lock)
        {
            // file path is Logs/Log_yyyyMMdd.json, 檔案路徑是 Logs/Log_yyyyMMdd.json
            var filePath = Path.Combine(LogDirectory, $"Log_{DateTime.Now:yyyyMMdd}.json");
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            // Read existing logs, 讀取現有日誌
            List<LogEntry> logEntries;
            if (File.Exists(filePath))
            {
                var existingLogs = File.ReadAllText(filePath);
                logEntries = JsonSerializer.Deserialize<List<LogEntry>>(existingLogs) ?? new List<LogEntry>();
            }
            else
            {
                logEntries = new List<LogEntry>();
            }

            logEntries.Add(logEntry);       // Add the new log entry, 添加新的日誌條目

            // Save the log entries to a file, 將日誌條目保存到檔案
            var jsonContent = JsonSerializer.Serialize(logEntries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonContent);
        }
    }

    /// <summary>
    ///     Clean up old logs
    ///     清理舊日誌
    /// </summary>
    /// <param name="RetentionDays"></param>
    public static void CleanUpOldLogs(uint retentionDays)
    {
        lock (_lock)
        {
            // Get all log files, 獲取所有日誌檔案
            var files = Directory.GetFiles(LogDirectory, "Log_*.json");
            var cutoffDate = DateTime.Now.AddDays(-retentionDays);

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                // Get the date part of the file name and compare with the cutoff date, 取得檔案名稱的日期部分並與截止日期比較
                if (DateTime.TryParseExact(fileName.Replace("Log_", ""), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var fileDate))
                {
                    if (fileDate < cutoffDate)
                    {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}
