using HdmiAudioSleepRepairer.UI.Interfaces;

namespace HdmiAudioSleepRepairer.UI.Views
{
    public partial class TrayIconMain : ITrayIcon
    {
        public TrayIconMain(ITrayIconViewModel trayIconViewModel)
        {
            Resources.Add("MainTrayIcon", new System.Drawing.Icon("Resources/Icons/TrayIcon.ico"));

            DataContext = trayIconViewModel;
            
            InitializeComponent();
        }
    }
}


      
        