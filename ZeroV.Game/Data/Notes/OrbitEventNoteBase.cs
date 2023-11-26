using System;

using osu.Framework.Graphics;

namespace ZeroV.Game.Data.Particles;

public class OrbitEventNoteBase : NoteBase {
    public required TimeSpan EndTime { get; init; }
    public required Easing Easing { get; init; }
}
