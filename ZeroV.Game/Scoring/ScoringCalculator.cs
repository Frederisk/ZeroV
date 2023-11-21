using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroV.Game.Scoring;
internal sealed class ScoringCalculator(UInt32 particleNumber) {
    public static Double MAX_SCORING = 1_000_000;

    public UInt32 DisplayScoring => Convert.ToUInt32(this.Scoring);

    public Double Scoring { get; private set; } = 0;

    private UInt32 particleCount = particleNumber;

    public UInt32 MissCount { get; private set; } = 0;
    public UInt32 BadEarlyCount { get; private set; } = 0;
    public UInt32 BadLateCount { get; private set; } = 0;
    public UInt32 NormalEarlyCount { get; private set; } = 0;
    public UInt32 NormalLateCount { get; private set; } = 0;
    public UInt32 PerfectCount { get; private set; } = 0;
    public UInt32 BadCount => this.BadEarlyCount + this.BadLateCount;
    public UInt32 NormalCount => this.NormalEarlyCount + this.NormalLateCount;
    public UInt32 EarlyCount => this.BadEarlyCount + this.NormalEarlyCount;
    public UInt32 LateCount => this.BadLateCount + this.NormalLateCount;

    public UInt32 MaxCombo { get; private set; } = 0;
    public UInt32 CurrentCombo { get; private set; } = 0;

    private Double ComboMultiplier => this.CurrentCombo switch {
        0 => throw new InvalidOperationException("CurrentCombo is 0"),
        1 or 2 or 3 => 1.0,
        4 or 5 => 1.1,
        6 => 1.2,
        7 => 1.3,
        8 => 1.5,
        9 => 1.7,
        _ => 2
    };

}
