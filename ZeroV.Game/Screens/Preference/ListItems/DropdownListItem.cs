using System;

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace ZeroV.Game.Screens.Preference.ListItems;

public partial class DropdownListItem<TValue, TSetting> : BasePreferenceListItem<TValue, TSetting> where TSetting : struct, Enum {
    public override Bindable<TValue> Current => this.dropdown.Current;

    private BasicDropdown<TValue> dropdown = null!;

    protected override Drawable LoadInputController() {
        this.dropdown = new BasicDropdown<TValue> {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
        };
        return this.dropdown;
    }
}
