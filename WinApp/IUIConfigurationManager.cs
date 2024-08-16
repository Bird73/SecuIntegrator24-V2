namespace Birdsoft.SecuIntegrator24.WinUI;

/// <summary>
///     Interface for UI Configuration Manager
///     UI 設定管理員介面
/// </summary>
public interface IUIConfigurationManager
{
    /// <summary>
    ///     UI Configuration Manager Constructor
    ///     UI 設定管理員建構函數
    /// </summary>
    /// <param name="form"></param>
    void IUIConfigurationManager(Form form);

    /// <summary>
    ///     Apply the resizable property to the form
    ///     設定表單是否可調整大小
    /// </summary>
    /// <param name="resizable"></param>
    void ApplyResizable(bool resizable);

    /// <summary>
    ///     Apply the maximize button property to the form
    ///     設定表單是否有最大化按鈕
    /// </summary>
    /// <param name="maximizeButton"></param>
    void ApplyMaximizeButton(bool maximizeButton);

    /// <summary>
    ///     Apply the minimize button property to the form
    ///     設定表單是否有最小化按鈕
    /// </summary>
    /// <param name="minimizeButton"></param>
    void ApplyMinimizeButton(bool minimizeButton);
}