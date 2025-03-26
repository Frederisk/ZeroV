using System;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

using ZeroV.Game.Configs;
using ZeroV.Game.Graphics;

namespace ZeroV.Game.Screens.Preference.ListItems;

public abstract partial class BasePreferenceListItem<T> : CompositeDrawable {
    public required ZeroVSetting Setting { get; init; }

    public required String LabelText { get; init; }

    public abstract Bindable<T> Current { get; }

    [BackgroundDependencyLoader]
    private void load(ZeroVConfigManager configManager) {
        this.RelativeSizeAxes = Axes.X;
        this.AutoSizeAxes = Axes.Y;
        this.InternalChild = new Container {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Children = [
                new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.DarkGray,
                },
                new ZeroVSpriteText {
                    Padding = new(24),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Text = this.LabelText,
                    FontSize = 30,
                },
                new Container {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Padding = new(24),
                    Child = this.LoadInputController(),
                },
            ],
        };
        configManager.BindWith(this.Setting, this.Current);
    }

    protected abstract Drawable LoadInputController();
}
