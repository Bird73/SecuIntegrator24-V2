namespace Birdsoft.SecuIntegrator24.Tests;

using Birdsoft.SecuIntegrator24.WinUI;

using Xunit;    // Add Xunit NuGet package, 測試框架
using Moq;      // Add Moq NuGet package, 假物件框架
using FluentAssertions;     // Add FluentAssertions NuGet package, 擴充斷言
using System.Windows.Forms;

/// <summary>
///     Unit Test for UI Configuration Manager
///     UI 設定管理員單元測試
/// </summary>
public class TC0101_UIConfigManager
{
    /// <summary>
    ///     Test if the delay load is called when the form is initialized
    ///     測試視窗初始化時是否有呼叫延遲載入
    /// </summary>
    /// <param name="form"></param>
    [Fact]
    public void DelayLoad()
    {
        // Arrange
        var mockUIConfigManager = new Mock<UIConfigManager>() { CallBase = true };
        var mainForm = new MainForm(mockUIConfigManager.Object, null);

        // Act
        mainForm.Show();

        // Assert
        mockUIConfigManager.Verify(m => m.DelayLoad(mainForm), Times.Once);         // DelayLoad, 延遲載入是否有被呼叫
    }

    /// <summary>
    ///     Test UI Configuration
    ///     測試 UI 設定
    /// </summary>
    [Fact]
    public void UIConfiguration()
    {
        // Arrange
        var mockUIConfigManager = new Mock<UIConfigManager>() { CallBase = true };
        var mainForm = new MainForm(mockUIConfigManager.Object, null);

        // Act
        mainForm.Show();

        // Assert
        mockUIConfigManager.Verify(m => m.DelayLoad(mainForm), Times.Once);         // DelayLoad, 延遲載入是否有被呼叫
        
        mainForm.FormBorderStyle.Should().Be(FormBorderStyle.FixedSingle);          // FormBorderStyle, 固定視窗邊框
        mainForm.MaximizeBox.Should().BeFalse();                                    // MaximizeBox, 不顯示最大化按鈕
        mainForm.MinimizeBox.Should().BeFalse();                                    // MinimizeBox, 不顯示最小化按鈕
        
        mainForm.Text.Should().Be("SecuIntegrator 24");                             // MainForm.Text, 視窗標題
    }
}