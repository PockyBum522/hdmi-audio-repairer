using System.IO;
using Autofac;
using Config.Net;
using HdmiAudioSleepRepairer.Core;
using HdmiAudioSleepRepairer.Core.Interfaces;
using HdmiAudioSleepRepairer.Main.Logic;
using HdmiAudioSleepRepairer.UI.Interfaces;
using HdmiAudioSleepRepairer.UI.ViewModels;
using HdmiAudioSleepRepairer.UI.Views;
using Serilog;

namespace HdmiAudioSleepRepairer.Main;

/// <summary>
/// Contains methods for building a dependency injection container with all necessary dependencies to run application
/// </summary>
public class DIContainerBuilder
{
    private readonly ContainerBuilder _builder = new ();
    private ILogger? _logger;
    private ISettingsApplicationLocal? _settingsApplicationLocal;

    /// <summary>
    /// Builds a dependency injection container with all necessary dependencies to run application
    /// </summary>
    /// <returns>Dependency injection container with all necessary dependencies to run application</returns>
    public IContainer GetBuiltContainer()
    {
        RegisterLogger();
        
        // All of these methods set up and initialize all necessary resources and dependencies,
        // then register the thing for Dependency Injection
        
        _builder.RegisterType<UnhandledExceptionHandler>().As<IUnhandledExceptionHandler>().SingleInstance();
        
        RegisterApplicationConfiguration();
        
        RegisterMainDependencies();
        
        RegisterUIDependencies();

        var container = _builder.Build();

        return container;
    }

    private void RegisterLogger()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ApplicationPaths.LogPath) ?? "");

        _logger = new LoggerConfiguration()
            .Enrich.WithProperty("Application", "SerilogTestContext")
            .MinimumLevel.Information()
            //.MinimumLevel.Debug()
            .WriteTo.File(ApplicationPaths.LogPath, rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .CreateLogger();
        
        _builder.RegisterInstance(_logger).As<ILogger>().SingleInstance();
    }
    private void RegisterApplicationConfiguration()
    {
        _settingsApplicationLocal = 
            new ConfigurationBuilder<ISettingsApplicationLocal>()
                .UseIniFile(ApplicationPaths.PathSettingsApplicationLocalIniFile)
                .Build();
        
        _builder.RegisterInstance(_settingsApplicationLocal).As<ISettingsApplicationLocal>().SingleInstance();
    }
    private void RegisterMainDependencies()
    {
    }
    
    private void RegisterUIDependencies()
    {
        _builder.RegisterType<SettingsViewModel>().AsSelf().SingleInstance();
        _builder.RegisterType<SettingsWindow>().AsSelf().SingleInstance();
        
        _builder.RegisterType<TrayIconViewModel>().As<ITrayIconViewModel>().SingleInstance();
        _builder.RegisterType<TrayIconMain>().As<ITrayIcon>().SingleInstance();
    }
}