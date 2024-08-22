namespace Birdsoft.SecuIntegrator24.SystemInfrastructureObject.LogManager;

// <summary>
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