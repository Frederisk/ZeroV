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

/// <summary>
/// Represents a base class for preference list items that bind a specific setting to a configurable value.
/// </summary>
/// <remarks>
/// This class provides a framework for creating UI components that allow users to modify application settings.
/// It binds a setting to a value using a configuration manager,
/// and provides an abstract method for loading the input controller responsible for user interaction.
/// </remarks>
/// <typeparam name="TValue">The type of the value associated with the setting.</typeparam>
/// <typeparam name="TSetting">The type of the setting, constrained to an enumeration.</typeparam>
public abstract partial class BasePreferenceListItem<TValue, TSetting> : CompositeDrawable where TSetting : struct, Enum {

    /// <summary>
    /// Configuration manager to bind the setting to. If <see langword="null"/>, the setting won't be bound.
    /// </summary>
    public required IniConfigManager<TSetting>? ConfigManager { get; init; }

    /// <summary>
    /// The setting to be modified. If <see cref="ConfigManager"/> is <see langword="null"/>, this property is ignored.
    /// </summary>
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
        this.ConfigManager?.BindWith<TValue>(this.Setting, this.Current);
    }

    /// <summary>
    /// Loads the input controller for the derived class.
    /// </summary>
    /// <remarks>
    /// This method must be implemented by derived classes to provide a specific implementation of the input controller.
    /// The returned <see cref="Drawable"/> represents the input controller to be used.
    /// </remarks>
    /// <returns>A <see cref="Drawable"/> instance representing the input controller.</returns>
    protected abstract Drawable LoadInputController();

    protected virtual void OnUpdateSettingDisplay(ValueChangedEvent<TValue> value) {
    }

    public virtual Func<TValue, LocalisableString> FormattingDisplayText { get; init; } = value => value?.ToString() ?? "";
}
