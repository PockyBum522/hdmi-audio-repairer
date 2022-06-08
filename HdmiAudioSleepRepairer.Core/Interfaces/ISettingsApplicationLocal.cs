namespace HdmiAudioSleepRepairer.Core.Interfaces;

public interface ISettingsApplicationLocal
{
    string DeviceClassGuid { get; set; }
    string DeviceInstancePath { get; set; }
}