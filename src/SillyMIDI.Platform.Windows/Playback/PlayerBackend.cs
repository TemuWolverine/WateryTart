namespace SillyMIDI.Platform.Windows.Playback;

public sealed partial class SwitchableAudioPlayer
{
    public enum PlayerBackend
    { 
        SimpleWasapi,
        SoundFlow 
    }
}