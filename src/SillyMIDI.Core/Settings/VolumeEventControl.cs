using System;
using System.ComponentModel;

namespace SillyMIDI.Core.Settings
{
    public enum VolumeEventControl
    {
        [Description("System Volume")]
        SystemVolume,
        [Description("App Volume")]
        AppVolume
    }
}
