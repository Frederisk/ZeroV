using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace ZeroV.Game.Screens.Preference.ListItems;

public partial class DropdownListItem<T> : BasePreferenceListItem<T> {
    public override Bindable<T> Current => this.dropdown.Current;

    private BasicDropdown<T> dropdown = null!;

    protected override Drawable LoadInputController() {
        this.dropdown = new BasicDropdown<T> {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
        };
        return this.dropdown;
    }
}
