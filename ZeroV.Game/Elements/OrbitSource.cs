using System;
using System.Collections.Generic;

using osu.Framework.Graphics;

using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements;

public class OrbitSource : TimeSource {
    public override Double StartTime => this.KeyFrames[0].Time;
    public override Double EndTime => this.KeyFrames[^1].Time;

    public struct KeyFrame {
        public Double Time { get; set; }
        public Single XPosition { get; set; }
        public Single Width { get; set; }
        public Colour4 Colour { get; set; }
    }

    public required List<KeyFrame> KeyFrames { get; init; }
    public required List<ParticleSource> HitObjects { get; init; }
}
