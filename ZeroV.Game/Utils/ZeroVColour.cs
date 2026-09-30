using System;

using osu.Framework.Graphics;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Utils;

public static class ZeroVColour {
    public static readonly Colour4 ALL_PERFECT = Colour4.Gold;
    public static readonly Colour4 FULL_COMBO = Colour4.SkyBlue;
    public static readonly Colour4 CLEAR = Colour4.LightGreen;
    public static readonly Colour4 FAILED = Colour4.DarkRed;

    public static Colour4 FromDifficulty(Double difficulty) => difficulty switch {
        <= 0 => Colour4.Gray,
        > 0 and <= 2 => Colour4.Cyan,
        > 3 and <= 5 => Colour4.GreenYellow,
        > 6 and <= 8 => Colour4.OrangeRed,
        > 8 and < 10 => Colour4.Red,
        > 10 => Colour4.Purple,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
    };

    public static Colour4 FromResult(ResultInfo result) => result switch {
        { IsAllPerfect: true, IsAllDone: true } => ALL_PERFECT,
        { IsFullCombo: true, IsAllDone: true } => FULL_COMBO,
        { IsAllDone: true } => CLEAR,
        { IsAllDone: false } => FAILED,
    };

    public static Colour4 FromRank(Int32 rank) => rank switch {
        1 => Colour4.Gold,
        2 => Colour4.Silver,
        3 => Colour4.Orange,
        _ => Colour4.DimGray,
    };
}
