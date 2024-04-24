using System;

using osu.Framework.Graphics;

using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements;

public class OrbitSource : TimeSource {
    public override Double StartTime => this.KeyFrames.Span[0].Time;
    public override Double EndTime => this.KeyFrames.Span[^1].Time;

    public struct KeyFrame {
        public Double Time { get; set; }
        public Single XPosition { get; set; }
        public Single Width { get; set; }
        public Colour4 Colour { get; set; }
    }

    public ReadOnlyMemory<KeyFrame> KeyFrames { get; init; }
    public ReadOnlyMemory<ParticleSource> HitObjects { get; init; }
}
