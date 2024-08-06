using System;
using System.Collections.Generic;

using osu.Framework.Graphics;

using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements.Orbits;

public class OrbitSource : TimeSource {
    public override Double StartTime => this.KeyFrames[0].Time;
    public override Double EndTime => this.KeyFrames[^1].Time;

    public struct KeyFrame {
        public Double Time { get; set; }
        public Single XPosition { get; set; }
        public Single Width { get; set; }
        public Colour4 Colour { get; set; }

        public void ApplyOffset(Double offset) {
            this.Time += offset;
        }
    }

    public required List<KeyFrame> KeyFrames { get; init; }

    public required List<ParticleSource> HitObjects { get; init; }

    public void ApplyOffset(Double offset) {
        foreach (KeyFrame keyFrame in this.KeyFrames) {
            keyFrame.ApplyOffset(offset);
        }
        foreach (ParticleSource particle in this.HitObjects) {
            particle.ApplyOffset(offset);
        }
        //this.KeyFramesValue = this.KeyFramesValue.ConvertAll(kf => {
        //    kf.ApplyOffset(offset);
        //    return kf;
        //});
        //this.ParticleSourcesValue = this.ParticleSourcesValue.ConvertAll(ps => {
        //    ps.ApplyOffset(offset);
        //    return ps;
        //});
    }
}
