using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;

using osuTK;

using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;

public partial class BlinkParticle : ParticleBase {

    public BlinkParticle() : base() {
        //this.Type = ParticleType.Blink;
        //this.AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChild = new BlinkDiamond {
            //InnerColor = Colour4.Red,
            //OuterColor = Colour4.Black,
        };
    }

    // protected override TargetResult JudgeMain(in Double targetTime, in Double currentTime) =>
    //     base.JudgeMain(targetTime, currentTime);

    public override TargetResult? JudgeEnter(in Double currentTime, in Boolean isNewTouch) {
        // base.JudgeEnter(currentTime, isNewTouch); // just return null
        if (isNewTouch) {
            TargetResult result = this.JudgeMain(this.Source!.StartTime, currentTime);
            return result;
        }
        // only judge when touch is down
        return null;
    }

    // public override TargetResult? JudgeUpdate(in Double currentTime, in Boolean hasTouches) =>
    //     base.JudgeUpdate(currentTime, hasTouches);
}
