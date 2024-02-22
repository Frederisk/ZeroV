using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace ZeroV.Game;

/// <summary>
/// A diamond shape. Can be coloured using the <see cref="Drawable.Colour"/> property.
/// </summary>
public partial class Diamond : CompositeDrawable {

    public Diamond() {
        this.AddInternal(new Triangle() {
            Anchor = osu.Framework.Graphics.Anchor.TopCentre,
            Origin = osu.Framework.Graphics.Anchor.TopCentre,
            RelativeSizeAxes = osu.Framework.Graphics.Axes.Both,
            Size = new osuTK.Vector2(1f, 0.5f)
        });
        this.AddInternal(new Triangle() {
            Anchor = osu.Framework.Graphics.Anchor.BottomCentre,
            Origin = osu.Framework.Graphics.Anchor.TopCentre,
            RelativeSizeAxes = osu.Framework.Graphics.Axes.Both,
            Size = new osuTK.Vector2(1f, -0.5f)
        });
    }

}
