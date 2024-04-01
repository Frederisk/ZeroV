using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;

using osuTK;

using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;

public partial class StrokeParticle : ParticleBase {

    public StrokeParticle() : base() {
        //this.Type = ParticleType.Stroke;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Diamond {
                Size = new Vector2(74),
                Colour = Colour4.Gray,
            },
            new Diamond {
                Size = new Vector2(74 * 0.88f),
                Colour = Colour4.Gold,
            },
            new Diamond {
                Size = new Vector2(40),
                Colour = Colour4.Gray,
            }
        ];
    }

    public override TargetResult? JudgeEnter(in Double currentTime, in Boolean isNewTouch) {
        // base.JudgeEnter(currentTime, isNewTouch); // just return null
        TargetResult result = Judgment.JudgeStroke(this.Source!.StartTime, currentTime);
        return result;
    }

    public override TargetResult? JudgeUpdate(in Double currentTime, in Boolean hasTouches) {
        // base.JudgeUpdate(currentTime); // just return null
        TargetResult result = Judgment.JudgeBlink(this.Source!.EndTime, currentTime);
        if (hasTouches || result is TargetResult.Miss) {
            return result;
        }

        return null;
    }
}
