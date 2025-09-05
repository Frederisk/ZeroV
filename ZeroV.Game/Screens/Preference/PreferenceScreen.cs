using System;

using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Configs;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Screens.Preference.ListItems;

namespace ZeroV.Game.Screens.Preference;

public partial class PreferenceScreen : Screen {

    [BackgroundDependencyLoader]
    private void load(ZeroVConfigManager zeroVConfigManager, FrameworkConfigManager frameworkConfigManager) {
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
                            new SliderBarListItem<Double, ZeroVSetting> {
                                ConfigManager = zeroVConfigManager,
                                Setting = ZeroVSetting.GamePlayParticleFallingTime,
                                LabelText = "Particle Falling Time",
                                MinValue = TimeSpan.FromSeconds(0.1).TotalMilliseconds,
                                MaxValue = TimeSpan.FromSeconds(5).TotalMilliseconds,
                                Precision = TimeSpan.FromSeconds(0.1).TotalMilliseconds,
                                FormattingDisplayText = value => $"{value} ms",
                            },
                            new ButtonListItem<Double, ZeroVSetting> {
                                ConfigManager = zeroVConfigManager,
                                Setting = ZeroVSetting.GlobalSoundOffset,
                                LabelText = "Setup Offset",
                                Action = () => this.Push(new OffsetScreen()),
                                FormattingDisplayText = value => $"{value} ms",
                            },
                            new ButtonListItem<String, ZeroVSetting> {
                                ConfigManager = zeroVConfigManager,
                                Setting = ZeroVSetting.BeatmapStoragePath,
                                LabelText = "Storage Path",
                                Action = () => this.Push(new DirectorySelectorScreen()),
                                FormattingDisplayText = _ => "Config",
                            },
                            new CheckBoxListItem<ExecutionMode, FrameworkSetting> {
                                ConfigManager= frameworkConfigManager,
                                Setting = FrameworkSetting.ExecutionMode,
                                ValueConverter = v => v is ExecutionMode.MultiThreaded,
                                InverseValueConverter = v => v ? ExecutionMode.MultiThreaded : ExecutionMode.SingleThread,
                                LabelText = "Enable Multi-Threaded Execution",
                            },
                            new ButtonListItem<Object, ZeroVSetting> {
                                ConfigManager = null,
                                Setting = default,
                                LabelText = "Import Beatmap",
                                Action = () => this.Push(new FileSelectorScreen()),
                                FormattingDisplayText = _ => "Import",
                            }
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
