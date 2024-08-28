namespace Birdsoft.SecuIntegrator24.WinUI;

using Birdsoft.SecuIntegrator24.BusinessObject;
using Birdsoft.SecuIntegrator24.SystemInfrastructureObject.LogManager;


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

    /// <summary>
    ///     Customer Initializ Component
    ///     自定義初始化元件
    /// </summary>
    private void CustomerInitializComponent()
    {
        // Set the form properties, 設定視窗屬性
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

        // Set the form size, 設定視窗大小
        this.Size = new Size(800, 600);

        // Exit Button
        Button exitButton = new Button();
        exitButton.Size = new Size(120, 50);
        exitButton.Location = new Point((ClientSize.Width - exitButton.Width) / 2, ClientSize.Height - exitButton.Height - 10);
        exitButton.Name = "exitButton";
        exitButton.Text = "Exit";
        exitButton.Click += Exit_Click;
        this.Controls.Add(exitButton);

        // Work Status TextBox
        TextBox workStatusTextBox = new TextBox();
        workStatusTextBox.Name = "workStatusTextBox";

        // workStatusTextBox is located at 1/2 height of the screen to the top of exitButton, workStatusTextBox 位於螢幕高度的 1/2 到 exitButton 的頂部
        workStatusTextBox.Size = new Size(ClientSize.Width - 20, this.ClientSize.Height / 2 - exitButton.Height / 2 - 20);
        workStatusTextBox.Location = new Point(10, (exitButton.Top - workStatusTextBox.Height) / 2);
        workStatusTextBox.Multiline = true;
        workStatusTextBox.ScrollBars = ScrollBars.Vertical;
        workStatusTextBox.ReadOnly = true;
        LogManager.Logged += (sender, e) =>
        {
            // Append the log message to the workStatusTextBox on the top, 將日誌訊息附加到 workStatusTextBox 的頂部
            workStatusTextBox.Text = e.Message + Environment.NewLine + workStatusTextBox.Text;
        };
        this.Controls.Add(workStatusTextBox);

        // Set the event handler, 設定事件處理程序
        this.Load += (sender, e) => mainForm_OnLoad(e);                 // Main Form On Load Event, 主視窗載入事件
        this.FormClosing += (sender, e) => mainForm_OnClosing(e);       // Form Closing Event, 表單關閉事件

        SystemConfiguration.LoadSettings();
    }

    /// <summary>
    ///     Form Closing Event, not really exit the program, just minimize to tray
    ///     表單關閉事件，不是真的退出程式，只是最小化到系統列
    /// </summary>
    /// <param name="e"></param>
    private void mainForm_OnClosing(FormClosingEventArgs e)
    {
        if (!_isExit)
        {
            e.Cancel = true;
            _uiTrayManager.MinimizeToTray();
        }
    }

    /// <summary>
    ///     Main Form On Load Event
    ///     主視窗載入事件
    /// </summary>
    /// <param name="e"></param>
    private void mainForm_OnLoad(EventArgs e)
    {
        // Set the form to the center of the screen, 設定視窗置中
        this.CenterToScreen();

        LogManager.Log(LogManager.LogLevel.Information, "Program loaded.");
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

    /// <summary>
    ///     Exit Button Click Event
    ///     退出按鈕點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Exit_Click(object? sender, EventArgs e)
    {
        // Message dialog to confirm exit, 確認退出的訊息對話框
        DialogResult result = MessageBox.Show("是否確定退出程式？", "退出", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            LogManager.Log(LogManager.LogLevel.Information, "Exit the program.");

            _isExit = true;
            Application.Exit();
        }
    }
}
