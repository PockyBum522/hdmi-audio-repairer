using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HdmiAudioSleepRepairer.Core;
using HdmiAudioSleepRepairer.Core.Interfaces;
using HdmiAudioSleepRepairer.Core.Logic;
using HdmiAudioSleepRepairer.Core.WindowsHelpers;
using HdmiAudioSleepRepairer.UI.Interfaces;
using HdmiAudioSleepRepairer.UI.ViewDependencies.SettingsWindow;
using HdmiAudioSleepRepairer.UI.ViewDependencies.ViewModelHelpers;
using HdmiAudioSleepRepairer.UI.ViewDependencies.ViewModelHelpers.Commands;
using Serilog;

namespace HdmiAudioSleepRepairer.UI.ViewDependencies.TrayIcon;

/// <summary>
/// ViewModel for Tray Icon
/// </summary>
public class TrayIconViewModel : BaseViewModel, ITrayIconViewModel
{
    private readonly ILogger _logger;
    private readonly ISettingsApplicationLocal _settingsAppLocal;
    
    private readonly SettingsWindow.SettingsWindow _settingsWindow;
    
    /// <summary>
    /// Constructor for dependency injection
    /// </summary>
    /// <param name="logger">Injected logger to use</param>
    /// <param name="settingsViewModel">Injected settings window viewmodel to use</param>
    /// <param name="settingsAppLocal">Injected application local settings to use</param>
    public TrayIconViewModel(ILogger logger, SettingsViewModel settingsViewModel, ISettingsApplicationLocal settingsAppLocal)
    {
        _logger = logger;
        
        _logger.Information("Initializing Tray Icon View");
        
        _settingsAppLocal = settingsAppLocal;

        _settingsWindow = new SettingsWindow.SettingsWindow();

        _settingsWindow.DataContext = settingsViewModel;
        
        _settingsWindow.Hide();
        
        _logger.Information("Hid settings window, tray icon init finished");
    }
    
    /// <summary>
    /// Command to exit the application completely
    /// </summary>
    public ICommand CommandExitApplication => new SimpleCommand(() => Environment.Exit(0));
    /// <summary>
    /// Command to show the settings window
    /// </summary>
    public ICommand CommandOpenSettingsWindow => new SimpleCommand(() => _settingsWindow.Show());
    /// <summary>
    /// Command to open the log file in VSCode
    /// </summary>
    public ICommand CommandOpenLogFile => new SimpleCommand(RunLogfileInVscode);
    /// <summary>
    /// Command to enable the hardware device manually
    /// </summary>
    public ICommand CommandEnableDevice => new SimpleCommand(EnableDevice);
    /// <summary>
    /// Command to disable the hardware device manually
    /// </summary>
    public ICommand CommandDisableDevice => new SimpleCommand(DisableDevice);
    
    /// <summary>
    /// Command to disable then enable the hardware device manually
    /// </summary>
    // ReSharper disable once AsyncVoidLambda because resharper doesn't know what it's talking about
    public ICommand CommandRestartDevice => new SimpleCommand(async () => await RestartDevice());

    private async Task RestartDevice()
    {
        DisableDevice();
        
        await Task.Delay(5000);
        
        EnableDevice();
    }

    private void EnableDevice()
    {
        var deviceCategoryGuid = new Guid(_settingsAppLocal.DeviceClassGuid);

        var instancePath = _settingsAppLocal.DeviceInstancePath;

        DeviceHelper.SetDeviceEnabled(deviceCategoryGuid, instancePath, true);
        
        _logger.Information("Enabled device at path: {DevicePath}",
            _settingsAppLocal.DeviceInstancePath);
    }
    
    private void DisableDevice()
    {
        var deviceCategoryGuid = new Guid(_settingsAppLocal.DeviceClassGuid);
        
        var instancePath = _settingsAppLocal.DeviceInstancePath;
        
        DeviceHelper.SetDeviceEnabled(deviceCategoryGuid, instancePath, false);
        
        _logger.Information("Disabled device at path: {DevicePath}",
            _settingsAppLocal.DeviceInstancePath);
    }
    
    private void RunLogfileInVscode()
    {
        // This is just to parent the messageboxes
        var temporaryWindow = new Window()
        {
            Visibility = Visibility.Hidden,
            // Just hiding the window is not sufficient, as it still temporarily pops up the first time.
            // Therefore, make it transparent.
            AllowsTransparency = true,
            Background = System.Windows.Media.Brushes.Transparent,
            WindowStyle = WindowStyle.None,
            ShowInTaskbar = false
        };

        temporaryWindow.Show();
        
        var vscodeProcess = new Process();

        var newestFileName = FolderHelpers.GetNewestFileNameIn(ApplicationPaths.LogAppBasePath);
        
        var vscodePath = @"C:\Program Files\Microsoft VS Code\Code.exe";

        if (!File.Exists(vscodePath))
        {
            MessageBox.Show(temporaryWindow, 
                $"Please install Visual Studio Code to {vscodePath} before trying to open log");
            
            return;
        }

        vscodeProcess.StartInfo.FileName = vscodePath;
        vscodeProcess.StartInfo.Arguments = "\"" + Path.Combine(ApplicationPaths.LogAppBasePath, newestFileName) + "\"";
        
        vscodeProcess.Start();
    }
}