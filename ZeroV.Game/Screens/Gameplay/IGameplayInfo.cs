using System;
using System.Collections.Generic;

using osu.Framework.Audio.Track;
using osu.Framework.Input;

using osuTK;

using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Screens.Gameplay;

public interface IGameplayInfo {
    TrackInfo TrackInfo { get; }
    MapInfo MapInfo { get; }
    Track GameplayTrack { get; }
    ScoringCalculator ScoringCalculator { get; }

    Double ParticleFallingTime { get; }
    Double ParticleFadingTime { get; }

    IReadOnlyDictionary<TouchSource, Vector2> TouchPositions { get; }

    /// <summary>
    /// Occurs when a touch event is updated. Such events include press, move, and release.
    /// </summary>
    event TouchUpdateDelegate? TouchUpdate;

    /// <summary>
    /// Encapsulates a touch update method.
    /// </summary>
    /// <param name="source">The source of the touch event.</param>
    /// <param name="isNewTouch">Whether the touch event is a new touch. This is <see langword="true"/> if the touch event is a press, and <see langword="false"/> if the touch event is a move. <see langword="null"/> if the touch event is a release.</param>
    delegate void TouchUpdateDelegate(TouchSource source, Boolean? isNewTouch);
}
