using System;
using System.Collections.Generic;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Performance;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Utils;

using osuTK;

using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Graphics;
using ZeroV.Game.Scoring;
using ZeroV.Game.Screens;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Elements.Orbits;

/// <summary>
/// OrbitSources that carry particles. It's also the main interactive object in this game.
/// </summary>
public partial class Orbit : ZeroVPoolableDrawable<OrbitSource> {

    /// <summary>
    /// The position of the top of visible orbit.
    /// </summary>
    private const Single visual_orbit_top = -ZeroVMath.SCREEN_DRAWABLE_Y;

    /// <summary>
    /// The position beyond the Y-axis at the top of visible orbit.
    /// </summary>
    private const Single visual_orbit_out_of_top = visual_orbit_top - ZeroVMath.DIAMOND_SIZE / 2;

    /// <summary>
    /// The position of the bottom of visible orbit. It's also the offset of visible orbit relative to the screen.
    /// </summary>
    private const Single visual_orbit_offset = -ZeroVMath.SCREEN_GAME_BASELINE_Y;

    /// <summary>
    /// The position of the bottom of the screen.
    /// </summary>
    private const Single visual_orbit_bottom = 0;

    /// <summary>
    /// The position beyond the Y-axis at the bottom of visible orbit.
    /// </summary>
    private const Single visual_orbit_out_of_bottom = visual_orbit_bottom + ZeroVMath.DIAMOND_SIZE / 2;

    /// <summary>
    /// The container that contains all the elements of the orbit.
    /// </summary>
    /// <remarks>
    /// This field will never be null after <see cref="LoadComplete"/> has been called.
    /// It's a <see cref="BufferedContainer"/> because we need to use <see cref="BufferedContainer.Blending"/> to make the orbit partially hidden.
    /// </remarks>
    private BufferedContainer container = null!;

    private Box innerBox = null!;
    private Box innerLine = null!;
    private ParticleQueue particles = null!;

    // FIXME: These properties are redundant. In the future, they will be obtained by some fade-in animations.
    public new Single Y => base.Y;

    public new Single X { get => base.X; set => base.X = value; }
    public new Single Height => base.Height;
    public new Single Width { get => base.Width; set => base.Width = value; }

    [Resolved]
    private GameplayScreen gameplayScreen { get; set; } = null!;
    private Double currentTime => this.gameplayScreen.GameplayTrack.CurrentTime;
    private Double particleFallingTime => this.gameplayScreen.ParticleFallingTime;
    private Double particleFadingTime => this.gameplayScreen.ParticleFadingTime;

