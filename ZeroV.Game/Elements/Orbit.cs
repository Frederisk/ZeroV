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
    private Double particleFadingTime = TimeSpan.FromSeconds(1.2).TotalMilliseconds;

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
        base.Height = 768;
        base.Y = 0;
        this.Alpha = 0.9f;
    }

    private ReadOnlyMemory<OrbitSource.KeyFrame> keyFrames;

    protected override void Update() {
        base.Update();
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

        if (this.touches.Count > 0) {
            this.judgeStrokeMain();
        }
        this.judgeMissMain();
    }

    #region Judge

    private Boolean judgeBlinkMain() {
        if (this.particles.GetFirstOrDefaultFromQueue() is not BlinkParticle lastParticle) {
            return false;
        }
        TargetResult result = Judgment.JudgeBlink(lastParticle.Source!.StartTime, this.gameplayScreen.GameplayTrack.CurrentTime);
        if (result is not TargetResult.None) {
            this.particles.HideFromQueueAt(0);
        }
        this.gameplayScreen.ScoringCalculator.AddTarget(result);
        return true;
    }

    private Boolean waitSlideMove = false;
    private SlideParticle? currJudgeSlide;
    private TouchSource currJudgeSlideTouchSource;
    private TargetResult currJudgeSlideResult;
    private Boolean judgeSlideMain(Boolean isNewTouch, TouchSource touchSource, Vector2? delta) {
        if (!this.waitSlideMove && isNewTouch) {
            if (this.particles.GetFirstOrDefaultFromQueue() is not SlideParticle lastParticle) {
                return false;
            }
            TargetResult result = Judgment.JudgeSlide(lastParticle.Source!.StartTime, this.gameplayScreen.GameplayTrack.CurrentTime);
            if (result is not TargetResult.None) {
                this.particles.HideFromQueueAt(0, 0.5f);

                Logger.Log("waite slide move");
                this.waitSlideMove = true;
                this.currJudgeSlide = lastParticle;
                this.currJudgeSlideTouchSource = touchSource;
                this.currJudgeSlideResult = result;
            }
            return true;
        } else if (this.waitSlideMove && touchSource == this.currJudgeSlideTouchSource) {
            Boolean moveSucceed;
            if (delta.HasValue) {
                Vector2 offset = delta.Value;
                switch (this.currJudgeSlide!.Direction) {
                    case SlidingDirection.Left: moveSucceed = offset.X < 0; break;
                    case SlidingDirection.Right: moveSucceed = offset.X > 0; break;
                    case SlidingDirection.Up: moveSucceed = offset.Y < 0; break;
                    case SlidingDirection.Down: moveSucceed = offset.Y > 0; break;
                    default: throw new NotImplementedException($"Unknown SlidingDirection {this.currJudgeSlide!.Direction}");
                }
            } else {
                moveSucceed = false;
            }
            Logger.Log($"{moveSucceed}");
            this.gameplayScreen.ScoringCalculator.AddTarget(moveSucceed ? this.currJudgeSlideResult : TargetResult.Miss);

            this.waitSlideMove = false;
            this.currJudgeSlide!.Alpha = 0f;
            this.currJudgeSlide = null;
            return true;
        }
        return false;
    }


    private void judgeStrokeMain() {
        // Outdated code, judgement in reverse order.
        // for (var i = this.particles.Queue.Count - 1; i >= 0; i--) {
        //     if (this.particles.Queue[i] is not StrokeParticle particle) {
        //         break;
        //     }
        //     TargetResult result = Judgment.JudgeStroke(particle.Source!.StartTime, this.gameplayScreen.GameplayTrack.CurrentTime);
        //     if (result is not TargetResult.None) {
        //         this.particles.HideFromQueueAt(i); // Note the count of particles will decrease here.
        //     }
        //     this.gameplayScreen.ScoringCalculator.AddTarget(result);
        // }

        for (var i = 0; i < this.particles.Queue.Count; i++) {
            if (this.particles.Queue[i] is not StrokeParticle particle) {
                break; // The judgement is terminated when there are any other particles in the front of the stroke particle that cannot be judged. This is to avoid the strange flickering of target results and reduce the judgment performance load.
            }
            TargetResult result = Judgment.JudgeStroke(particle.Source!.StartTime, this.gameplayScreen.GameplayTrack.CurrentTime);
            if (result is not TargetResult.None) {
                this.particles.HideFromQueueAt(i); // Note the count of particles will decrease here.
                i--; // The index should be decreased. because the next particle will move incrementally to the current index due to the removal of the current particle.
            }
            this.gameplayScreen.ScoringCalculator.AddTarget(result);
        }
    }

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

    private void judgeMissMain() {
        ParticleBase? first = this.particles.GetFirstOrDefaultFromQueue();
        if (first is not null && this.gameplayScreen.GameplayTrack.CurrentTime - first.Source!.EndTime > 1000) {
            this.particles.HideFromQueueAt(0);
            this.gameplayScreen.ScoringCalculator.AddTarget(TargetResult.Miss);
        }
    }

    private sealed partial class ParticleQueue : Container<ParticleBase> {
        private readonly List<ParticleBase> queue = [];

        public IReadOnlyList<ParticleBase> Queue => this.queue;

        /// <summary>
        /// Get the first particle in the queue.
        /// </summary>
        /// <returns>The first particle in the queue. If the queue is empty, return <see langword="null"/>.</returns>
        public ParticleBase? GetFirstOrDefaultFromQueue() => this.queue.Count > 0 ? this.queue[0] : null;

        public override void Add(ParticleBase drawable) {
            base.Add(drawable);
            if (this.queue.IndexOf(drawable) < 0) {
                this.queue.Add(drawable);
            } else {
                throw new InvalidOperationException("You can't add the same particle in the orbit twice.");
            }
        }

        public override Boolean Remove(ParticleBase drawable, Boolean disposeImmediately) {
            var result = base.Remove(drawable, disposeImmediately);
            _ = this.queue.Remove(drawable);
            return result;
        }

        /// <summary>
        /// Hide and Remove the particle at the specified index in the queue.
        /// Note the particle in the <see cref="Container{T}.Children"/> will not be removed.
        /// </summary>
        /// <param name="index">The zero-based index of the particle to hide.</param>
        /// <param name="alpha">The alpha value of the particle to hide.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void HideFromQueueAt(Int32 index, Single alpha = 0) {
            if (index < 0 || index >= this.queue.Count) {
                throw new ArgumentOutOfRangeException(nameof(index), index, "The index is out of range.");
            }
            ParticleBase particle = this.queue[index];
            particle.Alpha = alpha;
            this.queue.RemoveAt(index);
        }
    }

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

    protected void OnTouchChecked(TouchSource source, Boolean? isNewTouch) {
        if (isNewTouch is null) {
            this.OnTouchLeave(source);
            return;
        }
        var isHovered = this.ScreenSpaceDrawQuad.Contains(this.gameplayScreen.TouchPositions[source]);
        var isEntered = this.touches.Contains(source);

        switch (isHovered, isEntered) {
            case (true, false):
                this.OnTouchEnter(source, isNewTouch.Value);
                break;

            case (false, true):
                this.OnTouchLeave(source);
                break;
        }
    }

    protected void OnTouchEnter(TouchSource source, Boolean isTouchDown) {
        this.touches.Add(source);

        this.judgeStrokeMain();
    }

    protected override Boolean OnTouchDown(TouchDownEvent e) {
        // base.OnTouchDown(e); // only return false
        if (this.judgeBlinkMain()) { }
        else if (this.judgeSlideMain(true, e.Touch.Source, null)) { }
        return false;
    }

    protected override void OnTouchMove(TouchMoveEvent e) {
        // base.OnTouchMove(e); // do nothing
        this.judgeSlideMain(false, e.Touch.Source, e.Delta);
    }

    protected override void OnTouchUp(TouchUpEvent e) {
        // base.OnTouchUp(e); // do nothing
        this.judgeSlideMain(false, e.Touch.Source, null);
    }

    protected void OnTouchLeave(TouchSource source) {
        this.touches.Remove(source);
    }

    #endregion Touch
}
