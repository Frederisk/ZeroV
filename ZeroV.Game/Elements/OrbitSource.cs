using System;

using osuTK.Graphics;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements;

public class OrbitSource : TimeSource {
    public override Double StartTime => this.KeyFrames.Span[0].Time;
    public override Double EndTime => this.KeyFrames.Span[^1].Time;

    public struct KeyFrame {
        public Double Time { get; set; }
        public Single XPosition { get; set; }
        public Single Width { get; set; }
        public Color4 Color { get; set; }
    }

    public ReadOnlyMemory<KeyFrame> KeyFrames { get; init; }
    public ReadOnlyMemory<TimeSourceWithHit> HitObjects { get; init; }
}
