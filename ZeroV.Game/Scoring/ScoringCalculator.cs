using System;
using System.Diagnostics;

namespace ZeroV.Game.Scoring;

public class ScoringCalculator {

    /// <summary>
    /// The total number of particles in a beatmap.
    /// </summary>
    public UInt32 ParticleCount { get; init; }

    /// <summary>
    /// The maximum scoring value.
    /// </summary>
    public const Double MAX_SCORING = 1_000_000;

    public UInt32 DisplayScoring => Convert.ToUInt32(this.Scoring);
    public Double Scoring { get; private set; } = 0;
    public Double BaseScoring { get; private init; }

    public UInt32 JudgedCount => this.MissCount + this.NormalCount + this.PerfectCount + this.MaxPerfectCount;
    public UInt32 MissCount { get; private set; } = 0;

    public UInt32 NormalCount => this.NormalEarlyCount + this.NormalLateCount;
    public UInt32 NormalEarlyCount { get; private set; } = 0;
    public UInt32 NormalLateCount { get; private set; } = 0;

    public UInt32 PerfectCount => this.PerfectEarlyCount + this.PerfectLateCount;
    public UInt32 PerfectEarlyCount { get; private set; } = 0;
    public UInt32 PerfectLateCount { get; private set; } = 0;

    public UInt32 MaxPerfectCount { get; private set; } = 0;

    public UInt32 EarlyCount => this.NormalEarlyCount + this.PerfectEarlyCount;
    public UInt32 LateCount => this.NormalLateCount + this.PerfectLateCount;

    public UInt32 MaxCombo { get; private set; } = 0;
    public UInt32 CurrentCombo { get; private set; } = 0;

    public TargetResult CurrentTarget { get; private set; }

    public Boolean IsFullCombo => this.CurrentCombo == this.JudgedCount;
    public Boolean IsAllPerfect => (this.PerfectCount + this.MaxPerfectCount) == this.JudgedCount;

    public Boolean IsAllDone => this.JudgedCount == this.ParticleCount;

    public Action? ScoringChanged;

    private Double comboMultiplier => this.CurrentCombo switch {
        0 => 0, // throw new InvalidOperationException("CurrentCombo is 0"),
        1 or 2 or 3 => 1.0,
        4 or 5 => 1.1,
        6 => 1.2,
        7 => 1.3,
        8 => 1.5,
        9 => 1.7,
        _ => 2
    };

    public ScoringCalculator(UInt32 particleNumber) {
        // if (particleNumber is 0) {
        //     throw new ArgumentOutOfRangeException(nameof(particleNumber), particleNumber, "ParticleNumber is 0");
        // }
        Debug.Assert(particleNumber is not 0);

        this.ParticleCount = particleNumber;
        this.BaseScoring = this.getBaseScoring();
    }

    private Double getBaseScoring() {
        this.CurrentCombo = this.ParticleCount;
        Double multiplierSum = 0;
        for (var i = 0; i < this.ParticleCount; i++) {
            multiplierSum += this.comboMultiplier;
            this.CurrentCombo--;
        }

        Debug.Assert(this.CurrentCombo is 0);
        return MAX_SCORING / multiplierSum;
    }

    public void AddTarget(TargetResult targetResult) {
        if (targetResult is TargetResult.None) {
            return;
        }

        this.CurrentCombo++;
        Double targetMultiplier;
        switch (targetResult) {
            case TargetResult.Miss:
                this.MissCount++;
                targetMultiplier = 0.0;
                this.CurrentCombo = 0;
                break;

            case TargetResult.NormalEarly:
                this.NormalEarlyCount++;
                targetMultiplier = 0.1;
                break;

            case TargetResult.NormalLate:
                this.NormalLateCount++;
                targetMultiplier = 0.1;
                break;

            case TargetResult.PerfectEarly:
                this.PerfectEarlyCount++;
                targetMultiplier = 0.5;
                break;

            case TargetResult.PerfectLate:
                this.PerfectLateCount++;
                targetMultiplier = 0.5;
                break;

            case TargetResult.MaxPerfect:
                this.MaxPerfectCount++;
                targetMultiplier = 1.0;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(targetResult), targetResult, null);
        }
        this.MaxCombo = Math.Max(this.MaxCombo, this.CurrentCombo);
        // this.Scoring += this.BaseScoring * this.comboMultiplier * getTargetMultiplier(targeResult);
        this.Scoring += this.BaseScoring * this.comboMultiplier * targetMultiplier;
        this.CurrentTarget = targetResult;
        this.ScoringChanged?.Invoke();
    }

    // private static Double getTargetMultiplier(TargetResult targetResult) {
    //     return targetResult switch {
    //         TargetResult.Miss => 0.0,
    //         TargetResult.NormalEarly or TargetResult.NormalLate => 0.1,
    //         TargetResult.PerfectEarly or TargetResult.PerfectLate => 0.5,
    //         TargetResult.MaxPerfect => 1.0,
    //         _ => throw new ArgumentOutOfRangeException(nameof(targetResult), targetResult, null)
    //     };
    // }
}
