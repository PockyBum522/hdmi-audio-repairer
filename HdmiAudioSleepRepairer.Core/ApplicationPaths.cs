namespace HdmiAudioSleepRepairer.Core;

public static class ApplicationPaths
{
    public static string GetAppRoot()
    {
        try
        {
            return Path.GetDirectoryName(Environment.ProcessPath) ?? "ERROR_GETTING_APP_PATH";
        }
        catch (IOException ex)
        {
            throw new Exception($"Can't get app root directory{Environment.NewLine}{ex.StackTrace}");
        }
    }
    
    public static string PathSettingsApplicationLocalIniFile =>
        Path.Combine(
            GetAppRoot(), 
            "Settings", 
            "settings.ini");
    
    public static string LogAppBasePath =>
        Path.Combine(
            GetAppRoot(), 
            "Logs", 
            Environment.UserName);
    
    public static string LogPath => 
        Path.Combine(
            LogAppBasePath,
            $"{ApplicationData.AppName}.log");
}