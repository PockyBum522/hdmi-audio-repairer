using JetBrains.Annotations;
using Serilog;

namespace HdmiAudioSleepRepairer.Core.WindowsHelpers
{
    /// <summary>
    /// Contains operations relating to working with directories, such as delete, get oldest/newest file in a
    /// directory, etc...
    /// </summary>
    [PublicAPI]
    public class FolderHelpers
    {
        /// <summary>
        /// Get the path to the newest file matching searchPattern in a given directory, non-recursively
        /// </summary>
        /// <param name="fullFolderPath">The directory to look in</param>
        /// <param name="searchPattern">Pattern to match, defaults to *.*</param>
        /// <returns>The filename (not full path) of the newest file</returns>
        public static string GetNewestFileNameIn(string fullFolderPath, string searchPattern = "*.*")
        {
            var files = new DirectoryInfo(fullFolderPath).GetFiles(searchPattern);
            var newestFile = "";
        
            var lastModified = DateTime.MinValue;
        
            foreach (var file in files)
            {
                if (file.LastWriteTime <= lastModified) continue;
                
                // Otherwise:
                lastModified = file.LastWriteTime;
                newestFile = file.Name;
            }
        
            return newestFile;
        }
    }
}