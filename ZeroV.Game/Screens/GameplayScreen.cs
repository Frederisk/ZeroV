using System;

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Performance;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;

using osuTK;
using osuTK.Graphics;

using ZeroV.Game.Elements;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Screens;

[Cached]
public partial class GameplayScreen : Screen {
    private Track? track;

    private readonly DrawablePool<Orbit> orbitDrawablePool = new(10, 15);
    private readonly LifetimeEntryManager lifetimeEntryManager;
    private Container<Orbit> orbits = null!;

    public GameplayScreen(Beatmap beatmap) {
        this.Anchor = Anchor.BottomCentre;
        this.Origin = Anchor.BottomCentre;

        this.lifetimeEntryManager = new();
        this.lifetimeEntryManager.EntryBecameAlive += this.lifetimeEntryManager_EntryBecameAlive;
        this.lifetimeEntryManager.EntryBecameDead += this.lifetimeEntryManager_EntryBecameDead;
        foreach (OrbitSource item in beatmap.OrbitSources.Span) {
            var entry = new OrbitLifetimeEntry(item);
            this.lifetimeEntryManager.AddEntry(entry);
        }
    }

    private void lifetimeEntryManager_EntryBecameAlive(LifetimeEntry obj) {
        var entry = (OrbitLifetimeEntry)obj;
        entry.Drawable = this.orbitDrawablePool.Get();
        entry.Drawable.Source = entry.Source;

        this.orbits.Add(entry.Drawable);
        Logger.Log("Orbit added.");
    }
    private void lifetimeEntryManager_EntryBecameDead(LifetimeEntry obj) {
        var entry = (OrbitLifetimeEntry)obj;

        this.orbits.Remove(entry.Drawable!, false);
        this.orbitDrawablePool.Return(entry.Drawable);
        Logger.Log("Orbit removed.");
    }

    protected override Boolean CheckChildrenLife() {
        var result = base.CheckChildrenLife();
        var currTime = this.Time.Current;
        var startTime = currTime - 2000;
        var endTime = currTime + 1000;
        result |= this.lifetimeEntryManager.Update(startTime, endTime);
        return result;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.orbits = new Container<Orbit>() {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
        };
        this.InternalChildren = [
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
        ];

        this.AddInternal(this.orbitDrawablePool);
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

    /// <summary>
    /// Occurs when a touch event is updated. Such events include press, move, and release.
    /// </summary>
    public event TouchUpdateDelegate? TouchUpdate;

    /// <summary>
    /// Encapsulates a touch update method.
    /// </summary>
    /// <param name="source">The source of the touch event.</param>
    /// <param name="position">The position of the touch event. Null if the touch event is a release.</param>
    /// <param name="isNewTouch">Whether the touch event is a new touch. This is true if the touch event is a press, and false if the touch event is a move. For release events, this value is not important.</param>
    public delegate void TouchUpdateDelegate(TouchSource source, Vector2? position, Boolean isNewTouch);
}
