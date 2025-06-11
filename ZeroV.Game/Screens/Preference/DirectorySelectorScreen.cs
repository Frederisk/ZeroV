using System;
using System.IO;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Configs;
using ZeroV.Game.Data.IO;
using ZeroV.Game.Elements;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens.Preference;

public partial class DirectorySelectorScreen : BaseUserInterfaceScreen {
    private Bindable<String> currentStoragePath = null!;

    private ZeroVDirectorySelector directorySelector = null!;

    [BackgroundDependencyLoader]
    private void load(ZeroVConfigManager configManager) {
        this.currentStoragePath = configManager.GetBindable<String>(ZeroVSetting.BeatmapStoragePath);
        this.directorySelector = new ZeroVDirectorySelector(this.currentStoragePath.Value) {
            RelativeSizeAxes = Axes.Both,
        };
        this.InternalChild = new FillFlowContainer {
            Direction = FillDirection.Vertical,
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Children = [
                new BackButton(this){},
                new DrawSizePreservingFillContainer() {
                    TargetDrawSize = new Vector2(ZeroVMath.SCREEN_DRAWABLE_X / 2, ZeroVMath.SCREEN_DRAWABLE_Y / 2),
                    RelativeSizeAxes = Axes.X,
                    Height = 660,
                    Child = this.directorySelector,
                },
                new FillFlowContainer {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Direction = FillDirection.Horizontal,
                    AutoSizeAxes = Axes.Both,
                    Children = [
                        new BasicButton {
                            Size = new Vector2(180, 64),
                            Text = "OK",
                            Action = () => {
                                if (FileManager.MoveFolder(new DirectoryInfo(this.currentStoragePath.Value), this.directorySelector.CurrentPath.Value)){
                                    // FIXME: Apply currentStoragePath change.
                                    //this.currentStoragePath.Value = this.directorySelector.CurrentPath.Value.FullName;
                                }
                            },
                        },
                        new BasicButton {
                            Size = new Vector2(180, 64),
                            Text = "Cancel",
                            Action = this.Exit,
                        },
                    ],
                },
            ],
        };
    }
}
