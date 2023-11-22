using System;

namespace ZeroV.Game.Scoring;

public static class Judgment {

    public static TargetResult JudgeBlink(Double targetTime, Double touchTime) {
        // -: late, +: early,
        var offset = targetTime - touchTime;

        // late------------------------early
        // xxxxx-1000======0======+1000xxxxx
        return offset switch {
            // ~1000: Miss
            var x when x is < -1000 or > +1000 => TargetResult.Miss,
            // 1000~500: Bad
            var x when x is < -500 => TargetResult.BadLate,
            var x when x is > +500 => TargetResult.BadEarly,
            // 500~300: Normal
            var x when x is < -100 => TargetResult.NormalLate,
            var x when x is > +100 => TargetResult.NormalEarly,
            // 300~0: Perfect
            _ => TargetResult.Perfect,
        };
    }

    public static TargetResult JudgeStroke(Double targetTime, Double touchTime) {
        // -: late, +: early,
        var offset = targetTime - touchTime;

        // late------------------------early
        // xxxxx-1000======0======+1000xxxxx
        return offset switch {
            // ~1000: Miss
            var x when x is < -1000 or > +1000 => TargetResult.Miss,
            // 1000~0: Perfect
            _ => TargetResult.Perfect,
        };
    }

    public static TargetResult JudgeSlide(Double targetTime, Double touchTime) {
        // -: late, +: early,
        var offset = targetTime - touchTime;

        // late------------------------early
        // xxxxx-1000======0======+1000xxxxx
        return offset switch {
            // ~1000: Miss
            var x when x is < -1000 or > +1000 => TargetResult.Miss,
            // 1000~800: Bad
            var x when x is < -800 => TargetResult.BadLate,
            var x when x is > +800 => TargetResult.BadEarly,
            // 800~400: Normal
            var x when x is < -400 => TargetResult.NormalLate,
            var x when x is > +400 => TargetResult.NormalEarly,
            // 400~0: Perfect
            _ => TargetResult.Perfect,
        };
    }
    // TODO: JudgePress
}
