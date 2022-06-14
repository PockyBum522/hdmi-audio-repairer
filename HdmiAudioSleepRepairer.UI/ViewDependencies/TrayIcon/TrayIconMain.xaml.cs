using HdmiAudioSleepRepairer.UI.Interfaces;

namespace HdmiAudioSleepRepairer.UI.ViewDependencies.TrayIcon;

/// <summary>
/// TrayIconMain class for setting up  
/// </summary>
public partial class TrayIconMain : ITrayIcon
{
    /// <summary>
    /// Constructor that sets up ViewModel, icon resource
    /// </summary>
    /// <param name="trayIconViewModel">Injected TrayIconViewModel</param>
    public TrayIconMain(ITrayIconViewModel trayIconViewModel)
    {
        Resources.Add("MainTrayIcon", new System.Drawing.Icon("Resources/Icons/TrayIcon.ico"));

        DataContext = trayIconViewModel;
        
        InitializeComponent();
    }
}