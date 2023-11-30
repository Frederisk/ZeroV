using osu.Framework.Allocation;
using osu.Framework.Graphics;
// using osu.Framework.Graphics.Containers;
// using osu.Framework.Graphics.Shapes;
// using osu.Framework.Graphics.Textures;

using osuTK;

namespace ZeroV.Game.Elements.Particles;

internal partial class BlinkParticle : ParticleBase {
    // private Container? container;
    //public Double StartTime { get; set; }

    public BlinkParticle(Orbit fatherOrbit) : base(fatherOrbit) {
        //this.AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Diamond {
                Size = new Vector2(52),
                Colour= Colour4.Black,
            },
            new Diamond {
                Size = new Vector2(28),
                Colour= Colour4.Red,
            },
        ];
    }
}
