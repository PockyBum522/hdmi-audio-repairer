namespace HdmiAudioSleepRepairer.Core.Interfaces;

/// <summary>
/// Settings for the application that are stored locally on end user's computer 
/// </summary>
public interface ISettingsApplicationLocal
{
    /// <summary>
    /// Store of the Class GUID for the audio device to work on
    /// </summary>
    string DeviceClassGuid { get; set; }
    
    /// <summary>
    /// Store of the device manager device instance path for the audio device to work on
    /// </summary>
    string DeviceInstancePath { get; set; }
}