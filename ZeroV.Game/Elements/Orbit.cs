using System;
using System.Collections.Generic;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

using osuTK;
using osuTK.Graphics;

using ZeroV.Game.Elements.Particles;

namespace ZeroV.Game.Elements;

internal partial class Orbit : CompositeDrawable {
    private BufferedContainer? container;
    private Box? touchSpace;
    private Box? innerBox;
    private Box? innerLine;

    private Container? particlesContainer;

    private Int32 touchCount;
    private Colour4[] colors;

    public Orbit() {
        this.AutoSizeAxes = Axes.Both;
        this.Origin = Anchor.BottomCentre;
        this.Anchor = Anchor.BottomCentre;
        this.colors = new Colour4[] {
            Colour4.Black,
            Colour4.Red,
            Colour4.Orange,
            Color4.Yellow,
            Color4.Green,
            Color4.Cyan,
            Color4.Blue,
            Color4.Purple,
        };
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.touchSpace = new Box {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Colour = Colour4.Yellow,
            Size = new Vector2(100, 768),
        };
        this.innerBox = new Box {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Colour = Colour4.Azure,
            Size = new Vector2(100, 768 - 50),
            Position = new Vector2(0, -50),
        };
        this.innerLine = new Box {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Colour = Colour4.Black,
            EdgeSmoothness = new Vector2(1, 0),
            Size = new Vector2(1, 768 - 50),
            Position = new Vector2(0, -50),
        };
        this.particlesContainer = new Container() {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
        };
        this.container = new BufferedContainer() {
            AutoSizeAxes = Axes.Both,
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Children = new Drawable[] {
                this.touchSpace,
                this.innerBox,
                this.innerLine,
                this.particlesContainer,
                // From osu.Game.Rulesets.Mania.UI.PlayfieldCoveringWrapper
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
                            Height = 0.25f,
                            Y = 0.05f,
                            Colour = ColourInfo.GradientVertical(
                                Color4.White.Opacity(1f),
                                Color4.White.Opacity(0f)
                            )
                        },
                        new Box {
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            RelativeSizeAxes = Axes.Both,
                            Height = 0.05f
                        }
                    }
                }
            }
        };
        this.InternalChild = this.container;

        // TODO: For Test
        this.Add(new BlinkParticle(this) {
            Position = new Vector2(0, -128)
        });
    }

    public void Add(HitableParticle a) {
        this.particlesContainer?.Add(a);
    }

    public void Remove(HitableParticle a) {
        this.particlesContainer?.Remove(a, true);
    }

    private void updateColor() {
        var colorIndex = this.touchCount % 8;
        this.innerBox!.Colour = this.colors[colorIndex];
    }

    public void TouchEnter() {
        this.touchCount++;
        this.updateColor();
    }

    public void TouchLeave() {
        this.touchCount--;
        this.updateColor();
    }
}
