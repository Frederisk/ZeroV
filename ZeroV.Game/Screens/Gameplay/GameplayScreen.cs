using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // sum
using System.Threading.Tasks;

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

using ZeroV.Game.Configs;
using ZeroV.Game.Data;
using ZeroV.Game.Elements;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Elements.Counters;
using ZeroV.Game.Elements.Orbits;
using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens.Gameplay;

//[Cached]
[Cached(Type = typeof(IGameplayInfo))]
public partial class GameplayScreen : Screen, IGameplayInfo {
    public TrackInfo TrackInfo { get; }

    public MapInfo MapInfo { get; }

    public Track GameplayTrack { get; private set; } = null!;
    public ScoringCalculator ScoringCalculator { get; private set; } = null!;

    [Resolved]
    private GameLoader gameLoader { get; set; } = null!;

    public Double ParticleFallingTime { get; private set; } //TimeSpan.FromSeconds(2).TotalMilliseconds;
    public Double ParticleFadingTime { get; } = 250;

    private Container<Orbit> orbits = null!;
    private Container overlay = null!;
    private ScoreCounter scoreCounter = null!;
    private ComboCounter comboCounter = null!;
    private ZeroVSpriteText topText = null!;
    private PauseOverlay pauseOverlay = null!;
    private ResultOverlay resultOverlay = null!;

    public GameplayScreen(TrackInfo trackInfo, MapInfo mapInfo) {
        this.TrackInfo = trackInfo;
        this.MapInfo = mapInfo;
        this.Anchor = Anchor.BottomCentre;
        this.Origin = Anchor.BottomCentre;

        this.lifetimeEntryManager.EntryBecameAlive += this.lifetimeEntryManager_EntryBecameAlive;
        this.lifetimeEntryManager.EntryBecameDead += this.lifetimeEntryManager_EntryBecameDead;
    }

    #region Lifetime

    private readonly LifetimeEntryManager lifetimeEntryManager = new();

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
        result |= this.lifetimeEntryManager.Update(currTime);
        return result;
    }

    #endregion Lifetime

    [BackgroundDependencyLoader]
    private void load(ZeroVConfigManager configManager, AudioManager audioManager) {
        this.ParticleFallingTime = configManager.Get<Double>(ZeroVSetting.GamePlayParticleFallingTime);

        #region Load track

        FileInfo trackFile = this.TrackInfo.TrackFile;
        NativeStorage storage = new(trackFile.Directory!.FullName);
        using StorageBackedResourceStore store = new(storage);
        ITrackStore trackStore = audioManager.GetTrackStore(store);
        this.GameplayTrack = trackStore.Get(trackFile.Name);

        #endregion Load track

        #region Load beatmap

        var wrapper = BeatmapWrapper.Create(this.TrackInfo.BeatmapFile);
        Beatmap beatmap = wrapper.GetBeatmapByIndex(this.MapInfo.Index);
        Double deviceOffset = configManager.Get<Double>(ZeroVSetting.GlobalSoundOffset);
        Double trackOffset = this.TrackInfo.FileOffset.TotalMilliseconds;
        beatmap.ApplyOffset(deviceOffset + trackOffset);

        // TODO: Need a better way to calculate the count of hit objects.
        UInt32 count = (UInt32)beatmap.OrbitSources.Sum(orbit => orbit.HitObjects.Count);
        this.ScoringCalculator = new ScoringCalculator(count);
        foreach (OrbitSource item in beatmap.OrbitSources) {
            var entry = new OrbitLifetimeEntry(item);
            this.lifetimeEntryManager.AddEntry(entry);
        }

        #endregion Load beatmap

        #region Load drawable

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
               this.comboCounter = new ComboCounter {
                      Origin = Anchor.TopRight,
                      Anchor = Anchor.TopRight,
                      Y = 75.0F,
                      X = -25.0F,
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
                   Action = () => {
                       this.GameplayTrack.Stop();
                       this.pauseOverlay.Show();
                   },
               }
            ],
        };

        this.pauseOverlay = new PauseOverlay {
            OnResume = () => {
                this.Schedule(async() => {
                    this.pauseOverlay.ButtonsContainer.Hide();
                    this.pauseOverlay.CountdownDisplay.Show();
                    this.pauseOverlay.CountdownDisplay.Text = "3";
                    await Task.Delay(1000);
                    this.pauseOverlay.CountdownDisplay.Text = "2";
                    await Task.Delay(1000);
                    this.pauseOverlay.CountdownDisplay.Text = "1";
                    await Task.Delay(1000);
                    this.pauseOverlay.Hide();
                    this.GameplayTrack.Start();
                });
            },
            OnRetry = () => {
                this.retryThisGamePlay();
            },
            OnQuit = () => {
                this.exitThisGamePlay();
            }
        };
        this.resultOverlay = new ResultOverlay();

        this.InternalChildren = [
            this.orbitDrawablePool,
            this.BlinkParticlePool,
            this.PressParticlePool,
            this.SlideParticlePool,
            this.StrokeParticlePool,
            new PlayfieldBackground(),
            // underline
            new Box() {
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Position = new Vector2(0, -ZeroVMath.SCREEN_GAME_BASELINE_Y),
                RelativeSizeAxes = Axes.X,
                Height = 10,
                Colour = Colour4.Red,
            },
            this.orbits,
            this.overlay,
            this.pauseOverlay,
            this.resultOverlay,
        ];
        this.GameplayTrack.Looping = false;
        this.GameplayTrack.Completed += () => {
            this.Schedule(() => {
                this.resultOverlay.Show();
            });
        };
        // FIXME: Add Combo
        this.ScoringCalculator.ScoringChanged += () => {
            this.scoreCounter.Current.Value = this.ScoringCalculator.DisplayScoring;
            this.comboCounter.Current.Value = this.ScoringCalculator.CurrentCombo;
            this.topText.Text = this.ScoringCalculator.CurrentTarget.ToString();
        };

        #endregion Load drawable
    }

    protected override void LoadComplete() {
        base.LoadComplete();
        this.GameplayTrack.Start();
    }

    #region Touch

    private Dictionary<TouchSource, Vector2> touchPositions = [];

    public IReadOnlyDictionary<TouchSource, Vector2> TouchPositions => this.touchPositions;

    protected override Boolean OnTouchDown(TouchDownEvent e) {
        this.touchPositions.Add(e.Touch.Source, e.ScreenSpaceTouchDownPosition);
        this.TouchUpdate?.Invoke(e.Touch.Source, true);
        return true;
    }

    protected override void OnTouchMove(TouchMoveEvent e) {
        this.touchPositions[e.Touch.Source] = e.ScreenSpaceLastTouchPosition;
        this.TouchUpdate?.Invoke(e.Touch.Source, false);
    }

    protected override void OnTouchUp(TouchUpEvent e) {
        this.touchPositions.Remove(e.Touch.Source);
        this.TouchUpdate?.Invoke(e.Touch.Source, null);
    }

    public event IGameplayInfo.TouchUpdateDelegate? TouchUpdate;

    protected override void Dispose(Boolean isDisposing) {
        base.Dispose(isDisposing);
        if (isDisposing) {
            this.GameplayTrack?.Dispose();
        }
    }

    #endregion Touch

    private void exitThisGamePlay() {
        this.gameLoader.ExitRequested = true;
        this.Exit();
    }

    private void retryThisGamePlay() {
        this.gameLoader.ExitRequested = false;
        this.Exit();
    }
}
