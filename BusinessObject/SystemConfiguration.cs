namespace Birdsoft.SecuIntegrator24.BusinessObject;


using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text.Json;

public static class SystemConfiguration
{
    private static int _initialYear;
    private static int _connectionInterval;

    /// <summary>
    ///     Initial year of the data.
    ///     資料的初始年份。
    /// </summary>
    public static int InitialYear 
    { 
        get => _initialYear;
        set => _initialYear = (value < 2015 || value > DateTime.Now.Year) ? DateTime.Now.Year : value;
    }

    /// <summary>
    ///     Connection interval in seconds.
    ///     連接間隔（秒）。
    /// </summary>
    public static int ConnectionInterval
    { 
        get => _connectionInterval;
        set => _connectionInterval = (value < 1 || value > 180) ? 3 : value;
    }

    /// <summary>
    ///     Run all background tasks once after the program starts.
    ///     在程式啟動後自動執行一次所有背景任務。
    /// </summary>
    public static bool IsRunAllBackgroundTasksOnStartup { get; set; }

    /// <summary>
    ///     Settings file name.
    ///     設置檔名稱。
    /// </summary>
    private static readonly string SettingsFileName = "Settings.json";

    private class AppConfig
    {
        public Settings Settings { get; set; } = new Settings();
    }   

    private class Settings
    {
        public int InitialYear { get; set; } = DateTime.Now.Year;
        public int ConnectionInterval { get; set; } = 3;
        public bool RunAllBackgroundTasksOnStartup { get; set; } = true;
    }

    public static void LoadSettings()
    {
        if (!File.Exists(SettingsFileName))
        {
            // Create a new settings file with default values. 使用默認值創建新的設置檔
            var defaultSettings = new AppConfig();
            var json = JsonSerializer.Serialize(defaultSettings);
            File.WriteAllText(SettingsFileName, json);
        }

        // Load settings from file, 讀取設置檔
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(SettingsFileName, optional: false, reloadOnChange: true);

        var configuration = builder.Build();

        var appConfig = configuration.Get<AppConfig>();

        if (appConfig == null)
        {
            InitialYear = DateTime.Now.Year;
            ConnectionInterval = 3;
            IsRunAllBackgroundTasksOnStartup = true;
            return;
        }
        else
        {
            var initialYear = appConfig.Settings.InitialYear;
            var connectionInterval = appConfig.Settings.ConnectionInterval;

            InitialYear = (initialYear < 2015 || initialYear > DateTime.Now.Year) ? DateTime.Now.Year : initialYear;
            ConnectionInterval = (connectionInterval < 1 || connectionInterval > 180) ? 3 : connectionInterval;
            IsRunAllBackgroundTasksOnStartup = appConfig.Settings.RunAllBackgroundTasksOnStartup;
        }
    }

    public static void SaveSettings()
    {
        // Save settings to file
    }
}