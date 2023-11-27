using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

using osuTK;
using osuTK.Graphics;

namespace ZeroV.Game.Elements.Particles;
internal partial class StrokeParticle : ParticleBase {
    private Container? container;


    public StrokeParticle(Orbit fatherOrbit) : base(fatherOrbit) {
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.container = new Container {
            Children = new Drawable[] {
                new Diamond {
                    Size = new Vector2(52),
                    Colour= Colour4.LightYellow,
                },
                new Diamond {
                    Size = new Vector2(28),
                    Colour= Colour4.White,
                },
            },
        };

        this.InternalChild = this.container;
    }

}
