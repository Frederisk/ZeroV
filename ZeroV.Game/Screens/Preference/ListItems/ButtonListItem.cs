using System;

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;

using osuTK;

namespace ZeroV.Game.Screens.Preference.ListItems;

public partial class ButtonListItem<T> : BasePreferenceListItem<T> {
    public override Bindable<T> Current => this.current;
    public required Action Action { get; set; }

    private readonly Bindable<T> current = new();

    private BasicButton button = null!;

    protected override Drawable LoadInputController() {
        this.button = new BasicButton {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Size = new Vector2(100, 37),
            Text = this.FormattingDisplayText(this.current.Value),
            Action = this.Action,
        };
        this.current.ValueChanged += this.UpdateDisplayText;
        return this.button;
    }

    protected override void UpdateDisplayText(ValueChangedEvent<T> value) =>
        this.button.Text = this.FormattingDisplayText(value.NewValue);
}
