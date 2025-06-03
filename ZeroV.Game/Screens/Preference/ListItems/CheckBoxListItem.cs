using System;

using osu.Framework.Bindables;
using osu.Framework.Graphics;

using ZeroV.Game.Elements.Buttons;

namespace ZeroV.Game.Screens.Preference.ListItems;

public partial class CheckBoxListItem<TValue, TSetting> : BasePreferenceListItem<TValue, TSetting> where TSetting : struct, Enum {
    public required Func<TValue, Boolean> ValueConverter { get; init; }
    public required Func<Boolean, TValue> InverseValueConverter { get; init; }

    private Bindable<TValue> current = new();
    public override Bindable<TValue> Current => this.current; // this.switchButton.Current;

    private SwitchButton switchButton = null!;

    protected override Drawable LoadInputController() {
        this.switchButton = new SwitchButton {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            //Current = new BindableBool {
            //    Value = this.ValueConverter(this.current.Value),
            //},
        };
        // Two-way binding, the ValueChanged here does not need to check if the value has changed, as the Bindable's internal logic has already handled it.
        // So there will be no oscillation back and forth in this case.
        this.current.ValueChanged += value => {
            this.switchButton.Current.Value = this.ValueConverter(value.NewValue);
        };
        this.switchButton.Current.ValueChanged += value => {
            this.current.Value = this.InverseValueConverter(value.NewValue);
        };
        // Nothing to do here, the switch button will update the current value directly.
        // this.current.ValueChanged += this.OnUpdateSettingDisplay;
        return this.switchButton;
    }
}
