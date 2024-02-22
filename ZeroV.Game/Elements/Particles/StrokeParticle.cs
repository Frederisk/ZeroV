using osu.Framework.Allocation;
using osu.Framework.Graphics;

using osuTK;

namespace ZeroV.Game.Elements.Particles;

public partial class StrokeParticle : ParticleBase {

    public StrokeParticle(Orbit fatherOrbit) : base(fatherOrbit) {
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
}
