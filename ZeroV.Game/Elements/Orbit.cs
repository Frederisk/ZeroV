using System;
using System.Collections.Generic;

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
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
using osuTK.Graphics;

using SharpGen.Runtime;

using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Graphics;
using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;
using ZeroV.Game.Screens;

namespace ZeroV.Game.Elements;

/// <summary>
/// OrbitSources that carry particles. It's also the main interactive object in this game.
/// </summary>
public partial class Orbit : ZeroVPoolableDrawable<OrbitSource> {

    /// <summary>
    /// The size of half the particle's Y-axis radius.
    /// </summary>
    /// TODO: This value should be obtained from the particle's size.
    private const Single visual_half_of_particle_size = 24;

    /// <summary>
    /// The position of the top of visible orbit.
    /// </summary>
    private const Single visual_orbit_top = -768;

    /// <summary>
    /// The position beyond the Y-axis at the top of visible orbit.
    /// </summary>
    /// <remarks>
    /// -768 - 24 = -792
    /// </remarks>
    private const Single visual_orbit_out_of_top = visual_orbit_top - visual_half_of_particle_size;

    /// <summary>
    /// The position of the bottom of visible orbit. It's also the offset of visible orbit relative to the screen.
    /// </summary>
    private const Single visual_orbit_offset = -50;

    /// <summary>
    /// The position of the bottom of the screen.
    /// </summary>
    private const Single visual_orbit_bottom = 0;

    /// <summary>
    /// The position beyond the Y-axis at the bottom of visible orbit.
    /// </summary>
    /// <remarks>
    /// 0 + 24 = 24
    /// </remarks>
    private const Single visual_orbit_out_of_bottom = visual_orbit_bottom + visual_half_of_particle_size;

    /// <summary>
    /// The container that contains all the elements of the orbit.
    /// </summary>
    /// <remarks>
    /// This field will never be null after <see cref="LoadComplete"/> has been called.
    /// It's a <see cref="BufferedContainer"/> because we need to use <see cref="BufferedContainer.Blending"/> to make the orbit partially hidden.
    /// </remarks>
    private BufferedContainer container = null!;

