using System;
using osuTK.Graphics;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements;

public class Orbit : ZeroVObject
{
    public override double StartTime => KeyFrames.Span[0].Time;
    public override double EndTime => KeyFrames.Span[^1].Time;

    public struct KeyFrame {
        public Double Time { get; set; }
        public Single Position { get; set; }
        public Single Width { get; set; }
        public Color4 Color { get; set; }
    }
    public required ReadOnlyMemory<KeyFrame> KeyFrames { get; init; }
    public required ReadOnlyMemory<ZeroVHitObject> HitObjects { get; init; }
}
