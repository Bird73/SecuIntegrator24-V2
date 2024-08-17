namespace Birdsoft.SecuIntegrator24.WinUI;

/// <summary>
///     Main Form
///     主視窗
/// </summary>
public partial class MainForm : Form, IUIConfigManager, IUITrayManager
{
    /// <summary>
    ///     Constructor
    ///     建構函式
    /// </summary>
    /// <param name="uiConfigManager"></param>
    /// <param name="uiTrayManager"></param>
    public MainForm(IUIConfigManager? uiConfigManager, IUITrayManager? uiTrayManager)
    {
        InitializeComponent();
    }
}