    private Double particleFallingTime = TimeSpan.FromSeconds(5).TotalMilliseconds;
    private Double particleFadingTime = TimeSpan.FromSeconds(1).TotalMilliseconds;

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
            // Size = new Vector2(5000,768-50),
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
                                Color4.White.Opacity(1f),
                                Color4.White.Opacity(0f)
                            )
                        },
                    ],
                }
            ]
        };
        this.InternalChild = this.container;

        // FIXME: Just for test, remove it.
        base.Height = 768;
        base.Y = 0;
        this.Alpha = 0.9f;
    }

    private ReadOnlyMemory<OrbitSource.KeyFrame> keyFrames;

    protected override void Update() {
        base.Update();
        // var currTime = this.Time.Current;
        var currTime = this.gameplayScreen.GameplayTrack.CurrentTime;
        while (this.keyFrames.Length > 1) {
            var nextTime = this.keyFrames.Span[1].Time;
            if (currTime < nextTime) {
                break;
            }
            this.keyFrames = this.keyFrames[1..];
        }

        if (this.keyFrames.Length > 1) {
            OrbitSource.KeyFrame currKeyFrame = this.keyFrames.Span[0];
            OrbitSource.KeyFrame nextKeyFrame = this.keyFrames.Span[1];

            this.innerBox.Colour = Interpolation.ValueAt(currTime,
                currKeyFrame.Color, nextKeyFrame.Color,
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
    }

    #region Judge

    private void judgeBlinkMain() {
        if (this.particles.QueueFirstOrDefalut is not BlinkParticle lastParticle) {
            return;
        }
        TargetResult result = Judgment.JudgeBlink(lastParticle.Source!.StartTime, this.gameplayScreen.GameplayTrack.CurrentTime);
        if (result is not TargetResult.None) {
            this.particles.HideFirst();
        }
        this.gameplayScreen.ScoringCalculator.AddTarget(result);
    }

    private Boolean waitSlideMove = false;
    private SlideParticle? currJudgeSlide;
    private TouchSource currJudgeSlideTouchSource;
    private Vector2 currJudgeSlidePosition;
    private TargetResult currJudgeSlideResult;
    private void judgeSlideMain(TouchSource touchSource, Vector2? touchPosition) {
        if (!this.waitSlideMove) {
            if (this.particles.QueueFirstOrDefalut is not SlideParticle lastParticle) {
                return;
            }
            TargetResult result = Judgment.JudgeSlide(lastParticle.Source!.StartTime, this.gameplayScreen.GameplayTrack.CurrentTime);
            if (result is not TargetResult.None) {
                this.particles.HideFirst(0.5f);

                this.waitSlideMove = true;
                this.currJudgeSlide = lastParticle;
                this.currJudgeSlidePosition = touchPosition!.Value;
                this.currJudgeSlideTouchSource = touchSource;
                this.currJudgeSlideResult = result;
            }
        } else if(touchSource == this.currJudgeSlideTouchSource) {
            Boolean moveSucceed;
            if(touchPosition.HasValue) {
                Vector2 offset = this.currJudgeSlidePosition - touchPosition.Value;
                switch (this.currJudgeSlide!.Direction) {
                    case SlidingDirection.Left: moveSucceed = offset.X > 0; break;
                    case SlidingDirection.Right: moveSucceed = offset.X < 0; break;
                    case SlidingDirection.Up: moveSucceed = offset.Y > 0; break;
                    case SlidingDirection.Down: moveSucceed = offset.Y < 0; break;
                    default: throw new NotImplementedException($"Unknown SlidingDirection {this.currJudgeSlide!.Direction}");
                }
            } else {
                moveSucceed = false;
            }

            this.gameplayScreen.ScoringCalculator.AddTarget(moveSucceed ? this.currJudgeSlideResult : TargetResult.Miss);

            this.waitSlideMove = false;
            this.currJudgeSlide!.Alpha = 0f;
            this.currJudgeSlide = null;
        }
    }

    private sealed partial class ParticleQueue : Container<ParticleBase> {
        private readonly List<ParticleBase> queue = [];

        public ParticleBase? QueueFirstOrDefalut {
            get {
                if (this.queue.Count > 0) {
                    return this.queue[0];
                } else {
                    //throw new InvalidOperationException("The queue is empty.");
                    return null;
                }
            }
        }

        public override void Add(ParticleBase drawable) {
            base.Add(drawable);
            if (this.queue.IndexOf(drawable) < 0) {
                this.queue.Add(drawable);
            } else {
                throw new InvalidOperationException("You can't add the same particle twice.");
            }
        }

        public override Boolean Remove(ParticleBase drawable, Boolean disposeImmediately) {
            var result = base.Remove(drawable, disposeImmediately);
            this.queue.Remove(drawable);
            return result;
        }

        public void HideFirst(Single alpha = 0) {
            if (this.queue.Count > 0) {
                ParticleBase first = this.queue[0];
                first.Alpha = alpha;
                this.queue.RemoveAt(0);
            }
        }
    }

    // call this
    // if (the last particle is Blink)
    // when (TouchEnter && isNewTouch)
    //private TargetResult JudgePress() {
    //    if (this.particles.Count is 0) {
    //        return TargetResult.None;
    //    }
    //    //TargetResult result = Judgment.JudgeBlink(this.particles[0].Source!.StartTime, this.gameplayScreen.GameplayTrack.CurrentTime);
    //    //if (result is not TargetResult.None) {
    //    //    this.particles.Remove(this.particles[0], false);
    //    //}
    //    //return result;
    //}

    //// call this
    //// if (the last particle is Storke && touches.Count > 0)
    //// when (TouchEnter and touches.Count from 0 become more || Judge range changed)
    //private TargetResult JudgeStroke() {
    //    TargetResult result = Judgment.JudgeStroke(this.particles[^1].Source!.StartTime, this.gameplayScreen.GameplayTrack.CurrentTime);
    //    if (result is TargetResult.MaxPerfect) {
    //        this.particles.Remove(this.particles[1], false);
    //    }
    //    return result;
    //}

    // call this
    // if (the last particle is Slide)
    // when (TouchEnter && isNewTouch && SlideDirection = Self.Direction)
    // private TargetResult JudgeSlide() {
    //     return TargetResult.Miss;
    // }

    // call this
    // if (the last particle is Press)
    // when (Begin: TouchEnter && isNewTouch) && (End: Judge range become MaxPerfect || TouchLeave and touches.Count become 0)
    // private TargetResult JudgePress() {
    //     return TargetResult.Miss;
    // }

    #endregion Judge

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
                foreach (TimeSourceWithHit item in value.HitObjects.Span) {
                    this.lifetimeEntryManager.AddEntry(new ParticleLifetimeEntry(item));
                }
            }
            base.Source = value;
        }
    }

    private void lifetimeEntryManager_EntryBecameAlive(LifetimeEntry obj) {
        var entry = (ParticleLifetimeEntry)obj;

        entry.Drawable = entry.Source switch {
            BlinkParticleSource blink => this.blinkParticlePool.Get(p => {
                p.Y = visual_orbit_out_of_top;
            }),
            PressParticleSource press => this.pressParticlePool.Get(p => {
                p.Y = visual_orbit_out_of_top;
                p.Height = (Single)((visual_orbit_offset - visual_orbit_out_of_top) * (press.EndTime - press.StartTime) / this.particleFallingTime);
            }),
            SlideParticleSource slide => this.slideParticlePool.Get(p => {
                p.Y = visual_orbit_out_of_top;
                p.Direction = slide.Direction;
            }),
            StrokeParticleSource stroke => this.strokeParticlePool.Get(p => {
                p.Y = visual_orbit_out_of_top;
            }),
            _ => throw new NotImplementedException(),
        };

        entry.Drawable.Source = entry.Source;
        this.particles.Add(entry.Drawable);
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
            var currTime = this.gameplayScreen.GameplayTrack.CurrentTime;
            // Time to fade out
            var startTime = currTime - this.particleFadingTime;
            // Time to fall down
            var endTime = currTime + this.particleFallingTime;
            result |= this.lifetimeEntryManager.Update(startTime, endTime);
        }
        return result;
    }

    #endregion Poolable

    protected override void PrepareForUse() {
        base.PrepareForUse();
        this.gameplayScreen.TouchUpdate += this.OnTouchChecked;
    }

    protected override void FreeAfterUse() {
        this.gameplayScreen.TouchUpdate -= this.OnTouchChecked;
        base.FreeAfterUse();
    }

    #region Touch

    private HashSet<TouchSource> touches = [];

    protected void OnTouchChecked(TouchSource source, Boolean? isNewTouch) {
        if (isNewTouch is null) {
            this.TouchLeave(source);
            return;
        }
        var isHovered = this.ScreenSpaceDrawQuad.Contains(this.gameplayScreen.TouchPositions[source]);
        var isEntered = this.touches.Contains(source);

        switch (isHovered, isEntered) {
            case (true, false):
                this.TouchEnter(source, isNewTouch.Value);
                break;

            case (false, true):
                this.TouchLeave(source);
                break;
        }
    }

    protected void TouchEnter(TouchSource source, Boolean isTouchDown) {
        this.touches.Add(source);

        if (isTouchDown) {
            this.judgeBlinkMain();
        }
    }

    // FIXME: This method is for test only.
    protected override Boolean OnClick(ClickEvent e) {
        this.TouchEnter(0, true);
        return false;
    }

    protected void TouchLeave(TouchSource source) {
        this.touches.Remove(source);
    }

    #endregion Touch
}
