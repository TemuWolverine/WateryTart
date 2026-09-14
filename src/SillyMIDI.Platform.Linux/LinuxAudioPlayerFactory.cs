using System;
using Sendspin.SDK.Audio;
using SillyMIDI.Core.Playback;
using SillyMIDI.Platform.Linux.Playback;

namespace SillyMIDI.Platform.Linux;

public class LinuxAudioPlayerFactory : IPlayerFactory
{
    Func<IAudioPlayer> IPlayerFactory.CreatePlayer
    {
        get
        {
            static IAudioPlayer PlayerFactory() => new OpenALAudioPlayer();
            return PlayerFactory;
        }
    }
}