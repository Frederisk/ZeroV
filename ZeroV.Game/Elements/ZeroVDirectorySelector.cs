using System;
using System.IO;

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace ZeroV.Game.Elements;

public partial class ZeroVDirectorySelector : DirectorySelector {

    public ZeroVDirectorySelector(String? initialPath = null) : base(initialPath) {
        this.ShowHiddenItems.Value = true;
    }

    protected override DirectorySelectorBreadcrumbDisplay CreateBreadcrumb() => new BasicDirectorySelectorBreadcrumbDisplay();

    protected override DirectorySelectorDirectory CreateDirectoryItem(DirectoryInfo directory, String? displayName = null) => new BasicDirectorySelectorDirectory(directory, displayName);

    protected override DirectorySelectorDirectory CreateParentDirectoryItem(DirectoryInfo directory) => new BasicDirectorySelectorParentDirectory(directory);

    protected override ScrollContainer<Drawable> CreateScrollContainer() => new BasicScrollContainer();

    protected override void NotifySelectionError() => this.FlashColour(Colour4.Red, 300);
}
