using System;

using osuTK.Graphics;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements;

public class OrbitSource : ZeroVObjectSource {
    public override Double StartTime => this.KeyFrames.Span[0].Time;
    public override Double EndTime => this.KeyFrames.Span[^1].Time;

    public struct KeyFrame {
        public Double Time { get; set; }
        public Single Position { get; set; }
        public Single Width { get; set; }
        public Color4 Color { get; set; }
    }

    public ReadOnlyMemory<KeyFrame> KeyFrames { get; init; }
    public ReadOnlyMemory<ZeroVObjectSourceWithHit> HitObjects { get; init; }
}
