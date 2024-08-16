namespace Birdsoft.SecuIntegrator24.Test;

using Birdsoft.SecuIntegrator24.WinUI;

using Xunit;    // Add Xunit NuGet package, 測試框架
using Moq;      // Add Moq NuGet package, 假物件框架

/// <summary>
///     Unit Test for UI Configuration Manager
///     UI 設定管理員單元測試
/// </summary>
public class UT0101_UIConfigurationManager
{
    /// <summary>
    ///     Test UI Configuration
    ///     測試 UI 設定
    /// </summary>
    [Fact]
    public void UT0101_TestUIConfiguration()
    {
        // Arrange
        var mainForm = new MainForm();
        var mockUIConfigurationManager = new Mock<UIConfigurationManager>(mainForm);

        // Act
        mainForm.Show();

        // Assert
        mockUIConfigurationManager.Verify(m => m.ApplyResizable(false), Times.Once);        // Resizable = false, 視窗大小不可調整
        mockUIConfigurationManager.Verify(m => m.ApplyMaximizeButton(false), Times.Once);   // MaximizeButton = false, 最大化按鈕不顯示
        mockUIConfigurationManager.Verify(m => m.ApplyMinimizeButton(false), Times.Once);   // MinimizeButton = false, 最小化按鈕不顯示
    }
}