using System;

namespace ZeroV.Game.Scoring;

[Flags]
public enum TargetResult {
    None       = 0b0_0000, // Too Early, Nothing happened
    Normal     = 0b0_0001,
    Perfect    = 0b0_0010,
    MaxPerfect = 0b0_0100,
    Miss       = 0b0_1000, // Too Late, Failed
    Early      = 0b1_0000,

    NormalEarly  = Normal | Early,
    NormalLate   = Normal,
    PerfectEarly = Perfect | Early,
    PerfectLate  = Perfect,
    //ResultMask = Normal | Perfect | MaxPerfect,
}
