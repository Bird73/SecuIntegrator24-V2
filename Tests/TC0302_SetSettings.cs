namespace Birdsoft.SecuIntegrator24.Tests;

using Birdsoft.SecuIntegrator24.BusinessObject;
using Birdsoft.SecuIntegrator24.WinUI;
using System.Windows.Forms;
using Xunit;
using FluentAssertions;

public class TC0302_SetSettings
{
    /// <summary>
    ///     Set Seetings By Form
    ///     透過表單進行設定
    /// </summary>
    [Fact]
    public void SetSeetingsByForm()
    {
        // Arrange
        int minimumYear = 2015;
        int maximumYear = DateTime.Now.Year;
        int minimumInterval = 1;
        int maximumInterval = 180;

        // Boundary test, 邊界測試
        List<int> testYears = new List<int> { -1, 0, minimumYear - 1, minimumYear, maximumYear, maximumYear + 1 };
        List<int> testIntervals = new List<int> { -1, 0, minimumInterval - 1, minimumInterval, maximumInterval, maximumInterval + 1 };

        var mainForm = new MainForm(null, null);

        mainForm.Show();

        var initialYearComboBox = mainForm.Controls.Find("initialYearComboBox", true).FirstOrDefault() as ComboBox;
        initialYearComboBox.Should().NotBeNull();

        var connectionIntervalComboBox = mainForm.Controls.Find("connectionIntervalComboBox", true).FirstOrDefault() as ComboBox;
        connectionIntervalComboBox.Should().NotBeNull();

        var isRunAllBackgroundTasksOnStartup = mainForm.Controls.Find("isRunAllBackgroundTasksOnStartup", true).FirstOrDefault() as CheckBox;
        isRunAllBackgroundTasksOnStartup.Should().NotBeNull();

        var saveButton = mainForm.Controls.Find("saveSettingsButton", true).FirstOrDefault() as Button;
        saveButton.Should().NotBeNull();

        // Act
        var index = 0;
        foreach (var testYear in testYears)
        {
            var testInterval = testIntervals[index % testIntervals.Count];

            RunTestForYearAndInterval(testYear, testInterval, index % 2 == 0);

            index++;
        }

        void RunTestForYearAndInterval(int testYear, int testInterval, bool isRunAllBackgroundTasks)
        {
            initialYearComboBox!.Text = testYear.ToString();
            connectionIntervalComboBox!.Text = testInterval.ToString();
            isRunAllBackgroundTasksOnStartup!.Checked = isRunAllBackgroundTasks;

            saveButton!.PerformClick();

            // Check if the year is within the valid range, if not, set it to the default year. 檢查年份是否在有效範圍內，如果不是，則將其設置為默認年份。
            var expectedYear = (testYear < minimumYear || testYear > maximumYear) ? DateTime.Now.Year : testYear;
            initialYearComboBox.Text.Should().Be(expectedYear.ToString());
            SystemConfiguration.InitialYear.Should().Be(expectedYear);

            // Check if the interval is within the valid range, if not, set it to the default interval. 檢查間隔是否在有效範圍內，如果不是，則將其設置為默認間隔。
            var expectedInterval = (testInterval < minimumInterval || testInterval > maximumInterval) ? 3 : testInterval;
            connectionIntervalComboBox.Text.Should().Be(expectedInterval.ToString());
            SystemConfiguration.ConnectionInterval.Should().Be(expectedInterval);

            // Check if the AutoRun setting is saved correctly. 檢查 AutoRun 設定是否正確保存
            isRunAllBackgroundTasksOnStartup.Checked.Should().Be(isRunAllBackgroundTasks);
            SystemConfiguration.IsRunAllBackgroundTasksOnStartup.Should().Be(isRunAllBackgroundTasks);
        }
    }
}