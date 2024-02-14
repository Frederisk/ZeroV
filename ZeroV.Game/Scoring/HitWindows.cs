using System;
using System.Collections.Generic;

namespace ZeroV.Game.Scoring;
public abstract class HitWindows {
    public TargetResult ResultFor(Double timeOffset) {
        TargetResult isEarly = Double.IsNegative(timeOffset) ? TargetResult.Early : 0;

        var abs = Double.Abs(timeOffset);
        foreach ((Double value, TargetResult result) in this.Windows) {
            if (abs <= value) {
                return result | isEarly;
            }
        }

        return TargetResult.Miss | isEarly;
    }
    public abstract IEnumerable<(Double Value, TargetResult Result)> Windows { get; }
}
