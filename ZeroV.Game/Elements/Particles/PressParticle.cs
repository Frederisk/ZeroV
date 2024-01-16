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
            new Box {
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Width = Single.Sqrt(2 * 52 * 52),
                RelativeSizeAxes = Axes.Y,
                Colour = Color4.Pink,
            },
            new Box {
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Width = 6.1f,
                RelativeSizeAxes = Axes.Y,
                Colour = Color4.Black,
            },
            new Box {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.BottomLeft,
                Width = 6.1f,
                RelativeSizeAxes = Axes.Y,
                Colour = Color4.Black,
            },
            new Box {
                Origin = Anchor.BottomRight,
                Anchor = Anchor.BottomRight,
                Width = 6.1f,
                RelativeSizeAxes = Axes.Y,
                Colour = Color4.Black,
            },
            // buttom
            new Diamond {
                Anchor = Anchor.BottomCentre,
                DiameterSize = 52,
                Colour = Color4.Black,
            },
            new Diamond {
                Anchor = Anchor.BottomCentre,
                DiameterSize = 28,
                Colour = Color4.Pink,
            },
            // top
            new Diamond {
                Anchor = Anchor.TopCentre,
                DiameterSize = 52,
                Colour = Color4.Black,
            },
            new Diamond {
                Anchor = Anchor.TopCentre,
                DiameterSize = 28,
                Colour = Color4.Pink,
            },
        ];
    }
}
