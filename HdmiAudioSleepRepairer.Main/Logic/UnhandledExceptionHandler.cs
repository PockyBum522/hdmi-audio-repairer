using System;
using System.Threading.Tasks;
using System.Windows;
using HdmiAudioSleepRepairer.Core.Interfaces;
using Serilog;

namespace HdmiAudioSleepRepairer.Main.Logic
{
    /// <summary>
    /// Sets up unhandled exception listeners and logging for such
    ///
    /// You must call SetupExceptionHandlingEvents() to set up listeners
    /// </summary>
    public class UnhandledExceptionHandler : IUnhandledExceptionHandler
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Sets up unhandled exception listeners and logging for such
        ///
        /// You must call SetupExceptionHandlingEvents() to set up listeners
        /// </summary>
        public UnhandledExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Sets up listeners for AppDomain.CurrentDomain.UnhandledException and
        /// Application.Current.DispatcherUnhandledException
        ///
        /// Also due to the fact that puppeteer throws AggregateExceptions that can't be caught
        /// (Or at least I can't figure out how to catch them.)
        /// This will check the message on exception.InnerException?.Message to see if it is that exception
        /// and debug log it but do nothing else if it is that specific exception message.
        /// </summary>
        public bool SetupExceptionHandlingEvents()
        {
            _logger.Debug("Setting up unhandled exception handling");
            
            AppDomain.CurrentDomain.UnhandledException += async (_, e) =>
                await LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            
            TaskScheduler.UnobservedTaskException += async (_, e) =>
            {
                // Handle regular exceptions or if it was aggregate, handle the aggregateException itself
                await LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };

            try
            {
                Application.Current.DispatcherUnhandledException += async (_, e) =>
                {
                    await LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
                    e.Handled = true;
                };
            }
            catch (NullReferenceException)
            {
                _logger.Error("NullReferenceException when trying to set up UnhandledExceptionHandler, this is " +
                              "expected when unit testing but is bad at runtime");
            }

            _logger.Debug("Unhandled exception handling was set up successfully");
            return true;
        }

        private async Task LogUnhandledException(Exception exception, string source)
        {
            if (IsPuppeteerAggregateException(exception)) return;
            
            var message = $"Unhandled exception ({source})";
            
            try
            {
                var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                
                message = $"Unhandled exception in {assemblyName.Name} v{assemblyName.Version}";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception in LogUnhandledException");
            }
            finally
            {
                await Task.Delay(1000);
                
                // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                _logger.Error(exception, message);
                Log.CloseAndFlush();
                
                await Task.Delay(1000);
            }
        }

        private bool IsPuppeteerAggregateException(Exception exception)
        {
            if (exception.InnerException?.Message == "Response body is unavailable for redirect responses")
            {
                _logger.Debug("PuppeteerSharp AggregateException, this is known and is fine");
                return true;
            }
            
            return false;
        }
    }
}