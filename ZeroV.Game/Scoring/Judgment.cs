using System;

namespace ZeroV.Game.Scoring;

public static class Judgment {

    public static TargetResult JudgeBlink(Double targetTime, Double touchTime) {
        // -: late, +: early,
        var offset = targetTime - touchTime;

        // late------------------------early
        // xxxxx-1000======0======+1000xxxxx
        return offset switch {
            // -1000~: None
            var x when x is > +1000 => TargetResult.None,
            // ~1000: Miss
            var x when x is < -1000 => TargetResult.Miss,
            // 1000~500: Bad
            var x when x is < -500 => TargetResult.NormalLate,
            var x when x is > +500 => TargetResult.NormalEarly,
            // 500~300: Normal
            var x when x is < -100 => TargetResult.PerfectLate,
            var x when x is > +100 => TargetResult.PerfectEarly,
            // 300~0: Perfect
            _ => TargetResult.MaxPerfect,
        };
    }

    public static TargetResult JudgeStroke(Double targetTime, Double touchTime) {
        // -: late, +: early,
        var offset = targetTime - touchTime;

        // late------------------------early
        // xxxxx-1000======0======+1000xxxxx
        return offset switch {
            // -1000~: None
            var x when x is > +1000 => TargetResult.None,
            // ~1000: Miss
            var x when x is < -1000 => TargetResult.Miss,
            // 1000~0: Perfect
            _ => TargetResult.MaxPerfect,
        };
    }

    public static TargetResult JudgeSlide(Double targetTime, Double touchTime) {
        // -: late, +: early,
        var offset = targetTime - touchTime;

        // late------------------------early
        // xxxxx-1000======0======+1000xxxxx
        return offset switch {
            // -1000~: None
            var x when x is > +1000 => TargetResult.None,
            // ~1000: Miss
            var x when x is < -1000 => TargetResult.Miss,
            // 1000~500: Bad
            var x when x is < -800 => TargetResult.NormalLate,
            var x when x is > +800 => TargetResult.NormalEarly,
            // 500~300: Normal
            var x when x is < -400 => TargetResult.PerfectLate,
            var x when x is > +400 => TargetResult.PerfectEarly,
            // 400~0: Perfect
            _ => TargetResult.MaxPerfect,
        };
    }
    // TODO: JudgePress
}
