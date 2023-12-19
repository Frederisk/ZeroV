using System;

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.ZeroV.Scoring;

public class ZeroVHitWindows : HitWindows {

    private readonly Double multiplier;

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

    protected override DifficultyRange[] GetRanges() {
        DifficultyRange[] result = base.GetRanges();

        if (this.multiplier != 1) {
            var newResult = new DifficultyRange[result.Length];
            for (var i = 0; i < result.Length; i++) {
                DifficultyRange range = result[i];
                newResult[i] = new DifficultyRange(
                    range.Result,
                    range.Min * this.multiplier,
                    range.Average * this.multiplier,
                    range.Max * this.multiplier);
            }

            result = newResult;
        }

        return result;
    }
}
