using System;

using ZeroV.Game.Elements.Particles;

namespace ZeroV.Game.Scoring;

/// <summary>
/// The result of a target judgement.
/// </summary>
[Flags]
public enum TargetResult {

    /// <summary>
    /// Too early, nothing happened.
    /// </summary>
    /// <remarks>
    /// Please use <see langword="null"/> to represent the vast majority of cases that do not require judgement.
    /// Including lack of objects in the queue, failure to meet judgement conditions, etc.
    /// Just use <see cref="None"/> to indicate that it is too early.
    /// </remarks>
    None       = 0b0_0000,

    Normal     = 0b0_0001,
    Perfect    = 0b0_0010,
    MaxPerfect = 0b0_0100,

    /// <summary>
    /// Too late, failed.
    /// </summary>
    Miss  = 0b0_1000,

    Early = 0b1_0000,

    NormalEarly = Normal | Early,
    NormalLate = Normal,
    PerfectEarly = Perfect | Early,
    PerfectLate = Perfect,
}
