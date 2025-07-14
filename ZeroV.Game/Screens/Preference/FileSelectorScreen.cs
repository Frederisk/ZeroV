using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

using osuTK;

using ZeroV.Game.Elements;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens.Preference;

public partial class FileSelectorScreen : BaseUserInterfaceScreen {
    private ZeroVFileSelector fileSelector = null!;

    [BackgroundDependencyLoader]
    private void load() {
        String defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        this.fileSelector = new ZeroVFileSelector(defaultPath, [".0vm"]) {
            RelativeSizeAxes = Axes.Both,
        };
        this.fileSelector.CurrentFile.ValueChanged += (value) => {
            if (value.NewValue is null) {
                return;
            }


        };
        //this.fileSelector.
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
                //new BasicButton {
                //    Anchor = Anchor.TopRight,
                //    Origin = Anchor.TopRight,
                //    Size = new Vector2(180, 64),
                //    Text = "Import",
                //    Action = () => {
                //        //if (this.fileSelector.CurrentFile.Value is null) {
                //        //    throw new Exception();
                //        //}
                //    }
                //}
            ],
        };
    }
}
