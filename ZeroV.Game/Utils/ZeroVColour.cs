using System;
using osu.Framework.Graphics;

namespace ZeroV.Game.Utils;

/// <summary>
/// Centralized color definitions and theme palette inspired by VOEZ.
/// Features bright, luminous surfaces, sharp geometric accents, and vivid neon tones.
/// </summary>
public static class ZeroVColour {
    // --- Accent & Neon Highlights ---
    public static readonly Colour4 Cyan = Colour4.FromHex("00d2d3");
    public static readonly Colour4 CyanBright = Colour4.FromHex("00f2fe");
    public static readonly Colour4 Purple = Colour4.FromHex("a55eea");
    public static readonly Colour4 PurpleBright = Colour4.FromHex("c084fc");
    public static readonly Colour4 Gold = Colour4.FromHex("ffd700");
    public static readonly Colour4 Amber = Colour4.FromHex("ff9f43");
    public static readonly Colour4 Red = Colour4.FromHex("ff4757");
    public static readonly Colour4 Pink = Colour4.FromHex("ff6b81");
    public static readonly Colour4 Green = Colour4.FromHex("2ed573");
    public static readonly Colour4 Blue = Colour4.FromHex("38bdf8");

    // --- Difficulty Levels (VOEZ Standard: Easy / Hard / Special) ---
    public static readonly Colour4 Easy = Colour4.FromHex("00d2d3");     // Bright Cyan / Aqua
    public static readonly Colour4 Hard = Colour4.FromHex("ff9f43");     // Bright Warm Amber
    public static readonly Colour4 Special = Colour4.FromHex("ff4757");  // Bright Crimson / Red

    // --- Gameplay Note Types ---
    public static readonly Colour4 NotePress = Colour4.FromHex("00d2d3");
    public static readonly Colour4 NoteSlide = Colour4.FromHex("a55eea");
    public static readonly Colour4 NoteStroke = Colour4.FromHex("ffd700");
    public static readonly Colour4 NoteBlink = Colour4.FromHex("ff6b81");

    // --- Bright & Luminous Backgrounds / Card Surfaces ---
    public static readonly Colour4 BgBase = Colour4.FromHex("eef2f7");
    public static readonly Colour4 BgLight = Colour4.FromHex("f8fafc");
    public static readonly Colour4 BgCard = Colour4.FromHex("ffffff").Opacity(0.92f);
    public static readonly Colour4 BgCardHover = Colour4.FromHex("f1f5f9").Opacity(0.97f);
    public static readonly Colour4 BgCardSelected = Colour4.FromHex("e2e8f0").Opacity(0.98f);
    public static readonly Colour4 BgPanel = Colour4.FromHex("ffffff").Opacity(0.88f);
    public static readonly Colour4 BgHeader = Colour4.FromHex("ffffff").Opacity(0.94f);
    public static readonly Colour4 BgGlass = Colour4.FromHex("ffffff").Opacity(0.82f);
    public static readonly Colour4 BgDarkCard = Colour4.FromHex("151a24").Opacity(0.92f);

    // --- Sharp Linework & Borders ---
    public static readonly Colour4 BorderSubtle = Colour4.FromHex("cbd5e1").Opacity(0.7f);
    public static readonly Colour4 BorderLight = Colour4.FromHex("e2e8f0");
    public static readonly Colour4 BorderAccent = Colour4.FromHex("00d2d3");
    public static readonly Colour4 BorderAccentHover = Colour4.FromHex("00e5ff");

    // --- Typography & Foreground ---
    public static readonly Colour4 TextDark = Colour4.FromHex("0f172a");        // Deep Slate (Main text)
    public static readonly Colour4 TextMedium = Colour4.FromHex("334155");      // Secondary text
    public static readonly Colour4 TextLight = Colour4.FromHex("64748b");       // Metadata / captions
    public static readonly Colour4 TextMuted = Colour4.FromHex("94a3b8");       // Subdued
    public static readonly Colour4 TextWhite = Colour4.White;

    // --- Leaderboard Ranks & Status ---
    public static readonly Colour4 Rank1 = Colour4.FromHex("ffd700"); // Gold
    public static readonly Colour4 Rank2 = Colour4.FromHex("94a3b8"); // Silver
    public static readonly Colour4 Rank3 = Colour4.FromHex("cd7f32"); // Bronze
    public static readonly Colour4 RankOther = Colour4.FromHex("64748b");

    public static readonly Colour4 AllPerfect = Colour4.FromHex("ffd700");
    public static readonly Colour4 FullCombo = Colour4.FromHex("00d2d3");
    public static readonly Colour4 Clear = Colour4.FromHex("2ed573");
    public static readonly Colour4 Failed = Colour4.FromHex("ff4757");

    public static Colour4 ForDifficulty(Double difficulty) => difficulty switch {
        <= 3.0 => Easy,
        <= 7.0 => Hard,
        _ => Special,
    };

    public static String DifficultyName(Double difficulty) => difficulty switch {
        <= 3.0 => "EASY",
        <= 7.0 => "HARD",
        _ => "SPECIAL",
    };

    public static Colour4 ForRank(Int32 rank) => rank switch {
        1 => Rank1,
        2 => Rank2,
        3 => Rank3,
        _ => RankOther,
    };
}
