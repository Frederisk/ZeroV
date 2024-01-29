using System;

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Framework.Screens;

using osuTK;
using osuTK.Graphics;

using ZeroV.Game.Elements;

namespace ZeroV.Game.Screens;

public partial class GameplayScreen : Screen {
    private Track? track;

    private Container<Orbit> orbits = null!;

    public GameplayScreen() {
        this.Anchor = Anchor.BottomCentre;
        this.Origin = Anchor.BottomCentre;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.orbits = new Container<Orbit>() {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
        };
        this.InternalChildren = new Drawable[] {
            new PlayfieldBackground(),
            new Box() {
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Position = new Vector2(0, -50),
                RelativeSizeAxes = Axes.X,
                Height = 10,
                Colour = Color4.Red,
            },
            this.orbits,
        };
    }

    protected override void LoadComplete() {
        // TODO: For test
        this.orbits.Add(
            new Orbit(this) { X = 0, Width = 128 }
        );
        this.orbits.Add(
            new Orbit(this) { X = 100, Width = 256 }
        );
        base.LoadComplete();
    }

    protected override Boolean OnTouchDown(TouchDownEvent e) {
        this.TouchUpdate?.Invoke(e.Touch.Source, e.ScreenSpaceTouchDownPosition, true);
        return true;
    }

    protected override void OnTouchMove(TouchMoveEvent e) {
        this.TouchUpdate?.Invoke(e.Touch.Source, e.ScreenSpaceLastTouchPosition, false);
    }

    protected override void OnTouchUp(TouchUpEvent e) {
        this.TouchUpdate?.Invoke(e.Touch.Source, null, false);
    }

    public event TouchUpdateDelegate? TouchUpdate;

    public delegate void TouchUpdateDelegate(TouchSource source, Vector2? position, Boolean isNewTouch);
}