    public Orbit() {
        this.Origin = Anchor.BottomCentre;
        this.Anchor = Anchor.BottomCentre;
        this.lifetimeEntryManager.EntryBecameAlive += this.lifetimeEntryManager_EntryBecameAlive;
        this.lifetimeEntryManager.EntryBecameDead += this.lifetimeEntryManager_EntryBecameDead;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.innerBox = new Box {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Colour = Colour4.Azure,
            RelativeSizeAxes = Axes.Both,
            // Size = new Vector2(5000, 768-50),
            // XPosition = new Vector2(0, -50),
            Y = visual_orbit_offset,
        };
        this.innerLine = new Box {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Colour = Colour4.Black,
            RelativeSizeAxes = Axes.Y,
            Width = 1,
            EdgeSmoothness = new Vector2(1, 0),
            Y = visual_orbit_offset,
            // XPosition = new Vector2(0, visual_orbit_offset),
        };
        this.particles = new ParticleQueue() {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
        };
        this.container = new BufferedContainer() {
            RelativeSizeAxes = Axes.Both,
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Children = [
                //this.TouchSpace,
                this.innerBox,
                this.innerLine,
                this.particles,
                // From `osu.Game.Rulesets.Mania.UI.PlayfieldCoveringWrapper`
                // Partially hidden
                new Container {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Blending = new BlendingParameters {
                        // Don't change the destination colour.
                        RGBEquation = BlendingEquation.Add,
                        Source = BlendingType.Zero,
                        Destination = BlendingType.One,
                        // Subtract the cover's alpha from the destination (points with alpha 1 should make the destination completely transparent).
                        AlphaEquation = BlendingEquation.Add,
                        SourceAlpha = BlendingType.Zero,
                        DestinationAlpha = BlendingType.OneMinusSrcAlpha
                    },
                    Children = [
                        new Box {
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            RelativeSizeAxes = Axes.Both,
                            RelativePositionAxes = Axes.Both,
                            // Colour = Color4.White.Opacity(0.1f),
                            Y = 0f,
                            Height = 0.05f
                        },
                        new Box {
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            RelativeSizeAxes = Axes.Both,
                            RelativePositionAxes = Axes.Both,
                            Y = 0.05f,
                            Height = 0.25f,
                            Colour = ColourInfo.GradientVertical(
                                Colour4.White.Opacity(1f),
                                Colour4.White.Opacity(0f)
                            )
                        },
                    ],
                }
            ]
        };
        this.InternalChild = this.container;

        // FIXME: Just for test, remove it.
        base.Height = ZeroVMath.SCREEN_DRAWABLE_Y;
        base.Y = 0;
        this.Alpha = 0.9f;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This field will never be null after <see cref="Source"/> has been set.
    /// </remarks>
    private List<OrbitSource.KeyFrame> keyFrames = null!;

    protected override void Update() {
        base.Update();
        var currTime = this.currentTime;
        while (this.keyFrames.Count > 1) {
            var nextTime = this.keyFrames[1].Time;
            if (currTime < nextTime) {
                break;
            }
            this.keyFrames = this.keyFrames[1..];
        }

        if (this.keyFrames.Count > 1) {
            OrbitSource.KeyFrame currKeyFrame = this.keyFrames[0];
            OrbitSource.KeyFrame nextKeyFrame = this.keyFrames[1];

            this.innerBox.Colour = Interpolation.ValueAt(currTime,
                currKeyFrame.Colour, nextKeyFrame.Colour,
                currKeyFrame.Time, nextKeyFrame.Time);

            var nextX = Interpolation.ValueAt(currTime,
                currKeyFrame.XPosition, nextKeyFrame.XPosition,
                currKeyFrame.Time, nextKeyFrame.Time);
            var isXChanged = this.X != nextX;
            if (isXChanged) {
                this.X = nextX;
            }

            var nextWidth = Interpolation.ValueAt(currTime,
                currKeyFrame.Width, nextKeyFrame.Width,
                currKeyFrame.Time, nextKeyFrame.Time);
            var isWidthChanged = this.Width != nextWidth;
            if (isWidthChanged) {
                this.Width = nextWidth;
            }

            if (isXChanged || isWidthChanged) {
                foreach (TouchSource touchSource in this.gameplayScreen.TouchPositions.Keys) {
                    this.OnTouchChecked(touchSource, false);
                }
            }
        }

        foreach (ParticleBase item in this.particles) {
            //if (currTime < item.EndTime) {
            var startTime = item.Source!.StartTime - this.particleFallingTime;

            item.Y = Interpolation.ValueAt(currTime,
            visual_orbit_out_of_top, visual_orbit_offset,
            startTime, item.Source.StartTime);
            //item.Y += 1;
            //} else {
            //    var endTime = item.EndTime + this.particleFadingTime;
            //    var startOffset = visual_orbit_offset;
            //    var endOffset = visual_orbit_out_of_bottom;
            //    if (item is PressParticle press) {
            //        startOffset -= press.Height;
            //        endOffset -= press.Height;
            //    }

            //    item.Y = Interpolation.ValueAt(currTime,
            //        startOffset, endOffset,
            //        item.EndTime, endTime);
            //    item.Alpha = Interpolation.ValueAt(currTime,
            //        1f, 0f,
            //        item.EndTime, endTime);
            //}
        }

        TargetResult? result = this.particles.PeekOrDefault()?.JudgeUpdate(this.currentTime, this.HasTouches);
        this.processTarget(result);
    }

    #region Poolable

    private readonly LifetimeEntryManager lifetimeEntryManager = new();

    [Resolved]
    private DrawablePool<BlinkParticle> blinkParticlePool { get; set; } = null!;

    [Resolved]
    private DrawablePool<PressParticle> pressParticlePool { get; set; } = null!;

    [Resolved]
    private DrawablePool<SlideParticle> slideParticlePool { get; set; } = null!;

    [Resolved]
    private DrawablePool<StrokeParticle> strokeParticlePool { get; set; } = null!;

    public override OrbitSource? Source {
        get => base.Source;
        set {
            if (value is not null) {
                this.keyFrames = value.KeyFrames;

                this.lifetimeEntryManager.ClearEntries();
                foreach (ParticleSource item in value.HitObjects) {
                    this.lifetimeEntryManager.AddEntry(new ParticleLifetimeEntry(item));
                }
            }
            base.Source = value;
        }
    }

    private void lifetimeEntryManager_EntryBecameAlive(LifetimeEntry obj) {
        var entry = (ParticleLifetimeEntry)obj;

        entry.Drawable = entry.Source switch {
            BlinkParticleSource blinkSource => this.blinkParticlePool.Get(),
            PressParticleSource pressSource => this.pressParticlePool.Get(p => {
                p.UpdateLength(pressSource.StartTime, pressSource.EndTime);
            }),
            SlideParticleSource slideSource => this.slideParticlePool.Get(p => {
                p.Direction = slideSource.Direction;
            }),
            StrokeParticleSource strokeSource => this.strokeParticlePool.Get(),
            _ => throw new NotImplementedException(),
        };

        entry.Drawable.Source = entry.Source;
        this.particles.Enqueue(entry.Drawable);
        Logger.Log($"{entry.Drawable.GetType()} added.");
    }

    private void lifetimeEntryManager_EntryBecameDead(LifetimeEntry obj) {
        var entry = (ParticleLifetimeEntry)obj;

        if (this.particles.Remove(entry.Drawable!, false)) {
            // entry.Drawable = null;
            Logger.Log("Particle removed.");
        }
    }

    protected override Boolean CheckChildrenLife() {
        var result = base.CheckChildrenLife();
        if (this.gameplayScreen.GameplayTrack is not null) {
            var currTime = this.currentTime;
            // Time to fade out
            var startTime = currTime - this.particleFadingTime;
            // Time to fall down
            var endTime = currTime + this.particleFallingTime;
            result |= this.lifetimeEntryManager.Update(startTime, endTime);
        }
        return result;
    }

    protected override void PrepareForUse() {
        base.PrepareForUse();
        this.gameplayScreen.TouchUpdate += this.OnTouchChecked;
    }

    protected override void FreeAfterUse() {
        this.gameplayScreen.TouchUpdate -= this.OnTouchChecked;
        base.FreeAfterUse();
    }

    #endregion Poolable

    #region Touch

    private HashSet<TouchSource> touches = [];
    public Boolean HasTouches => this.touches.Count > 0;

    protected void OnTouchChecked(TouchSource source, Boolean? isNewTouch) {
        if (isNewTouch is null) {
            this.OnTouchLeave(source);
            return;
        }

        Vector2 touchPosition = this.gameplayScreen.TouchPositions[source];
        var isHovered = this.ScreenSpaceDrawQuad.Contains(touchPosition);
        var isEntered = this.touches.Contains(source);

        switch (isHovered, isEntered) {
            case (true, false): this.OnTouchEnter(source, isNewTouch.Value); break;
            case (false, true): this.OnTouchLeave(source); break;
        }
    }

    protected void OnTouchEnter(TouchSource source, Boolean isTouchDown) {
        this.touches.Add(source);
        TargetResult? result = this.particles.PeekOrDefault()?.JudgeEnter(this.currentTime, isTouchDown);
        this.processTarget(result);
    }

    protected override void OnTouchMove(TouchMoveEvent e) {
        // base.OnTouchMove(e); // do nothing.
        TargetResult? result = this.particles.PeekOrDefault()?.JudgeMove(this.currentTime, e.Delta);
        this.processTarget(result);
    }

    protected void OnTouchLeave(TouchSource source) {
        this.touches.Remove(source);
        TargetResult? result = this.particles.PeekOrDefault()?.JudgeLeave(this.currentTime, false);
        this.processTarget(result);
    }

    private void processTarget(TargetResult? result) {
        if (result is not null and not TargetResult.None) {
            this.gameplayScreen.ScoringCalculator.AddTarget(result.Value);
            this.particles.DequeueInJudgeOnly();
        }
    }

    #endregion Touch
}
