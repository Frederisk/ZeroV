using System;
using System.Collections.Generic;

using ZeroV.Game.Elements.Orbits;

namespace ZeroV.Game.Objects;

public class Beatmap {
    public required List<OrbitSource> OrbitSources { get; set; }

    public void ApplyOffset(Double offset) {
        foreach (OrbitSource orbit in this.OrbitSources) {
            orbit.ApplyOffset(offset);
        }
        this.Offset += offset;
    }

    public void ResetOffset() {
        this.ApplyOffset(-this.Offset);
        this.Offset = 0;
    }

    protected Double Offset { get; set; } = 0;
}
