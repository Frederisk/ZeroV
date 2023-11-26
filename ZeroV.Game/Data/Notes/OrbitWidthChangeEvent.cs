using System;

namespace ZeroV.Game.Data.Particles;

public class OrbitWidthChangeEvent : OrbitEventNoteBase {
    public required Double Width { get; init; }
}
