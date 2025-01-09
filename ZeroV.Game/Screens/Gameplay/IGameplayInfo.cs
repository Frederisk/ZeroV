using System;
using System.Collections.Generic;

using osu.Framework.Audio.Track;
using osu.Framework.Input;

using osuTK;

using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Screens.Gameplay;

public interface IGameplayInfo {
    public TrackInfo TrackInfo { get; }
    public MapInfo MapInfo { get; }
    public Track GameplayTrack { get; }
    public ScoringCalculator ScoringCalculator { get; }

    public Double ParticleFallingTime { get; }
    public Double ParticleFadingTime { get; }

    public IReadOnlyDictionary<TouchSource, Vector2> TouchPositions { get; }

    /// <summary>
    /// Occurs when a touch event is updated. Such events include press, move, and release.
    /// </summary>
    public event TouchUpdateDelegate? TouchUpdate;

    /// <summary>
    /// Encapsulates a touch update method.
    /// </summary>
    /// <param name="source">The source of the touch event.</param>
    /// <param name="isNewTouch">Whether the touch event is a new touch. This is <see langword="true"/> if the touch event is a press, and <see langword="false"/> if the touch event is a move. <see langword="null"/> if the touch event is a release.</param>
    public delegate void TouchUpdateDelegate(TouchSource source, Boolean? isNewTouch);
}
