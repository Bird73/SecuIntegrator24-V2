namespace Birdsoft.SecuIntegrator24.Tests;

using Birdsoft.SecuIntegrator24.SystemInfrastructureObject.LogManager;
using Birdsoft.SecuIntegrator24.WinUI;

using System.Windows.Forms;
using Xunit;
using FluentAssertions;

/// <summary>
///     Unit Test for WorkStatusTextBox
///     工作狀態文字框測試
/// </summary>
public class TC0104_WorkStatusTextBox
{
    /// <summary>
    ///     Test if the work status is displayed correctly
    ///     測試是否正確顯示工作狀態
    /// </summary>
    [Fact]
    public void DisplayWorkStatus()
    {
        // Arrange
        var frmMain = new MainForm(null, null);
        string testMessage = $"Test message : {Guid.NewGuid()}";     // Generate a unique message, 產生一個獨特的訊息

        var workStatusTextBox = frmMain.Controls.Find("workStatusTextBox", true)[0] as TextBox;
        workStatusTextBox.Should().NotBeNull();         // Check if the work status text box is found, 檢查是否找到工作狀態文字框

        // Act
        LogManager.Log(LogManager.LogLevel.Information, testMessage, null);     // Log the test message, 記錄測試訊息

        // Assert
        testMessage.Should().Be(workStatusTextBox!.Lines[0]);                    // Check if the message is displayed correctly, 檢查訊息是否正確顯示
    }
}