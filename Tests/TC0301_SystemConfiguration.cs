namespace Birdsoft.SecuIntegrator24.Tests;

using Birdsoft.SecuIntegrator24.BusinessObject;
using Xunit;
using FluentAssertions;

public class TC0301_SystemConfiguration
{
    /// <summary>
    ///     Test if the settings are saved within the expected range.
    ///     測試是否在預期範圍內保存設置。
    /// </summary>
    [Fact]
    public void SaveSettingsWithinExpectedRange()
    {
        int minimumYear = 2015;
        int maximumYear = DateTime.Now.Year;
        int defaultYear = DateTime.Now.Year;
        int minimumInterval = 1;
        int maximumInterval = 180;
        int defaultInterval = 3;

        // Boundary test, 邊界測試
        List<int> testYears = new List<int> { -1, 0, minimumYear - 1, minimumYear, maximumYear, maximumYear + 1 };
        List<int> testIntervals = new List<int> { -1, 0, minimumInterval - 1, minimumInterval, maximumInterval, maximumInterval + 1 };
        List<bool> runAllBackgroundTasksOnStartup = new List<bool> { true, false };

        int index = 0;
        // Arrange
        foreach (int testYear in testYears)
        {
            var testInterval = testIntervals[index % testIntervals.Count];
            var testAutoRun = runAllBackgroundTasksOnStartup[index % runAllBackgroundTasksOnStartup.Count];

            ++index;

            // Act
            SystemConfiguration.InitialYear = testYear;
            SystemConfiguration.ConnectionInterval = testInterval;
            SystemConfiguration.IsRunAllBackgroundTasksOnStartup = testAutoRun;
            SystemConfiguration.SaveSettings();

            // Assert
            // Check if the year is within the valid range, if not, set it to the default year. 檢查年份是否在有效範圍內，如果不是，則將其設置為默認年份。
            int expectedYear = (testYear < minimumYear || testYear > maximumYear) ? defaultYear : testYear;
            SystemConfiguration.InitialYear.Should().Be(expectedYear);        

            // Check if the interval is within the valid range, if not, set it to the default interval. 檢查間隔是否在有效範圍內，如果不是，則將其設置為默認間隔。
            int expectedInterval = (testInterval < minimumInterval || testInterval > maximumInterval) ? defaultInterval : testInterval;
            SystemConfiguration.ConnectionInterval.Should().Be(expectedInterval);

            // Check if the AutoRun setting is saved correctly. 檢查 AutoRun 設定是否正確保存
            SystemConfiguration.IsRunAllBackgroundTasksOnStartup.Should().Be(testAutoRun);
        }       
    }

    /// <summary>
    ///     Test if the settings are loaded correctly.
    ///     測試是否正確加載設置。
    /// </summary>
    [Fact]
    public void LoadSettings()
    {
        // Arrange
        int minimumYear = 2015;
        int maximumYear = DateTime.Now.Year;
        int minimumInterval = 1;
        int maximumInterval = 180;

        // Act
        SystemConfiguration.LoadSettings();

        // Assert
        SystemConfiguration.InitialYear.Should().BeInRange(minimumYear, maximumYear);
        SystemConfiguration.ConnectionInterval.Should().BeInRange(minimumInterval, maximumInterval);
    }
}