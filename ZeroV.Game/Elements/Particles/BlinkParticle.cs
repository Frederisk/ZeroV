using osu.Framework.Allocation;
using osu.Framework.Graphics;

using osuTK;

namespace ZeroV.Game.Elements.Particles;

public partial class BlinkParticle : ParticleBase {
    // private Container? container;
    //public Double StartTime { get; set; }

    public BlinkParticle(Orbit fatherOrbit) : base(fatherOrbit) {
        //this.AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Diamond {
                //DiameterSize = 52,
                Size = new Vector2(52),
                Colour = Colour4.Black,
            },
            new Diamond {
                DiameterSize = 28,
                Colour = Colour4.Red,
            },
        ];
    }
}
