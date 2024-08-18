namespace Birdsoft.SecuIntegrator24.WinUI;

/// <summary>
///     UI Config Manager
///     UI 設定管理器
/// </summary>
public class UIConfigManager : IUIConfigManager
{
    /// <summary>
    ///     Form
    ///     表單
    /// </summary>
    private Form? _form;                // initialize when DelayLoad is called

    /// <summary>
    ///     Delay load
    ///     延遲載入
    /// </summary>
    /// <param name="form"></param>
    public virtual void DelayLoad(Form form)
    {
        _form = form;
    }

    /// <summary>
    ///     Set the resizable property to the form
    ///     設定表單是否可調整大小
    /// </summary>
    /// <param name="resizable"></param>
    public virtual void SetResizable(bool resizable)
    {
        if (_form != null)
        {
            _form.FormBorderStyle = resizable ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;
        }
    }

    /// <summary>
    ///     Set the maximize button property to the form
    ///     設定表單是否有最大化按鈕
    /// </summary>
    /// <param name="maximizeButton"></param>
    public virtual void SetMaximizeButton(bool maximizeButton)
    {
        if (_form != null)
        {
            _form.MaximizeBox = maximizeButton;
        }
    }

    /// <summary>
    ///     Set the minimize button property to the form
    ///     設定表單是否有最小化按鈕
    /// </summary>
    /// <param name="minimizeButton"></param>
    public virtual void SetMinimizeButton(bool minimizeButton)
    {
        if (_form != null)
        {
            _form.MinimizeBox = minimizeButton;
        }
    }
}