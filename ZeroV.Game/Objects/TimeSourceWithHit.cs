using System;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Objects;

public abstract class TimeSourceWithHit : TimeSource {

    public TargetResult ResultFor(Double timeOffset) {
        TargetResult isEarly = Double.IsNegative(timeOffset) ? TargetResult.Early : 0;
        TargetResult result = this.ResultForInternal(timeOffset);

        return result | isEarly;
    }

    protected virtual TargetResult ResultForInternal(Double timeOffset) =>
        Double.Abs(timeOffset) switch {
            <= 100.0 => TargetResult.MaxPerfect,
            <= 500.0 => TargetResult.Perfect,
            <= 1000.0 => TargetResult.Normal,
            _ => TargetResult.Miss,
        };
}
