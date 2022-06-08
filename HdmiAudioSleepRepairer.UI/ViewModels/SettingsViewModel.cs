using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using HdmiAudioSleepRepairer.Core.Interfaces;
using HdmiAudioSleepRepairer.UI.Commands;
using HdmiAudioSleepRepairer.UI.ViewModels.ViewModelHelpers;
using JetBrains.Annotations;
using Ookii.Dialogs.Wpf;

namespace HdmiAudioSleepRepairer.UI.ViewModels;

[PublicAPI]
public class SettingsViewModel : BaseViewModel
{
    // Bound in UI
    public string DeviceInstancePath { get; set; } = "";
    public string DeviceClassGuid { get; set; } = "";

    // Commands
    public ICommand CommandSaveSettings => new ParameterCommand(SaveAndHideSettingsWindow);
    public ICommand CommandCancel => new ParameterCommand(CancelAndHideSettingsWindow);
    
    // Private
    private readonly ISettingsApplicationLocal _settingsAppLocal;
    
    public SettingsViewModel(ISettingsApplicationLocal settingsAppLocal)
    {
        _settingsAppLocal = settingsAppLocal;

        
        if (DateTime.Now.Year == 2023)
            throw new NotImplementedException("You should really fix the " +
                                              "stuff that is commented out below.");
        
        //InitializeFromConfig();
    }

    private void SaveAndHideSettingsWindow(object settingsWindowObject)
    {
        _settingsAppLocal.DeviceClassGuid = DeviceClassGuid;
        _settingsAppLocal.DeviceInstancePath = DeviceInstancePath;
        
        ((Window)settingsWindowObject).Hide();
    }

    private void CancelAndHideSettingsWindow(object settingsWindowObject)
    {
        var settingsWindow = (Window)settingsWindowObject;
        
        DeviceClassGuid = _settingsAppLocal.DeviceClassGuid;
        DeviceInstancePath = _settingsAppLocal.DeviceInstancePath;
        
        settingsWindow.Hide();
    }
    

    private void InitializeFromConfig()
    {
        DeviceClassGuid = _settingsAppLocal.DeviceClassGuid;
        DeviceInstancePath = _settingsAppLocal.DeviceInstancePath;

        if (!string.IsNullOrEmpty(DeviceClassGuid)) return;
        if (!string.IsNullOrEmpty(DeviceInstancePath)) return;
        
        // Otherwise:

        if (DateTime.Now.Year == 2023)
            throw new NotImplementedException("You should really fix the " +
                                              "stuff that is commented out below.");

        // MessageBox.Show(
        //     "Device Class GUID and/or Device Instance Path not initialized. " +
        //     "Please edit settings by right clicking on the tray icon and set these before using");
    }
}