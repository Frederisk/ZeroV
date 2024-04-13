using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;

using osuTK;

using ZeroV.Game.Scoring;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Elements.Particles;

public partial class StrokeParticle : ParticleBase {

    public StrokeParticle() : base() {
        //this.Type = ParticleType.Stroke;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Diamond {
                Size = new Vector2(ZeroVMath.DIAMOND_SIZE),
                Colour = Colour4.Gray,
            },
            new Diamond {
                Size = new Vector2(ZeroVMath.DIAMOND_SIZE * 0.88f),
                Colour = Colour4.Gold,
            },
            new Diamond {
                Size = new Vector2(ZeroVMath.DIAMOND_SIZE * 0.54f),
                Colour = Colour4.Gray,
            }
        ];
    }

    protected override TargetResult JudgeMain(in Double targetTime, in Double touchTime) =>
        base.JudgeMain(targetTime, touchTime) switch {
            TargetResult.None => TargetResult.None,
            TargetResult.Miss => TargetResult.Miss,
            _ => TargetResult.MaxPerfect,
        };

    public override TargetResult? JudgeEnter(in Double currentTime, in Boolean isTouchDown) {
        // base.JudgeEnter(currentTime, isNewTouch); // just return null
        TargetResult result = this.JudgeMain(this.Source!.StartTime, currentTime);
        return result;
    }

    public override TargetResult? JudgeUpdate(in Double currentTime, in Boolean hasTouches) {
        TargetResult result = this.JudgeMain(this.Source!.EndTime, currentTime);
        if (hasTouches || result is TargetResult.Miss) {
            return result;
        }

        return null;
    }
}
