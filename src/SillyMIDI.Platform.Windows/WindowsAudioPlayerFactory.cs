using System;
using Microsoft.Extensions.Logging;
using Sendspin.SDK.Audio;
using SillyMIDI.Core.Playback;
using SillyMIDI.Platform.Windows.Playback;

namespace SillyMIDI.Platform.Windows;

public class WindowsAudioPlayerFactory(Func<IAudioPlayer> create) : IPlayerFactory
{
    private readonly Func<IAudioPlayer> _create = create ?? throw new ArgumentNullException(nameof(create));
    public WindowsAudioPlayerFactory(ILoggerFactory factory) : this(() => new SoundflowPlayer(factory)) { }

    Func<IAudioPlayer> IPlayerFactory.CreatePlayer => () =>
    {
        var p = _create();
        return p;
    };
}