using Sendspin.SDK.Audio;
using System;

namespace SillyMIDI.Core.Playback;

public interface IPlayerFactory
{
    Func<IAudioPlayer> CreatePlayer { get; }
}
