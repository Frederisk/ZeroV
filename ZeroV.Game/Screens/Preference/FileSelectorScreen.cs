using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Logging;

using osuTK;

using ZeroV.Game.Configs;
using ZeroV.Game.Data.IO;
using ZeroV.Game.Elements;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens.Preference;

public partial class FileSelectorScreen : BaseUserInterfaceScreen {
    private ZeroVFileSelector fileSelector = null!;

    [BackgroundDependencyLoader]
    private void load(ZeroVConfigManager configManager) {
        // String defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        this.fileSelector = new ZeroVFileSelector(null, ZeroVPath.VALID_BEATMAP_FILE_EXTENSIONS) {
            RelativeSizeAxes = Axes.Both,
        };

        this.fileSelector.CurrentFile.ValueChanged += (value) => {
            if (value.NewValue is null) {
                return;
            }
            // TODO: Display file info.
        };

        DirectoryInfo beatmapStorageDirInfo = new(configManager.Get<String>(ZeroVSetting.BeatmapStoragePath));
        this.InternalChild = new FillFlowContainer {
            Direction = FillDirection.Vertical,
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Children = [
                new BackButton(this),
                new DrawSizePreservingFillContainer() {
                    TargetDrawSize = new Vector2(ZeroVMath.SCREEN_DRAWABLE_X / 2, ZeroVMath.SCREEN_DRAWABLE_Y / 2),
                    RelativeSizeAxes = Axes.X,
                    Height = 660,
                    Child = this.fileSelector,
                },
                new BasicButton {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Size = new Vector2(180, 64),
                    Text = "Import",
                    Action = () => {
                        FileInfo archiveFile = this.fileSelector.CurrentFile.Value;
                        if (archiveFile is null
                        || !archiveFile.Exists) {
                            return;
                        }
                        try {
                            ArchiveProcessor.ExtractZeroVFile(archiveFile, beatmapStorageDirInfo);
                        } catch (Exception ex) {
                            Logger.Error(ex, "An unexpected exception was encountered while extracting the beatmap archive.");
                        }
                    }
                },
                new BasicButton {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Size = new Vector2(360, 64),
                    Text = "Import all in current folder",
                    Action = () => {
                        DirectoryInfo archiveDirInfo = this.fileSelector.CurrentPath.Value;
                        if (archiveDirInfo is null
                        || !archiveDirInfo.Exists) {
                            return;
                        }
                        DirectoryInfo storageInfo = new DirectoryInfo(configManager.Get<String>(ZeroVSetting.BeatmapStoragePath));
                        IEnumerable<FileInfo> archiveFiles = archiveDirInfo.GetFiles().Where(file =>
                            ZeroVPath.VALID_BEATMAP_FILE_EXTENSIONS.Contains(file.Extension)
                            );
                        foreach (FileInfo archiveFile in archiveFiles) {
                            try {
                                ArchiveProcessor.ExtractZeroVFile(archiveFile, storageInfo);
                            } catch (Exception ex) {
                                Logger.Error(ex, $"An unexpected exception was encountered while extracting the beatmap archive: {archiveFile.FullName}");
                            }
                        }
                    }
                },
            ],
        };
    }
}
