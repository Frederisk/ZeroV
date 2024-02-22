using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace ZeroV.Game;

/// <summary>
/// A diamond shape. Can be coloured using the <see cref="Drawable.Colour"/> property.
/// </summary>
public partial class Diamond : CompositeDrawable {

    public Diamond() {
        this.Origin = Anchor.Centre;
        this.Anchor = Anchor.Centre;
        this.AddInternal(new Triangle() {
            Anchor = Anchor.TopCentre,
            Origin = Anchor.TopCentre,
            RelativeSizeAxes = Axes.Both,
            Size = new osuTK.Vector2(1f, 0.5f)
        });
        this.AddInternal(new Triangle() {
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.TopCentre,
            RelativeSizeAxes = Axes.Both,
            Size = new osuTK.Vector2(1f, -0.5f)
        });
    }
}
