using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Graphics;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Scoring;

public static class ScoringHelper {
    private static readonly String scoring_format = new('0', 7);

    public static String ToDisplayScoring(this Double scoring) {
        return scoring.ToString(scoring_format, CultureInfo.InvariantCulture);
    }

    public static Colour4 ToResultColour(this ResultInfo result) => result switch {
        { IsAllPerfect: true } and { IsAllDone: true } => Colour4.Gold,
        { IsFullCombo: true } and { IsAllDone: true } => Colour4.Blue,
        { IsAllDone: false } => Colour4.Red,
        _ => Colour4.Wheat,
    };
}
