using System;

using osu.Framework.Bindables;
using osu.Framework.Graphics;

using ZeroV.Game.Elements.Buttons;

namespace ZeroV.Game.Screens.Preference.ListItems;

public partial class CheckBoxListItem : BasePreferenceListItem<Boolean> {
    public override Bindable<Boolean> Current => this.switchButton.Current;

    private SwitchButton switchButton = null!;

    protected override Drawable LoadInputController() {
        this.switchButton = new SwitchButton {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
        };
        return this.switchButton;
    }
}
