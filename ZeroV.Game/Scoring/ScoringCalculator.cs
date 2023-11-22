using System;
using System.Diagnostics;

namespace ZeroV.Game.Scoring;

public class ScoringCalculator {
    public const Double MAX_SCORING = 1_000_000;

    public UInt32 DisplayScoring => Convert.ToUInt32(this.Scoring);

    public Double Scoring { get; private set; } = 0;

    private UInt32 particleCount;

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

    private Double comboMultiplier => this.CurrentCombo switch {
        0 => throw new InvalidOperationException("CurrentCombo is 0"),
        1 or 2 or 3 => 1.0,
        4 or 5 => 1.1,
        6 => 1.2,
        7 => 1.3,
        8 => 1.5,
        9 => 1.7,
        _ => 2
    };

    private static Double getTargetMultiplier(TargetResult targetResult) {
        return targetResult switch {
            TargetResult.Miss => 0.0,
            TargetResult.BadEarly or TargetResult.BadLate => 0.1,
            TargetResult.NormalEarly or TargetResult.NormalLate => 0.5,
            TargetResult.Perfect => 1.0,
            _ => throw new ArgumentOutOfRangeException(nameof(targetResult), targetResult, null)
        };
    }

    public void AddTarget(TargetResult targeResult) {
        if (targeResult is TargetResult.Miss) {
            this.MissCount++;
            this.CurrentCombo = 0;
        } else {
            this.CurrentCombo++;
            this.MaxCombo = Math.Max(this.MaxCombo, this.CurrentCombo);
            switch (targeResult) {
                case TargetResult.BadEarly:
                    this.BadEarlyCount++;
                    break;

                case TargetResult.BadLate:
                    this.BadLateCount++;
                    break;

                case TargetResult.NormalEarly:
                    this.NormalEarlyCount++;
                    break;

                case TargetResult.NormalLate:
                    this.NormalLateCount++;
                    break;

                case TargetResult.Perfect:
                    this.PerfectCount++;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(targeResult), targeResult, null);
            }
            this.Scoring += this.BaseScoring * this.comboMultiplier * getTargetMultiplier(targeResult);
        }
    }

    public ScoringCalculator(UInt32 particleNumber) {
        // if (particleNumber is 0) {
        //     throw new ArgumentOutOfRangeException(nameof(particleNumber), particleNumber, "particleNumber must be greater than 0");
        // }
        this.particleCount = particleNumber;
        this.BaseScoring = this.GetBaseScoring();
    }

    public Double BaseScoring { get; private init; }

    public Double GetBaseScoring() {
        this.CurrentCombo = this.particleCount;
        Double multiplierSum = 0;
        for (var i = 0; i < this.particleCount; i++) {
            multiplierSum += this.comboMultiplier;
            this.CurrentCombo--;
        }

        Debug.Assert(this.CurrentCombo is 0);
        return MAX_SCORING / multiplierSum;
    }
}
