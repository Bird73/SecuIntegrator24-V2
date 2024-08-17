namespace Birdsoft.SecuIntegrator24.Tests;

using Birdsoft.SecuIntegrator24.WinUI;
using System.Windows.Forms;

using Xunit;
using Moq;

/// <summary>
///     Unit Test for Exit
///     退出程式測試
/// </summary>
public class TC0103_Exit
{
    /// <summary>
    ///     Test if the exit button is clicked
    ///     測試按下退出按鈕
    /// </summary>
    [Fact]
    public void ExitButtonClick()
    {
        // Arrange
        var mainForm = new MainForm(null, null);
        var exitButton = mainForm.Controls.Find("exitButton", true)[0] as Button;

        Assert.NotNull(exitButton);     // exitButton 是否存在

        mainForm.Show();                // 顯示視窗

        // Act
        exitButton.PerformClick();      // 模擬使用者按下按鈕

        // Assert
        Assert.False(mainForm.Visible);         // 視窗是否隱藏
        Assert.True(mainForm.IsDisposed);       // 視窗是否已經釋放
    }

    [Fact]
    public void ExitMenuItemClick()
    {
        // Arrange
        var mockUITrayManager = new Mock<IUITrayManager>();
        var mainForm = new MainForm(null, mockUITrayManager.Object);

        mainForm.Show();

        // simulate the context menu of the tray icon
        var notifyIcon = mainForm.GetNotifyIcon();
        Assert.NotNull(notifyIcon);     // notifyIcon 是否存在

        var contextMenu = notifyIcon.ContextMenuStrip;
        var exitMenuItem = contextMenu.Items.Find("exitMenuItem", true)[0] as ToolStripMenuItem;
        Assert.NotNull(exitMenuItem);   // exitMenuItem 是否存在

        // Act
        exitMenuItem.PerformClick();     // 模擬使用者按下選單項目

        // Assert
        Assert.False(mainForm.Visible);         // 視窗是否隱藏
        Assert.True(mainForm.IsDisposed);       // 視窗是否已經釋放
    }
}