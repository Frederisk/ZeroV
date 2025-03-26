using System;

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.UI.Scrolling;

using osuTK.Graphics;

namespace osu.Game.Rulesets.ZeroV.UI;

[Cached]
public partial class ZeroVPlayfield : ScrollingPlayfield {
    public const Single LANE_HEIGHT = 70;

    public const Int32 LANE_COUNT = 6;

    public BindableInt CurrentLane => this.zerov.LanePosition;

    private ZeroVCharacter zerov;

    [BackgroundDependencyLoader]
    private void load() {
        this.AddRangeInternal(new Drawable[] {
            new LaneContainer {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Child = new Container {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding {
                        Left = 200,
                        Top = LANE_HEIGHT / 2,
                        Bottom = LANE_HEIGHT / 2
                    },
                    Children = new Drawable[] {
                        this.HitObjectContainer,
                        this.zerov = new ZeroVCharacter {
                            Origin = Anchor.Centre,
                        },
                    }
                },
            },
        });
    }

    private partial class LaneContainer : BeatSyncedContainer {
        private OsuColour colours;
        private FillFlowContainer fill;

        private readonly Container content = new() {
            RelativeSizeAxes = Axes.Both,
        };

        protected override Container<Drawable> Content => this.content;

        [BackgroundDependencyLoader]
        private void load(OsuColour colours) {
            this.colours = colours;

            this.InternalChildren = new Drawable[] {
                this.fill = new FillFlowContainer {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Colour = this.colours.BlueLight,
                    Direction = FillDirection.Vertical,
                },
                this.content,
            };

            for (var i = 0; i < LANE_COUNT; i++) {
                this.fill.Add(new Lane {
                    RelativeSizeAxes = Axes.X,
                    Height = LANE_HEIGHT,
                });
            }
        }

        private partial class Lane : CompositeDrawable {
            public Lane() {
                this.InternalChildren = new Drawable[] {
                    new Box {
                        Colour = Color4.White,
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Height = 0.95f,
                    },
                };
            }
        }

        protected override void OnNewBeat(Int32 beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, ChannelAmplitudes amplitudes) {
            if (effectPoint.KiaiMode) {
                this.fill.FlashColour(this.colours.PinkLight, 800, Easing.In);
            }
        }
    }
}
