using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;

using osuTK;

namespace ZeroV.Game.Elements.Particles;

internal partial class BlinkParticle : ParticleBase {
    private Container? container;

    //public Double StartTime { get; set; }

    public BlinkParticle(Orbit fatherOrbit) : base(fatherOrbit) {
        //this.AutoSizeAxes = Axes.Both;
        //this.Origin = Anchor.Centre;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.container = new Container {
            //AutoSizeAxes = Axes.Both,
            //Origin = Anchor.Centre,
            //Anchor = Anchor.Centre,
            Children = new Drawable[] {
                new Box {
                    Origin= Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Size = new Vector2(52),
                    Colour= Colour4.Black,
                    Rotation = 45,
                },
                new Box {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    //RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(28),
                    Colour= Colour4.Red,
                    Rotation = 45,
                },
                //new Sprite {
                //    Anchor = Anchor.Centre,
                //    Origin = Anchor.Centre,
                //    Texture = textures.Get("logo")
                //},
            }
        };
        this.InternalChild = this.container;
    }

    //protected override Boolean OnTouchDown(TouchDownEvent e) => this.onDown(e);
    //protected override Boolean OnMouseDown(MouseDownEvent e) => this.onDown(e);
    //protected override Boolean OnTabletPenButtonPress(TabletPenButtonPressEvent e) => this.onDown(e);

    //private Boolean onDown(UIEvent e) {
    //    Drawable child = this.container!.Children[0];
    //    child.Colour = child.Colour == Color4.Blue ? Colour4.Red : Color4.Blue;
    //    HitTarget?.Invoke();
    //    return true;
    //}

    //public event Action? HitTarget;
}
