namespace HdmiAudioSleepRepairer.Core.Interfaces
{
    /// <summary>
    /// Handles all general unhandled exceptions, mainly for logging purposes
    /// </summary>
    public interface IUnhandledExceptionHandler
    {
        /// <summary>
        /// Sets up listeners for AppDomain.CurrentDomain.UnhandledException and
        /// Application.Current.DispatcherUnhandledException
        ///
        /// Also due to the fact that puppeteer throws AggregateExceptions that can't be caught
        /// (Or at least I can't figure out how to catch them.)
        /// This will check the message on exception.InnerException?.Message to see if it is that exception
        /// and debug log it but do nothing else if it is that specific exception message.
        /// </summary>
        /// <returns>true if all event listeners were added successfully</returns>
        bool SetupExceptionHandlingEvents();
    }
}