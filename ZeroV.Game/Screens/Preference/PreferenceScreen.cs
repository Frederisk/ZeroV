using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Configs;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Screens.Preference.ListItems;

namespace ZeroV.Game.Screens.Preference;

public partial class PreferenceScreen : Screen {

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Container {
                Y = 32,
                Padding = new MarginPadding(32),
                RelativeSizeAxes = Axes.Both,
                Child = new BasicScrollContainer<FillFlowContainer>(Direction.Vertical) {
                    RelativeSizeAxes = Axes.Both,
                    Child = new FillFlowContainer{
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 10),
                        Children = [
                            new SliderBarListItem<Double> {
                                Setting = ZeroVSetting.GamePlayParticleFallingTime,
                                LabelText = "Particle Falling Time",
                                MinValue = TimeSpan.FromSeconds(0.1).TotalMilliseconds,
                                MaxValue = TimeSpan.FromSeconds(5).TotalMilliseconds,
                                Precision = TimeSpan.FromSeconds(0.1).TotalMilliseconds,
                                FormattingDisplayText = value => $"{value} ms",
                            },
                            new ButtonListItem<Double> {
                                Setting = ZeroVSetting.GlobalSoundOffset,
                                LabelText = "Setup Offset",
                                Action = () => this.Push(new OffsetScreen()),
                                FormattingDisplayText = value => $"{value} ms",
                            },
                            new ButtonListItem<String> {
                                Setting = ZeroVSetting.BeatmapStoragePath,
                                LabelText = "Storage Path",
                                Action = () => this.Push(new DirectorySelectorScreen()),
                                FormattingDisplayText = value => "Config",
                            },
                        ],
                    },
                },
            },
            new BackButton(this) {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
            },
        ];
    }
}
