using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

using osuTK.Graphics;

namespace ZeroV.Game.Elements.Particles;

public partial class PressParticle : ParticleBase {

    public PressParticle(Orbit fatherOrbit) : base(fatherOrbit) {
        this.AutoSizeAxes = Axes.X;
        this.Origin = Anchor.BottomCentre;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            // hold
            //new Box {
            //    Origin = Anchor.BottomCentre,
            //    Anchor = Anchor.BottomCentre,
            //    Width = Single.Sqrt(2 * 52 * 52),
            //},
            new Box {
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Width = Single.Sqrt(2 * 52 * 52),
                RelativeSizeAxes = Axes.Y,
                Colour = Color4.Pink,
            },
            // buttom
            new Diamond {
                Anchor = Anchor.BottomCentre,
                DiameterSize = 52,
                Colour = Color4.Black,
            },
            // top
            new Diamond {
                Anchor = Anchor.TopCentre,
                DiameterSize = 52,
                Colour = Color4.Black,
            },
        ];
    }
}
