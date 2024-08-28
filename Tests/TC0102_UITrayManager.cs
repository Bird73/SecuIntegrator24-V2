namespace Birdsoft.SecuIntegrator24.Tests;

using Birdsoft.SecuIntegrator24.WinUI;

using Xunit;    // Add Xunit NuGet package, 測試框架
using Moq;      // Add Moq NuGet package, 假物件框架
using FluentAssertions;     // Add FluentAssertions NuGet package, 擴充斷言

/// <summary>
///     Unit Test for UITrayManager
///     視窗管理器測試
/// </summary>
public class TC0102_UITrayManager
{
    /// <summary>
    ///     Test if the delay load is called when the form is initialized
    ///     測試視窗初始化時是否有呼叫延遲載入
    /// </summary>
    [Fact]
    public void DelayLoadWhanFormInitialized()
    {
        // Arrange
        var mockUITrayManager = new Mock<UITrayManager>() { CallBase = true };
        var mainForm = new MainForm(null, mockUITrayManager.Object);

        // Act
        // 摸擬視窗初始化
        mainForm.Show();

        // Assert
        mockUITrayManager.Verify(m => m.DelayLoad(mainForm), Times.Once);       // DelayLoad, 延遲載入是否有被呼叫
    }

    /// <summary>
    ///     Test if the window closes to the system tray
    ///     測試視窗關閉時是否最小化到系統匣    
    /// </summary>
    [Fact]
    public void WindowClosesToSystemTray()
    {
        // Arrange
        var mockUITrayManager = new Mock<UITrayManager>() { CallBase = true };
        var mainForm = new MainForm(null, mockUITrayManager.Object);

        mainForm.Show();

        // Act
        // 摸擬使用者關閉視窗
        mainForm.Close();

        // Assert
        mockUITrayManager.Verify(m => m.MinimizeToTray(), Times.Once);      // MinimizeToTray, 最小化視窗到系統匣
        mainForm.Visible.Should().BeFalse();                                // Main form should be hidden, 主視窗應該被隱藏

        var notifyIcon = mockUITrayManager.Object.GetNotifyIcon();
        notifyIcon.Should().NotBeNull();                                    // Check if the notify icon exists, 檢查通知圖示是否存在
        notifyIcon.Icon.Should().NotBeNull();                               // Check if the notify icon has an icon, 檢查通知圖示是否有圖示
    }

    /// <summary>
    ///     Test if the window restores from the system tray
    ///     測試視窗從系統匣還原
    /// </summary>
    [Fact]
    public void WindowRestoresFromSystemTray()
    {
        // Arrange
        var mockUITrayManager = new Mock<UITrayManager>() { CallBase = true };
        var mainForm = new MainForm(null, mockUITrayManager.Object);

        mainForm.Show();
        mainForm.Close();

        // Act
        // 摸擬使用者從系統匣還原視窗
        mockUITrayManager.Object.RestoreFromTray();

        // Assert
        mockUITrayManager.Verify(m => m.RestoreFromTray(), Times.Once);     // RestoreFromTray, 從系統匣還原視窗
        mainForm.Visible.Should().BeTrue();                                  // Main form should be visible, 主視窗應該顯示
    }
}