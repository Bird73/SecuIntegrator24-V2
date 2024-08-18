namespace Birdsoft.SecuIntegrator24.WinUI;

/// <summary>
///     Main Form
///     主視窗
/// </summary>
public partial class MainForm : Form
{
    /// <summary>
    ///     UI Config Manager
    ///     UI 設定管理器
    /// </summary>
    IUIConfigManager _uiConfigManager;

    /// <summary>
    ///     UI Tray Manager
    ///     UI 系統列管理器
    /// </summary>
    IUITrayManager _uiTrayManager;

    /// <summary>
    ///     Flag to check if the program should exit
    ///     是否應該退出程式的旗標
    /// </summary>
    bool _isExit = false;

    /// <summary>
    ///     Constructor
    ///     建構函式
    /// </summary>
    /// <param name="uiConfigManager"></param>
    /// <param name="uiTrayManager"></param>
    public MainForm(IUIConfigManager? uiConfigManager, IUITrayManager? uiTrayManager)
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        Icon = resources.GetObject("$this.Icon") as Icon;

        // Set the UI Config Manager
        _uiConfigManager = uiConfigManager ?? new UIConfigManager();
        _uiConfigManager.DelayLoad(this);

        _uiTrayManager = uiTrayManager ?? new UITrayManager();
        _uiTrayManager.DelayLoad(this);

        InitializeComponent();

        // Customer Initializ Component
        CustomerInitializComponent();
    }

    private void CustomerInitializComponent()
    {
        // Set the form properties
        _uiConfigManager.SetResizable(false);
        _uiConfigManager.SetMaximizeButton(false);
        _uiConfigManager.SetMinimizeButton(false);
        
          // add notify icon context menu
        NotifyIcon notifyIcon = _uiTrayManager.GetNotifyIcon();
        if (notifyIcon != null)
        {
            notifyIcon.Icon = this.Icon;
            notifyIcon.MouseDoubleClick += (sender, e) => notifyIcon_MouseDoubleClick(sender, e);
            notifyIcon.ContextMenuStrip ??= new ContextMenuStrip();
            ToolStripItem exitMenuItem = new ToolStripMenuItem("Exit", null, (sender, e) => Exit_Click(sender, e), "exitMenuItem");
            notifyIcon.ContextMenuStrip.Items.Add(exitMenuItem);
        }
        
        this.Text = "SecuIntegrator 24";

        // Exit Button
        Button exitButton = new Button();
        exitButton.Size = new Size(120, 50);
        exitButton.Location = new Point((ClientSize.Width - exitButton.Width) / 2, ClientSize.Height - exitButton.Height - 10);
        exitButton.Name = "exitButton";
        exitButton.Text = "Exit";
        exitButton.Click += Exit_Click;
        this.Controls.Add(exitButton);
    }

    /// <summary>
    ///     Form Closing Event, not really exit the program, just minimize to tray
    ///     表單關閉事件，不是真的退出程式，只是最小化到系統列
    /// </summary>
    /// <param name="e"></param>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!_isExit)
        {
            e.Cancel = true;
            _uiTrayManager.MinimizeToTray();
        }
        base.OnFormClosing(e);
    }

    /// <summary>
    ///    Notify Icon Mouse Double Click Event, restore the form from tray
    ///    系統列圖示滑鼠雙擊事件，從系統列還原視窗
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void notifyIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        _uiTrayManager.RestoreFromTray();
    }

    private void Exit_Click(object? sender, EventArgs e)
    {
        _isExit = true;
        Application.Exit();
    }
}
