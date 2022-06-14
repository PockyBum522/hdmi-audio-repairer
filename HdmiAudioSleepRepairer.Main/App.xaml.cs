using System.Windows;
using Autofac;
using HdmiAudioSleepRepairer.Core.Interfaces;
using HdmiAudioSleepRepairer.UI.Interfaces;

namespace HdmiAudioSleepRepairer.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly DIContainerBuilder _mainBuilder = new ();
        
        /// <summary>
        /// Sets up application, app exception handling, and Tray Icon
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var container = _mainBuilder.GetBuiltContainer();
            
            using var scope = container.BeginLifetimeScope();

            // Attach unhandled exception logging
            scope.Resolve<IUnhandledExceptionHandler>().SetupExceptionHandlingEvents();
            
            // Start TrayIcon
            var unused = scope.Resolve<ITrayIcon>();
        }
    }
}
