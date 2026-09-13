using Sendspin.SDK.Audio;
using System;

namespace Racket.Core.Playback;

public interface IPlayerFactory
{
    Func<IAudioPlayer> CreatePlayer { get; }
}
