namespace Birdsoft.SecuIntegrator24.Tests;

using Birdsoft.SecuIntegrator24.WinUI;

using Xunit;    // Add Xunit NuGet package, 測試框架
using Moq;      // Add Moq NuGet package, 假物件框架
using System.Formats.Asn1;
using System.Windows.Forms;

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
        Assert.False(mainForm.Visible);                                     // 視窗是否隱藏

        var notifyIcon = mockUITrayManager.Object.GetNotifyIcon();
        Assert.NotNull(notifyIcon);                                         // 通知圖示是否存在
        Assert.NotNull(notifyIcon.Icon);                                    // 通知圖示是否有圖示
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
        Assert.True(mainForm.Visible);                                      // 視窗是否顯示
    }
}