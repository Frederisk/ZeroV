using osu.Framework.Allocation;
using osu.Framework.Graphics;

using osuTK;

using ZeroV.Game.Graphics.Shapes;

namespace ZeroV.Game.Elements.Particles;

public partial class BlinkParticle : ParticleBase<BlinkParticleSource> {

    public BlinkParticle() : base() {
        //this.AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChild = new BlinkDiamond {
            //InnerColor = Colour4.Red,
            //OuterColor = Colour4.Black,
        };
    }
}
