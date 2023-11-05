using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Elements;

namespace ZeroV.Game;
internal partial class GameplayScreen : Screen {

    private List<Orbit> orbits = new();
    private List<TrackedTouch> touches = new();

    public GameplayScreen() {
        this.Anchor = Anchor.BottomCentre;
        this.Origin = Anchor.BottomCentre;
    }

    [BackgroundDependencyLoader]
    private void load() {
        var testOrbit = new Orbit() {
            Position = new Vector2(0, 0)
        };
        this.orbits.Add(testOrbit);

        this.InternalChildren = new Drawable[] {
            new PlayfieldBackground(),
            testOrbit,
        };
    }

    protected override Boolean OnTouchDown(TouchDownEvent e) {
        var touch = new TrackedTouch(e.Touch.Source, this.orbits);
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

    private class TrackedTouch {
        private List<Orbit> orbits;
        private HashSet<Orbit> enteredOrbits;

        public TouchSource Source { get; }

        public TrackedTouch(TouchSource source, List<Orbit> orbits) {
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
