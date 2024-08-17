namespace Birdsoft.SecuIntegrator24.WinUI;

/// <summary>
///     Interface for UI Configuration Manager
///     UI 設定管理員介面
/// </summary>
public interface IUIConfigManager
{
    /// <summary>
    ///     Delay load 
    ///     延遲載入
    /// </summary>
    /// <param name="form"></param>
    void DelayLoad(Form form);

    /// <summary>
    ///     Set the resizable property to the form
    ///     設定表單是否可調整大小
    /// </summary>
    /// <param name="resizable"></param>
    void SetResizable(bool resizable);

    /// <summary>
    ///     Set the maximize button property to the form
    ///     設定表單是否有最大化按鈕
    /// </summary>
    /// <param name="maximizeButton"></param>
    void SetMaximizeButton(bool maximizeButton);

    /// <summary>
    ///     Set the minimize button property to the form
    ///     設定表單是否有最小化按鈕
    /// </summary>
    /// <param name="minimizeButton"></param>
    void SetMinimizeButton(bool minimizeButton);
}