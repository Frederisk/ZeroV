using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;

using osuTK.Graphics;

using ZeroV.Game.Graphics.Shapes;

namespace ZeroV.Game.Elements.Particles;

public partial class PressParticle : ParticleBase {

    public PressParticle() : base() {
        this.Type = ParticleType.Press;
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
            new BlinkDiamond {
                Anchor = Anchor.BottomCentre,
                InnerColor = Color4.Pink,
                OuterColor = Color4.Black,
            },
            // top
            new BlinkDiamond {
                Anchor = Anchor.TopCentre,
                InnerColor = Color4.Pink,
                OuterColor = Color4.Black,
            },
        ];
    }

    protected override Boolean ComputeIsMaskedAway(RectangleF maskingBounds) {
        // TODO: It seems that `with` is faster than `+=` here.
        //maskingBounds.Y += 37;
        //maskingBounds.Height += 74;
        RectangleF realMasking = maskingBounds with {
            Y = maskingBounds.Y + 37,
            Height = maskingBounds.Height + 74,
        };
        return base.ComputeIsMaskedAway(realMasking);
    }
}
