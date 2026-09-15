using System;
using System.Collections.Generic;

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace ZeroV.Game.Screens.Preference.ListItems;

public partial class DropdownListItem<TValue, TSetting> : BasePreferenceListItem<TValue, TSetting> where TSetting : struct, Enum {
    public override Bindable<TValue> Current => this.dropdown.Current;

    public IEnumerable<TValue>? Items { get; init; }

    public Single DropdownWidth { get; init; } = 250;

    private BasicDropdown<TValue> dropdown = null!;

    protected override Drawable LoadInputController() {
        this.dropdown = new BasicDropdown<TValue> {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Width = this.DropdownWidth,
        };

        if (this.Items is not null) {
            this.dropdown.Items = this.Items;
        } else if (typeof(TValue).IsEnum) {
            this.dropdown.Items = (TValue[])Enum.GetValues(typeof(TValue));
        }

        return this.dropdown;
    }
}
