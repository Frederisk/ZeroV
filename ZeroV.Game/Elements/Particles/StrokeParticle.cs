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

public partial class StrokeParticle : ParticleBase {

    public StrokeParticle(Orbit fatherOrbit) : base(fatherOrbit) {
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Diamond {
                DiameterSize = 74,
                Colour = Colour4.Gray,
            },
            new Diamond {
                DiameterSize = 74 * 0.88f,
                Colour = Colour4.Gold,
            },
            new Diamond {
                DiameterSize = 40,
                Colour = Colour4.Gray,
            }
        ];
    }
}
