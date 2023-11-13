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

public record Note(Double Time);

internal partial class Orbit : CompositeDrawable {
    private const Single visual_orbit_offset = -50;
    private BufferedContainer? container;
    public Box? TouchSpace { get; private set; }
    private Box? innerBox;
    private Box? innerLine;

    private Container<HittableParticle>? particles;

    private Int32 touchCount;
    public Boolean IsTouching => this.touchCount > 0;

    private Colour4[] colors;
    private Queue<HittableParticle> particleQueue;
    private Queue<Note> notes;
    private Double? lastTouchDownTime;
    private Double starTimeOffset = TimeSpan.FromSeconds(3).TotalMilliseconds;
    private Vector2 startPoint = new Vector2(0, -768);
    private Vector2 endPoint = new Vector2(0, 0);

    public new Single Y => base.Y;
    public new required Single X { get => base.X; set => base.X = value; }
    public new Single Height => base.Height;
    public new required Single Width { get => base.Width; set => base.Width = value; }

    public Orbit() {
        //this.AutoSizeAxes = Axes.Both;
        this.Origin = Anchor.BottomCentre;
        this.Anchor = Anchor.BottomCentre;
        this.colors = new Colour4[] {
            Colour4.White,
            Colour4.Red,
            Colour4.Orange,
            Color4.Yellow,
            Color4.Green,
            Color4.Cyan,
            Color4.Blue,
            Color4.Purple,
        };
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
        this.particles = new Container<HittableParticle>() {
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

        // TODO: For Test
        this.particleQueue = new Queue<HittableParticle>();
        this.particleQueue.Enqueue(new BlinkParticle(this) { Position = this.startPoint });
        this.particleQueue.Enqueue(new BlinkParticle(this) { Position = this.startPoint });
        this.particleQueue.Enqueue(new BlinkParticle(this) { Position = this.startPoint });

        foreach (HittableParticle item in this.particleQueue) {
            this.Add(item);
        }
    }

    protected override void Update() {
        base.Update();

        var time = this.Time.Current;

        if (this.lastTouchDownTime.HasValue) {
            if (this.notes.TryPeek(out Note? note)) {
                if (time - note.Time < 1000) {
                    // TODO: judgment note touch
                    this.notes.Dequeue();

                    HittableParticle particle = this.particleQueue.Dequeue();
                    particle.Position = this.startPoint;
                }
            }

            this.lastTouchDownTime = null;
        }

        foreach ((HittableParticle particle, Note note) in this.particleQueue.Zip(this.notes)) {
            if (note.Time - this.starTimeOffset > time) {
                break;
            }

            particle.Position =
                Interpolation.ValueAt(time, this.startPoint, this.endPoint,
                note.Time - this.starTimeOffset, note.Time);

            if (note.Time < time) {
                // TODO: Select a collection where objects can be removed during iteration.
            }
        }
    }

    public void Add(HittableParticle a) {
        this.particles?.Add(a);
    }

    public void Remove(HittableParticle a) {
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
