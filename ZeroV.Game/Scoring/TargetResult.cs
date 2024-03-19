using System;

namespace ZeroV.Game.Scoring;

[Flags]
public enum TargetResult {
    None = 0b0000, // Too Early, Nothing happened
    Normal = 0b0001,
    Perfect = 0b0010,
    MaxPerfect = 0b0100,
    Miss = 0b1000, // Too Late, Failed

    Early      = 0b1_0000,

    NormalEarly = Normal | Early,
    NormalLate = Normal,
    PerfectEarly = Perfect | Early,
    PerfectLate = Perfect,
    //ResultMask = Normal | Perfect | MaxPerfect,
}
