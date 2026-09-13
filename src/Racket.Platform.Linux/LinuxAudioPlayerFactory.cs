using System;
using Sendspin.SDK.Audio;
using Racket.Core.Playback;
using Racket.Platform.Linux.Playback;

namespace Racket.Platform.Linux;

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