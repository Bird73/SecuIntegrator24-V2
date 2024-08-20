namespace Birdsoft.SecuIntegrator24.SystemInfrastructureObject;

using Microsoft.Extensions.Logging;

/// <summary>
///     Log entry
///     日誌項目
/// </summary>
public class LogEntry
{
    /// <summary>
    ///     Log level : Information, Warning, Error
    ///     日誌等級 : 資訊, 警告, 錯誤
    /// </summary>
    public LogManager.LogLevel Level { get; set; } = LogManager.LogLevel.Information;

    /// <summary>
    ///     Log message
    ///     日誌訊息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///     Exception
    ///     例外
    /// </summary>
    public Exception? Exception { get; set; } = null;

    /// <summary>
    ///     Time stamp
    ///     時間戳記
    /// </summary>
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}

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
        // Log the message
    }

    /// <summary>
    ///     Clean up old logs
    ///     清理舊日誌
    /// </summary>
    /// <param name="RetentionDays"></param>
    public static void CleanUpOldLogs(uint RetentionDays)
    {

    }
}
