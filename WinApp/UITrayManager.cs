namespace Birdsoft.SecuIntegrator24.WinUI;

/// <summary>
///     UI Tray Manager
///     UI 系統列管理器
/// </summary>
public class UITrayManager : IUITrayManager
{
    /// <summary>
    ///     Form
    ///     表單
    /// </summary>
    private Form? _form;                // initialize when DelayLoad is called

    /// <summary>
    ///     Notify Icon
    ///     通知圖示
    /// </summary>
    private NotifyIcon? _notifyIcon;    // initialize when DelayLoad is called

    /// <summary>
    ///     Delay load
    ///     延遲載入
    /// </summary>
    /// <param name="form"></param>
    public virtual void  DelayLoad(Form form)
    {
        _form = form;

        _notifyIcon = new NotifyIcon();
    }

    /// <summary>
    ///    Set the notify icon to the form
    ///    設定表單的通知圖示
    /// </summary>
    /// <returns></returns>
    public virtual NotifyIcon GetNotifyIcon()
    {
        if (_notifyIcon == null)
        {
            _notifyIcon = new NotifyIcon();
        }
        
        return _notifyIcon ;
    }

    /// <summary>
    ///     Hide the form to tray
    ///     隱藏表單到系統列
    /// </summary>
    public virtual void MinimizeToTray()
    {
        if (_form != null && _notifyIcon != null)
        {
            _form.Visible = false;
            _notifyIcon.Visible = true;
        }

    }

    /// <summary>
    ///     Restore the form from tray
    ///     從系統列還原表單
    /// </summary>
    public virtual void RestoreFromTray()
    {
         if (_form != null && _notifyIcon != null)
         {
             _form.Visible = true;
             _notifyIcon.Visible = false;
         }
    }
}