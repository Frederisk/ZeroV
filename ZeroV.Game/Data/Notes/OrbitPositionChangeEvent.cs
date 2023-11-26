using System;

namespace ZeroV.Game.Data.Particles;

public class OrbitPositionChangeEvent : OrbitEventNoteBase {
    public required Double Position { get; init; }
}
