using System;
using System.Collections.Generic;
using System.IO;

using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Performance;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Elements;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Elements.Counters;
using ZeroV.Game.Elements.Orbits;
using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Screens.Gameplay;

[Cached]
public partial class GameplayScreen : Screen {

    // [Cached]
    public Track GameplayTrack = null!;

    public readonly Double ParticleFallingTime = TimeSpan.FromSeconds(2).TotalMilliseconds;
    public readonly Double ParticleFadingTime = 250;

    /// <summary>
    /// Drawable pool for <see cref="Orbit"/> objects.
    /// </summary>
    /// <remarks>
    /// FIXME: Appropriate maximum size value needs to be determined in the actual situation.
    /// </remarks>
    private readonly DrawablePool<Orbit> orbitDrawablePool = new(10, 15);

    [Cached]
    protected readonly DrawablePool<BlinkParticle> BlinkParticlePool = new(10, 15);

    [Cached]
    protected readonly DrawablePool<PressParticle> PressParticlePool = new(10, 15);

    [Cached]
    protected readonly DrawablePool<SlideParticle> SlideParticlePool = new(10, 15);

    [Cached]
    protected readonly DrawablePool<StrokeParticle> StrokeParticlePool = new(10, 15);

    [Resolved]
    private GameLoader gameLoader { get; set; } = null!;

    //private readonly Beatmap beatmap;
    private readonly LifetimeEntryManager lifetimeEntryManager = new();

    private Container<Orbit> orbits = null!;
    private Container overlay = null!;
    private ScoreCounter scoreCounter = null!;
    private ZeroVSpriteText topText = null!;
    private PauseOverlay pauseOverlay = null!;
    private FileInfo trackFileInfo;
    public ScoringCalculator ScoringCalculator;

    public GameplayScreen(Beatmap beatmap, FileInfo trackFile) {
        //this.beatmap = beatmap;
        this.trackFileInfo = trackFile;
        this.Anchor = Anchor.BottomCentre;
        this.Origin = Anchor.BottomCentre;

        this.lifetimeEntryManager.EntryBecameAlive += this.lifetimeEntryManager_EntryBecameAlive;
        this.lifetimeEntryManager.EntryBecameDead += this.lifetimeEntryManager_EntryBecameDead;

        // FIXME: Need a better way to calculate the count of hit objects.
        UInt32 count = 0;
        foreach (OrbitSource i in beatmap.OrbitSources) {
            foreach (ParticleSource ii in i.HitObjects) {
                count++;
            }
        }
        this.ScoringCalculator = new ScoringCalculator(count);

        foreach (OrbitSource item in beatmap.OrbitSources) {
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

        if (this.orbits.Remove(entry.Drawable!, false)) {
            // entry.Drawable = null;
            Logger.Log("Orbit removed.");
        }
    }

    protected override Boolean CheckChildrenLife() {
        var result = base.CheckChildrenLife();
        var currTime = this.GameplayTrack.CurrentTime;
        //var startTime = currTime - 2000;
        //var endTime = currTime + 1000;
        result |= this.lifetimeEntryManager.Update(currTime);
        return result;
    }

    [BackgroundDependencyLoader]
    private void load(AudioManager audio) {
        this.orbits = new Container<Orbit> {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
        };
        this.overlay = new Container {
            RelativeSizeAxes = Axes.Both,
            Children = [
               this.scoreCounter = new ScoreCounter {
                    Origin = Anchor.TopRight,
                    Anchor = Anchor.TopRight,
               },
               this.topText = new ZeroVSpriteText {
                    Origin = Anchor.TopCentre,
                    Anchor = Anchor.TopCentre,
                    Text = "ZeroV",
                    FontSize = 52,
               },
               new DiamondButton() {
                   Origin = Anchor.TopLeft,
                   Anchor = Anchor.TopLeft,
                   Size = new Vector2(120),
                   Text = "Pause",
                   Action = ()=> {
                       this.GameplayTrack.Stop();
                       this.pauseOverlay.Show();
                   },
               }
            ],
        };

        this.pauseOverlay = new PauseOverlay() {
            OnResume = () => {
                // TODO: Countdown 3 seconds
                this.Scheduler.AddDelayed(() => {
                    this.pauseOverlay.Hide();
                    this.GameplayTrack.Start();
                }, 3000.0);
            },
            OnRetry = () => {
                this.gameLoader.ExitRequested = false;
                this.Exit();
            },
            OnQuit = () => {
                this.gameLoader.ExitRequested = true;
                this.pauseOverlay.Hide();
                this.Exit();
            }
        };
        // FIXME:
        this.ScoringCalculator.ScoringChanged += delegate () {
            this.scoreCounter.Current.Value = this.ScoringCalculator.DisplayScoring;
            this.topText.Text = this.ScoringCalculator.CurrentTarget.ToString();
        };
        this.InternalChildren = [
            this.orbitDrawablePool,
            this.BlinkParticlePool,
            this.PressParticlePool,
            this.SlideParticlePool,
            this.StrokeParticlePool,
            new PlayfieldBackground(),
            new Box() {
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Position = new Vector2(0, -50),
                RelativeSizeAxes = Axes.X,
                Height = 10,
                Colour = Colour4.Red,
            },
            this.orbits,
            this.overlay,
            this.pauseOverlay,
        ];
#if DEBUG
        if (this.trackFileInfo is null) {
            this.GameplayTrack = new TrackVirtual(length: 1000 * 60 * 3, "春日影") {
                Looping = false
            };
        } else {
#endif
            StorageBackedResourceStore store = new(new NativeStorage(this.trackFileInfo.Directory!.FullName));
            ITrackStore trackStore = audio.GetTrackStore(store);
            this.GameplayTrack = trackStore.Get(this.trackFileInfo.Name);
#if DEBUG
        }
#endif

        this.GameplayTrack.Looping = false;
        this.GameplayTrack.Completed += () => {
            this.Exit();
        };
    }

    protected override void LoadComplete() {
        base.LoadComplete();
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
    /// <param name="isNewTouch">Whether the touch event is a new touch. This is <see langword="true"/> if the touch event is a press, and <see langword="false"/> if the touch event is a move. <see langword="null"/> if the touch event is a release.</param>
    public delegate void TouchUpdateDelegate(TouchSource source, Boolean? isNewTouch);
}
