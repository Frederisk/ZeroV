using System;

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;

using osuTK;

namespace ZeroV.Game.Screens.Preference.ListItems;

public partial class ButtonListItem<TValue, TSetting> : BasePreferenceListItem<TValue, TSetting> where TSetting : struct, Enum {
    public override Bindable<TValue> Current => this.current;
    public required Action Action { get; set; }

    private readonly Bindable<TValue> current = new();

    private BasicButton button = null!;

    protected override Drawable LoadInputController() {
        this.button = new BasicButton {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Size = new Vector2(130, 40),
            Masking = true,
            CornerRadius = 6,
            BorderThickness = 1.5f,
            BorderColour = Colour4.FromHex("00d2d3").Opacity(0.5f),
            BackgroundColour = Colour4.FromHex("121622").Opacity(0.9f),
            HoverColour = Colour4.FromHex("00d2d3").Opacity(0.35f),
            FlashColour = Colour4.FromHex("00d2d3"),
            Text = this.FormattingDisplayText(this.current.Value),
            Action = this.Action,
        };
        this.current.ValueChanged += this.OnUpdateSettingDisplay;
        return this.button;
    }

    protected override void OnUpdateSettingDisplay(ValueChangedEvent<TValue> value) =>
        this.button.Text = this.FormattingDisplayText(value.NewValue);
}
