using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

using osuTK;

namespace ZeroV.Game.Graphics.Shapes;

/// <summary>
/// A diamond shape. Can be coloured using the <see cref="Drawable.Colour"/> property.
/// </summary>
public partial class Diamond : CompositeDrawable {
    private Triangle upTriangle { get; set; } = null!;

    private Triangle downTriangle { get; set; } = null!;

    public Diamond() {
        this.Origin = Anchor.Centre;
        this.Anchor = Anchor.Centre;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.upTriangle = new Triangle() {
            Anchor = Anchor.TopCentre,
            Origin = Anchor.TopCentre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(1f, 0.5f)
        };
        this.downTriangle = new Triangle() {
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.TopCentre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(1f, -0.5f)
        };
        this.InternalChildren = [this.upTriangle, this.downTriangle];
    }

    public override Boolean Contains(Vector2 screenSpacePos) =>
        this.upTriangle.Contains(screenSpacePos)
        || this.downTriangle.Contains(screenSpacePos);
}
