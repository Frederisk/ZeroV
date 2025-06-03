using System;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;

using ZeroV.Game.Graphics;

namespace ZeroV.Game.Screens.Preference.ListItems;

public abstract partial class BasePreferenceListItem<TValue, TSetting> : CompositeDrawable where TSetting : struct, Enum {
    public required IniConfigManager<TSetting> ConfigManager { get; init; }
    public required TSetting Setting { get; init; }

    public required String LabelText { get; init; }

    public abstract Bindable<TValue> Current { get; }

    [BackgroundDependencyLoader]
    private void load() {
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
        this.ConfigManager.BindWith<TValue>(this.Setting, this.Current);
    }

    protected abstract Drawable LoadInputController();

    protected virtual void OnUpdateSettingDisplay(ValueChangedEvent<TValue> value) { }

    public virtual Func<TValue, LocalisableString> FormattingDisplayText { get; init; } = value => value?.ToString() ?? "";
}
