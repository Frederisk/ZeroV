using System;

using osu.Framework.Graphics;

namespace ZeroV.Game.Utils;

public static class ZeroVColour {
    public static readonly Colour4 ALL_PERFECT = Colour4.FromHex("ffd700");
    public static readonly Colour4 FULL_COMBO = Colour4.FromHex("00d2d3");
    public static readonly Colour4 CLEAR = Colour4.FromHex("2ed573");
    public static readonly Colour4 FAILED = Colour4.FromHex("ff4757");

    public static Colour4 FromDifficulty(Double difficulty) => difficulty switch {
        <= 0 => Colour4.Gray,
        > 0 and <= 2 => Colour4.Cyan,
        > 3 and <= 5 => Colour4.GreenYellow,
        > 6 and <= 8 => Colour4.OrangeRed,
        > 8 and < 10 => Colour4.Red,
        > 10 => Colour4.Purple,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
    };
}
