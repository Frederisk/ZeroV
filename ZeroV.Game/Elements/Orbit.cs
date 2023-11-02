using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

using osuTK;

namespace ZeroV.Game.Elements;

internal partial class Orbit : CompositeDrawable {
    private Container? container;
    private Box? innerBox;
    private Box? innerLine;

    private Boolean isActivate;

    // TODO:
    // public override Single Height { get;set; }

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

    protected override Boolean OnHover(HoverEvent e) => this.OnActivate(e);

    protected override void OnHoverLost(HoverLostEvent e) => this.OnActivateLost(e);

    protected override void OnTouchUp(TouchUpEvent e) => this.OnActivateLost(e);

    protected override Boolean OnTouchDown(TouchDownEvent e) => this.OnActivate(e);

    protected Boolean OnActivate(UIEvent? e) {
        if (!this.isActivate) {
            this.innerBox!.Colour = Colour4.Red;
            this.isActivate = true;
        }
        return false;
    }

    protected Boolean OnActivateLost(UIEvent? e) {
        if (this.isActivate) {
            this.innerBox!.Colour = Colour4.Black;
            this.isActivate = false;
        }
        return false;
    }
}
