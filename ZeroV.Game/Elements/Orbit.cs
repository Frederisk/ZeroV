using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

using osuTK;

namespace ZeroV.Game.Elements;

internal partial class Orbit : CompositeDrawable {
    private Container? container;
    private Box? innerBox;
    private Box? innerLine;

    // TODO:
    //public override Single Height { get;set;
    //}

    public Orbit() {
        this.AutoSizeAxes = Axes.Both;
        this.Origin = Anchor.Centre;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.innerBox = new Box {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Colour = Colour4.Black,
            Size = new Vector2(100, 768),
        };
        this.innerLine = new Box {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Colour = Colour4.White,
            EdgeSmoothness = new Vector2(3, 0),
            Size = new Vector2(4, 768),
        };

        this.container = new Container() {
            AutoSizeAxes = Axes.Both,
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Children = new Drawable[] {
                this.innerBox,
                this.innerLine,
            }
        };
        this.InternalChild = this.container;
    }
}
