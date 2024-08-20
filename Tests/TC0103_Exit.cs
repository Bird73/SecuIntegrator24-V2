namespace Birdsoft.SecuIntegrator24.Tests;

using Birdsoft.SecuIntegrator24.WinUI;
using System.Windows.Forms;

using Xunit;    // Add Xunit NuGet package, 測試框架
using Moq;      // Add Moq NuGet package, 假物件框架

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

        mainForm.Show();                // Show the form, 顯示視窗
        
        var exitButton = mainForm.Controls.Find("exitButton", true)[0] as Button;
        Assert.NotNull(exitButton);     // check if the button exists, 檢查按鈕是否存在

        // Act
        exitButton.PerformClick();      // simulate the user clicking the button, 模擬使用者按下按鈕

        // Assert
        Assert.False(mainForm.Visible);         // check if the form is hidden, 檢查視窗是否隱藏
        Assert.True(mainForm.IsDisposed);       // check if the form is disposed, 檢查視窗是否已經釋放
    }

    /// <summary>
    ///     Test if the exit menu item is clicked
    ///     測試按下退出選單項目
    /// </summary>
    [Fact]
    public void ExitMenuItemClick()
    {
        // Arrange
        var mockUITrayManager = new Mock<UITrayManager>() { CallBase = true };
        var mainForm = new MainForm(null, mockUITrayManager.Object);

        mainForm.Show();

        // simulate the context menu of the tray icon
        var notifyIcon = mockUITrayManager.Object.GetNotifyIcon();
        Assert.NotNull(notifyIcon);     // check if the notify icon exists, 檢查通知圖示是否存在

        var contextMenu = notifyIcon.ContextMenuStrip;
        var exitMenuItem = contextMenu.Items.Find("exitMenuItem", true)[0] as ToolStripMenuItem;
        Assert.NotNull(exitMenuItem);   // check if the menu item exists, 檢查選單項目是否存在

        // Act
        exitMenuItem.PerformClick();     // simulate the user clicking the menu item, 模擬使用者按下選單項目

        // Assert
        Assert.False(mainForm.Visible);         // check if the form is hidden, 檢查視窗是否隱藏
        Assert.True(mainForm.IsDisposed);       // check if the form is disposed, 檢查視窗是否已經釋放
    }
}