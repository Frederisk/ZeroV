using System;
using System.Collections.Generic;

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.ZeroV.Scoring;

public class ZeroVHitWindows : HitWindows {

    /// <summary>
    /// Hit window ranges, expressed as (minimum at difficulty 0, average at difficulty 5, maximum at difficulty 10).
    /// </summary>
    private static readonly (HitResult Result, Double Min, Double Average, Double Max)[] ranges = {
        (HitResult.Perfect, 22.4, 19.4, 13.9),
        (HitResult.Great, 64, 49, 34),
        (HitResult.Good, 97, 82, 67),
        (HitResult.Ok, 127, 112, 97),
        (HitResult.Meh, 151, 136, 121),
        (HitResult.Miss, 188, 173, 158),
    };

    private readonly Double multiplier;
    private readonly Dictionary<HitResult, Double> windows = new();

    public ZeroVHitWindows(): this(1) { }
    public ZeroVHitWindows(Double multiplier) {
        this.multiplier = multiplier;
    }

    public override Boolean IsHitResultAllowed(HitResult result) {
        switch (result) {
            case HitResult.Perfect:
            case HitResult.Great:
            case HitResult.Good:
            case HitResult.Ok:
            case HitResult.Meh:
            case HitResult.Miss:
                return true;
        }

        return false;
    }

    public override void SetDifficulty(Double difficulty) {
        foreach (var (result, min, average, max) in ranges) {
            this.windows[result] = IBeatmapDifficultyInfo.DifficultyRange(difficulty, min, average, max) * this.multiplier;
        }
    }

    public override Double WindowFor(HitResult result) => this.windows.TryGetValue(result, out var window) ? window : 0;
}
