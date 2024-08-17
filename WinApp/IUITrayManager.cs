namespace Birdsoft.SecuIntegrator24.WinUI;

/// <summary>
///     UI Tray Manager
///     UI 系統匣管理員
/// </summary>
public interface IUITrayManager
{
    /// <summary>
    ///     Delay load
    ///     延遲載入
    /// </summary>
    /// <param name="form"></param>
    void DelayLoad(Form form);

    /// <summary>
    ///     Minimize the form to the system tray
    ///     最小化視窗到系統匣
    /// </summary>
    void MinimizeToTray();

    /// <summary>
    ///     Restore the form from the system tray
    ///     從系統匣還原視窗
    /// </summary>
    void RestoreFromTray();
}