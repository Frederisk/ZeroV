using System;
using System.Collections.Generic;
using System.Linq;

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Utils;

using osuTK;
using osuTK.Graphics;

using ZeroV.Game.Elements.Particles;

namespace ZeroV.Game.Elements;

// FIXME: Remove it.
public record Note(Double Time);

/// <summary>
/// Orbits that carry particles. It's also the main interactive object in this game.
/// </summary>
internal partial class Orbit : CompositeDrawable {

    [Resolved]
    private ZeroVGameSettings settings { get; set; } = null!;

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

    /// <summary>
    /// The posotion of the bottom of the screen.
    /// </summary>
    private const Single visual_orbit_bottom = 0;

    /// <summary>
    /// The position beyond the Y-axis at the bottom of visible orbit.
    /// </summary>
    private const Single visual_orbit_out_of_bottom = visual_orbit_bottom + visual_half_of_particle_size;

    private BufferedContainer? container;

    /// <summary>
    /// The space used to touch judgment.
    /// </summary>
    public Box? TouchSpace { get; private set; }
    private Box? innerBox;
    private Box? innerLine;

    private Container<ParticleBase>? particles;

    private Int32 touchCount;
    public Boolean IsTouching => this.touchCount > 0;

    private Queue<ParticleBase> particleQueue;
    private Queue<Note> notes;

    private Double? lastTouchDownTime;

    private Colour4[] colors;

    // FIXME: These properties are redundant. In the future, they will be obtained by some fade-in animations.
    public new Single Y => base.Y;
    public new required Single X { get => base.X; set => base.X = value; }
    public new Single Height => base.Height;
    public new required Single Width { get => base.Width; set => base.Width = value; }

    public Orbit() {
        //this.AutoSizeAxes = Axes.Both;
        this.Origin = Anchor.BottomCentre;
        this.Anchor = Anchor.BottomCentre;
        // FIXME: Just for test, remove it.
        this.colors = [
            Colour4.White,
            Colour4.Red,
            Colour4.Orange,
            Color4.Yellow,
            Color4.Green,
            Color4.Cyan,
            Color4.Blue,
            Color4.Purple,
        ];
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
            Colour = Colour4.Yellow,
            RelativeSizeAxes = Axes.Both,
        };
        this.innerBox = new Box {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Colour = Colour4.Azure,
            RelativeSizeAxes = Axes.Both,
            // Size = new Vector2(5000,768-50),
            // Position = new Vector2(0, -50),
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
            // Position = new Vector2(0, visual_orbit_offset),
        };
        this.particles = new Container<ParticleBase>() {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
        };
        this.container = new BufferedContainer() {
            RelativeSizeAxes = Axes.Both,
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Children = new Drawable[] {
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
                    Children = new Drawable[] {
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
                    }
                }
            }
        };
        this.InternalChild = this.container;

        // FIXME: For Test
        this.particleQueue = new Queue<ParticleBase>();
        this.particleQueue.Enqueue(new BlinkParticle(this) { Y = visual_orbit_out_of_top });
        this.particleQueue.Enqueue(new BlinkParticle(this) { Y = visual_orbit_out_of_top });
        this.particleQueue.Enqueue(new BlinkParticle(this) { Y = visual_orbit_out_of_top });

        foreach (ParticleBase item in this.particleQueue) {
            this.Add(item);
        }
    }

    protected override void Update() {
        // This method is once-per-frame update.
        // For a music game, we may need higher-speed judgment and logic processing.
        // TODO: So, here wo should only deal with something that don't need to be updated that frequently.
        // Eg. It's redundant to repeat the movement of an Drawable item multiple times within a frame. But it makes sense for touch judgments.
        base.Update();

        var time = this.Time.Current;
        var startTimeOffset = this.settings.StartTimeOffset;

        // TODO: Maybe we can make it faster?
        if (this.lastTouchDownTime.HasValue) {
            if (this.notes.TryPeek(out Note? note)) {
                var touchOffset = Double.Abs(time - note.Time);
                if (touchOffset <= this.settings.GoodOffset) {

                    // TODO: Use Enum
                    var _ = touchOffset switch {
                        _ when touchOffset <= this.settings.MaxPerfectOffset => "MaxPerfect",
                        _ when touchOffset <= this.settings.PerfectOffset => "Perfect",
                        _ => "Good"
                    };

                    // TODO: show judgment result and count.

                    this.notes.Dequeue();

                    ParticleBase particle = this.particleQueue.Dequeue();
                    particle.Y = visual_orbit_out_of_top;
                }
            }

            this.lastTouchDownTime = null;
        }

        // TODO: `Zip` is so solw (Because there are too many bounds checks insider this method), stop using it.
        foreach ((ParticleBase particle, Note note) in this.particleQueue.Zip(this.notes)) {
            if (note.Time - startTimeOffset > time) {
                break;
            }
            // The Particle falls to the judgment line.
            if (time < note.Time) {
                particle.Y =
                    Interpolation.ValueAt(time, visual_orbit_out_of_top, visual_orbit_offset,
                    note.Time - startTimeOffset, note.Time);
            }

            if (note.Time < time) {
                particle.Y =
                    Interpolation.ValueAt(time, visual_orbit_offset, 0,
                    note.Time, note.Time + this.settings.GoodOffset);
                particle.Alpha =
                    Interpolation.ValueAt(time, 1f, 0f, note.Time, note.Time + this.settings.GoodOffset);
            }

            if (note.Time + this.settings.GoodOffset < time) {
                // TODO: Select a collection where objects can be removed during iteration.

            }
        }
    }

    public void Add(ParticleBase a) {
        this.particles?.Add(a);
    }

    public void Remove(ParticleBase a) {
        this.particles?.Remove(a, true);
    }

    private void updateColor() {
        var colorIndex = this.touchCount % 8;
        this.innerBox!.Colour = this.colors[colorIndex];
    }

    public void TouchEnter(Boolean isTouchDown) {
        if (isTouchDown) {
            this.lastTouchDownTime = this.Time.Current;
        }

        this.touchCount++;
        this.updateColor();
    }

    public void TouchLeave() {
        this.touchCount--;
        this.updateColor();
    }
}
