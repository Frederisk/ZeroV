using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

using osuTK;
using osuTK.Graphics;

namespace ZeroV.Game.Elements.Particles;

public partial class SlideParticle : ParticleBase<SlideParticleSource> {
    public SlidingDirection Direction { get; init; }

    public SlideParticle(Orbit fatherOrbit) : base(fatherOrbit) {
    }

    [BackgroundDependencyLoader]
    private void load() {
        const Single inner_size = 0.8f;
        //const Single inner_diff = 1.0f - inner_size;
        this.InternalChildren = [
            //new InnerTriangle() {
            //    Size = new Vector2(100),
            //    Rotation = 90 * (Int32)this.Direction,
            //},
            new Container {
                Size = new Vector2(52 * Single.Sqrt(2)),
                Rotation = 90 * (Int32)this.Direction,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Child = new Container {
                    RelativeSizeAxes = Axes.Both,
                    Height = 0.5f,
                    Children = [
                        new Triangle {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black,
                        },
                        new Triangle {
                            //                 #
                            //               # y #
                            //             #   *   #
                            //           #   * | *   #
                            //         #   * inner *   #
                            //       #   *     |     *   #
                            //     #   * * * * * * * * *   #
                            //   #             x             #
                            // # # # # # # # # # # # # # # # # #
                            // z = 1 - inner_size;
                            // x + y = z;
                            // y = x * Sqrt(2);
                            // z = x + x * Sqrt(2);
                            // x = z / (1 + Sqrt(2));
                            // Result:
                            // y = (z * Sqrt(2)) / (1 + Sqrt(2));
                            // x = (1 - inner_size) / (1 + Sqrt(2));
                            // Note: -x = (inner_size - 1) / (1 + Sqrt(2));

                            // Anchor = Anchor.TopCentre,
                            // Origin = Anchor.TopCentre,
                            // Y = (innerDiff * Single.Sqrt(2))/ (1 + Single.Sqrt(2)),
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            Y = (inner_size - 1) / (1 + Single.Sqrt(2)),
                            RelativeSizeAxes = Axes.Both,
                            RelativePositionAxes = Axes.Both,
                            Size = new Vector2(inner_size),
                            Colour = this.Direction switch {
                                SlidingDirection.Left => Color4.SkyBlue,
                                SlidingDirection.Right => Color4.Yellow,
                                SlidingDirection.Up => Color4.GreenYellow,
                                SlidingDirection.Down => Color4.MediumPurple,
                                _ => throw new ArgumentOutOfRangeException(nameof(this.Direction), this.Direction, null),
                            },
                        },
                    ],
                },
            },
            new Diamond {
                Size = new Vector2(34),
                Colour = Color4.Black,
            },
        ];
    }

    public enum SlidingDirection : Int32 {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }
}
