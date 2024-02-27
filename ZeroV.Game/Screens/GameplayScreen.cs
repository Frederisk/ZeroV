using System;
using System.Collections.Generic;

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
using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Screens;

[Cached]
public partial class GameplayScreen : Screen {

    // [Cached]
    public Track GameplayTrack = null!;

    /// <summary>
    /// Drawable pool for <see cref="Orbit"/> objects.
    /// </summary>
    /// <remarks>
    /// FIXME: Appropriate maximum size value needs to be determined in the actual situation.
    /// </remarks>
    private readonly DrawablePool<Orbit> orbitDrawablePool = new(10, 15);

    [Cached]
    protected readonly DrawablePool<BlinkParticle> BlinkParticlePool = new(10, 15);

    private readonly LifetimeEntryManager lifetimeEntryManager;
    private Container<Orbit> orbits = null!;
    private Container overlay = null!;

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
        var currTime = this.GameplayTrack.CurrentTime;
        var startTime = currTime - 2000;
        var endTime = currTime + 1000;
        result |= this.lifetimeEntryManager.Update(startTime, endTime);
        return result;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.orbits = new Container<Orbit> {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
        };
        this.overlay = new Container {
            RelativeSizeAxes = Axes.Both,
            Children = [
                new ScoreCounter {
                    Origin = Anchor.TopRight,
                    Anchor = Anchor.TopRight,
                },
            ],
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
            this.overlay,
        ];
        this.AddInternal(this.orbitDrawablePool);
        this.AddInternal(this.BlinkParticlePool);

        // FIXME: This is a temporary solution. The track should be loaded from the beatmap.
        this.GameplayTrack = new TrackVirtual(length: 1000 * 60 * 3, "春日影") {
            Looping = false
        };
        this.GameplayTrack.Start();
    }

    public Dictionary<TouchSource, Vector2> TouchPositions = [];

    protected override Boolean OnTouchDown(TouchDownEvent e) {
        this.TouchPositions.Add(e.Touch.Source, e.ScreenSpaceTouchDownPosition);
        this.TouchUpdate?.Invoke(e.Touch.Source, true);
        return true;
    }

    protected override void OnTouchMove(TouchMoveEvent e) {
        this.TouchPositions[e.Touch.Source] = e.ScreenSpaceLastTouchPosition;
        this.TouchUpdate?.Invoke(e.Touch.Source, false);
    }

    protected override void OnTouchUp(TouchUpEvent e) {
        this.TouchPositions.Remove(e.Touch.Source);
        this.TouchUpdate?.Invoke(e.Touch.Source, null);
    }

    /// <summary>
    /// Occurs when a touch event is updated. Such events include press, move, and release.
    /// </summary>
    public event TouchUpdateDelegate? TouchUpdate;

    /// <summary>
    /// Encapsulates a touch update method.
    /// </summary>
    /// <param name="source">The source of the touch event.</param>
    /// <param name="isNewTouch">Whether the touch event is a new touch. This is true if the touch event is a press, and false if the touch event is a move. Null if the touch event is a release.</param>
    public delegate void TouchUpdateDelegate(TouchSource source, Boolean? isNewTouch);
}
