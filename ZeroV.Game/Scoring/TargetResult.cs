using System;

using ZeroV.Game.Elements.Particles;

namespace ZeroV.Game.Scoring;

/// <summary>
/// The result of a target judgement.
/// </summary>
[Flags]
public enum TargetResult {

    /// <summary>
    /// Too early or conditions not met. Nothing happened.
    /// </summary>
    /// <remarks>
    /// As a proposal, <see cref="None"/> here will be used to represent the vast majority of cases that do not require judgement.
    /// Including lack of objects in the queue, failure to meet judgement conditions, etc.
    /// A more reasonable solution is to use <see langword="null"/> to represent those cases above.
    /// And just use <see cref="None"/> or another name to indicate that it is too early.
    /// In this case, we may need to modify the signature of some methods in <see cref="ParticleSource"/> to allow returning <see langword="null"/> and do so by default.
    /// </remarks>
    None       = 0b0_0000,
    Normal     = 0b0_0001,
    Perfect    = 0b0_0010,
    MaxPerfect = 0b0_0100,

    /// <summary>
    /// Too late, failed.
    /// </summary>
    Miss       = 0b0_1000,
    Early      = 0b1_0000,
    NormalEarly  = Normal | Early,
    NormalLate   = Normal,
    PerfectEarly = Perfect | Early,
    PerfectLate  = Perfect,
    //ResultMask = Normal | Perfect | MaxPerfect,
}
