using System;
using System.Collections.Generic;
using System.Linq;

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

internal partial class GameplayScreen : Screen {
    private Track? track;

    private List<TrackedTouch> touches;
    private Container<Orbit>? orbits;

    public GameplayScreen() {
        this.Anchor = Anchor.BottomCentre;
        this.Origin = Anchor.BottomCentre;
        this.touches = new List<TrackedTouch>();
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

        // TODO: For test
        this.orbits.Add(
            new Orbit { X = 0, Width = 128 }
        );
        this.orbits.Add(
            new Orbit { X = 100, Width = 256 }
        );
    }

    //protected override

    protected override Boolean OnTouchDown(TouchDownEvent e) {
        TrackedTouch touch = new(e.Touch.Source, this.orbits.Children);
        touch.UpdatePosition(e.ScreenSpaceTouchDownPosition);
        this.touches.Add(touch);
        return true;
    }

    protected override void OnTouchMove(TouchMoveEvent e) {
        TrackedTouch touch = this.touches.Single(t => t.Source == e.Touch.Source);
        touch.UpdatePosition(e.ScreenSpaceLastTouchPosition);
    }

    protected override void OnTouchUp(TouchUpEvent e) {
        TrackedTouch touch = this.touches.Single(t => t.Source == e.Touch.Source);
        touch.TouchUp();
        this.touches.Remove(touch);
    }

    //protected void OnStokeStart() {}

    private class TrackedTouch {
        private IEnumerable<Orbit> orbits;
        private HashSet<Orbit> enteredOrbits;

        public TouchSource Source { get; }

        public TrackedTouch(TouchSource source, IEnumerable<Orbit> orbits) {
            this.Source = source;
            this.orbits = orbits;

            this.enteredOrbits = new HashSet<Orbit>();
        }

        public void UpdatePosition(Vector2 position) {
            foreach (Orbit orbit in this.orbits) {
                var isHoverd = orbit.ScreenSpaceDrawQuad.Contains(position);
                var isEntered = this.enteredOrbits.Contains(orbit);

                switch (isHoverd, isEntered) {
                    case (true, false):
                        orbit.TouchEnter();
                        this.enteredOrbits.Add(orbit);
                        break;

                    case (false, true):
                        orbit.TouchLeave();
                        this.enteredOrbits.Remove(orbit);
                        break;

                    // default:
                    //     throw new ApplicationException($"Illegal touch determination status: `{nameof(isHoverd)}` is `{isHoverd}` and `{nameof(isEntered)}` is `{isEntered}`.");
                }
            }
        }

        public void TouchUp() {
            foreach (Orbit orbit in this.enteredOrbits) {
                orbit.TouchLeave();
            }
        }
    }
}
