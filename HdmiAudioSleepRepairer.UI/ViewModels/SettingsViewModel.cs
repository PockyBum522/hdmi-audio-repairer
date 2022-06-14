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

/// <summary>
/// ViewModel for settings window
/// </summary>
[PublicAPI]
public class SettingsViewModel : BaseViewModel
{
    /// <summary>
    /// Bound for getting/showing the stored device instance path
    /// </summary>
    public string DeviceInstancePath { get; set; } = "";
    /// <summary>
    /// Bound for getting/showing the stored device's class GUID
    /// </summary>
    public string DeviceClassGuid { get; set; } = "";

    /// <summary>
    /// Command to save settings and hide settings window
    /// </summary>
    public ICommand CommandSaveSettings => new ParameterCommand(SaveAndHideSettingsWindow);
    /// <summary>
    /// Command to NOT SAVE settings and hide settings window
    /// </summary>
    public ICommand CommandCancel => new ParameterCommand(CancelAndHideSettingsWindow);
    
    // Private
    private readonly ISettingsApplicationLocal _settingsAppLocal;
    
    /// <summary>
    /// Constructor for dependency injection and checking that settings are initiliazed
    /// </summary>
    /// <param name="settingsAppLocal">Injected application local settings to use</param>
    public SettingsViewModel(ISettingsApplicationLocal settingsAppLocal)
    {
        _settingsAppLocal = settingsAppLocal;
        
        InitializeFromConfig();
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
        MessageBox.Show(
            "Device Class GUID and/or Device Instance Path not initialized. " +
            "Please edit settings by right clicking on the tray icon and set these before using");
    }
}