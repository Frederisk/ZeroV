using System;
using System.Collections.Generic;

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Utils;

using osuTK;
using osuTK.Graphics;

using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Graphics;
using ZeroV.Game.Screens;

namespace ZeroV.Game.Elements;

// FIXME: Remove it.
public record Note(Double Time);

/// <summary>
/// OrbitSources that carry particles. It's also the main interactive object in this game.
/// </summary>
public partial class Orbit : ZeroVPoolableDrawable<OrbitSource> {

    /// <summary>
    /// The size of half the particle's Y-axis radius.
    /// </summary>
    private const Single visual_half_of_particle_size = 24;

    /// <summary>
    /// The position beyond the Y-axis at the top of visible orbit.
    /// </summary>
    /// <remarks>
    /// -768 - 24 = -792
    /// </remarks>
    private const Single visual_orbit_out_of_top = visual_orbit_top - visual_half_of_particle_size;

    /// <summary>
    /// The position of the top of visible orbit.
    /// </summary>
    private const Single visual_orbit_top = -768;

    /// <summary>
    /// The position of the bottom of visible orbit. It's also the offset of visible orbit relative to the screen.
    /// </summary>
    private const Single visual_orbit_offset = -50;

    ///// <summary>
    ///// The position of the bottom of the screen.
    ///// </summary>
    //private const Single visual_orbit_bottom = 0;

    ///// <summary>
    ///// The position beyond the Y-axis at the bottom of visible orbit.
    ///// </summary>
    //private const Single visual_orbit_out_of_bottom = visual_orbit_bottom + visual_half_of_particle_size;

    /// <summary>
    /// The container that contains all the elements of the orbit.
    /// </summary>
    /// <remarks>
    /// This field will never be null after <see cref="LoadComplete"/> has been called.
    /// It's a <see cref="BufferedContainer"/> because we need to use <see cref="BufferedContainer.Blending"/> to make the orbit partially hidden.
    /// </remarks>
    private BufferedContainer container = null!;

    /// <summary>
    /// The space used to touch judgment.
    /// </summary>
    /// <remarks>
    /// This field will never be null after <see cref="LoadComplete"/> has been called.
    /// </remarks>
    public Box TouchSpace { get; private set; } = null!;

    private Box innerBox = null!;
    private Box innerLine = null!;
    private Container<ParticleBase> particles = null!;

    private Queue<ParticleBase> particleQueue = null!;
    private Queue<Note> notes;
    private Double? lastTouchDownTime;

    //FIXME: Just for test, remove it.
    private Colour4[] colors = [
        Colour4.White,
        Colour4.Red,
        Colour4.Orange,
        Color4.Yellow,
        Color4.Green,
        Color4.Cyan,
        Color4.Blue,
        Color4.Purple,
    ];

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

        // FIXME: Just for test, remove it.
        base.Height = 768;
        base.Y = 0;
        this.Alpha = 0.9f;

        this.notes = new();
        this.notes.Enqueue(new Note(TimeSpan.FromSeconds(7).TotalMilliseconds));
        this.notes.Enqueue(new Note(TimeSpan.FromSeconds(8).TotalMilliseconds));
        this.notes.Enqueue(new Note(TimeSpan.FromSeconds(9).TotalMilliseconds));
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.TouchSpace = new Box {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            //Colour = Colour4.Yellow,
            Colour = Color4.Transparent,
            RelativeSizeAxes = Axes.Both,
        };
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
        this.particles = new Container<ParticleBase>() {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
        };
        this.container = new BufferedContainer() {
            RelativeSizeAxes = Axes.Both,
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Children = [
                this.TouchSpace,
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
                        // new Box {
                        //     Anchor = Anchor.TopLeft,
                        //     Origin = Anchor.TopLeft,
                        //     RelativeSizeAxes = Axes.Both,
                        //     RelativePositionAxes = Axes.Both,
                        //     Height = 1f - 0.05f - 0.25f,
                        //     Y = 0.05f + 0.25f,
                        //     Colour = Color4.White.Opacity(0.1f),
                        // }
                    ],
                }
            ]
        };
        this.InternalChild = this.container;

        // FIXME: For Test
        this.particleQueue = new Queue<ParticleBase>();
        this.particleQueue.Enqueue(new BlinkParticle(this) { Y = visual_orbit_out_of_top });
        this.particleQueue.Enqueue(new BlinkParticle(this) { Y = visual_orbit_out_of_top });
        this.particleQueue.Enqueue(new BlinkParticle(this) { Y = visual_orbit_out_of_top });

        foreach (ParticleBase item in this.particleQueue) {
            this.AddParticle(item);
        }
    }

    private ReadOnlyMemory<OrbitSource.KeyFrame> keyFrames;

    public override OrbitSource? Source {
        get => base.Source;
        set {
            if (value is not null) {
                this.keyFrames = value.KeyFrames;
            }
            base.Source = value;
        }
    }

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
    }

    protected override void PrepareForUse() {
        base.PrepareForUse();
        this.gameplayScreen.TouchUpdate += this.OnTouchChecked;
    }

    protected override void FreeAfterUse() {
        this.gameplayScreen.TouchUpdate -= this.OnTouchChecked;
        base.FreeAfterUse();
    }

    public void AddParticle(ParticleBase a) {
        this.particles.Add(a);
    }

    public void RemoveParticle(ParticleBase a) {
        this.particles.Remove(a, true);
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
        if (isTouchDown) {
            this.lastTouchDownTime = this.gameplayScreen.GameplayTrack.CurrentTime;
        }
        this.touches.Add(source);
        this.updateColor();
    }

    protected void TouchLeave(TouchSource source) {
        this.touches.Remove(source);
        this.updateColor();
    }

    private void updateColor() {
        var colorIndex = this.touches.Count % 8;
        this.innerBox.Colour = this.colors[colorIndex];
    }

    #endregion Touch
}
