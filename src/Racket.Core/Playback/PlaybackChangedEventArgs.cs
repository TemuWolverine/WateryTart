using Sendspin.SDK.Models;
using System;

namespace Racket.Core.Playback;

/// <summary>
/// Event args for playback changes.
/// </summary>
public class PlaybackChangedEventArgs : EventArgs
{
    public GroupState GroupState { get; set; }
}