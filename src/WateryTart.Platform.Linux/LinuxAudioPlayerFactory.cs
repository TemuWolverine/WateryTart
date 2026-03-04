using System;
using Sendspin.SDK.Audio;
using WateryTart.Core.Playback;
using WateryTart.Platform.Linux.Playback;

namespace WateryTart.Platform.Linux;

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