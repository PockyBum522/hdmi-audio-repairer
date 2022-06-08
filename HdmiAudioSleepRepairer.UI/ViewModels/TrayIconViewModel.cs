using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HdmiAudioSleepRepairer.Core;
using HdmiAudioSleepRepairer.Core.Interfaces;
using HdmiAudioSleepRepairer.Core.WindowsHelpers;
using HdmiAudioSleepRepairer.UI.Commands;
using HdmiAudioSleepRepairer.UI.Interfaces;
using HdmiAudioSleepRepairer.UI.ViewModels.ViewModelHelpers;
using HdmiAudioSleepRepairer.UI.Views;
using Serilog;

namespace HdmiAudioSleepRepairer.UI.ViewModels;

public class TrayIconViewModel : BaseViewModel, ITrayIconViewModel
{
    private readonly ILogger _logger;
    private readonly ISettingsApplicationLocal _settingsAppLocal;
    
    private readonly SettingsWindow _settingsWindow;
    
    public TrayIconViewModel(ILogger logger, SettingsViewModel settingsViewModel, ISettingsApplicationLocal settingsAppLocal)
    {
        _logger = logger;
        
        _settingsAppLocal = settingsAppLocal;

        _settingsWindow = new SettingsWindow();

        _settingsWindow.DataContext = settingsViewModel;
        
        _settingsWindow.Hide();
    }
    
    public ICommand CommandExitApplication => new SimpleCommand(() => Environment.Exit(0));
    public ICommand CommandOpenSettingsWindow => new SimpleCommand(() => _settingsWindow.Show());
    public ICommand CommandOpenLogFile => new SimpleCommand(RunLogfileInVscode);
    public ICommand CommandEnableDevice => new SimpleCommand(EnableDevice);
    public ICommand CommandDisableDevice => new SimpleCommand(DisableDevice);

    private void EnableDevice()
    {
        throw new NotImplementedException();
    }
    
    private void DisableDevice()
    {
        throw new NotImplementedException();
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