using System;

namespace ZeroV.Game.Scoring;

[Flags]
public enum TargetResult {
    Miss = 0b0000,
    Normal = 0b0001,
    Perfect = 0b0010,
    MaxPerfect = 0b0100,

    Early = 0b1000,

    NormalEarly = Normal | Early,
    NormalLate = Normal,
    PerfectEarly = Perfect | Early,
    PerfectLate = Perfect,

    ResultMask = Normal | Perfect | MaxPerfect,
}
