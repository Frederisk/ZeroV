using System;

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;

using osuTK;

using ZeroV.Game.Utils;

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
            CornerRadius = 0,
            BorderThickness = 1.5f,
            BorderColour = ZeroVColour.Cyan.Opacity(0.6f),
            BackgroundColour = ZeroVColour.BgCard,
            HoverColour = ZeroVColour.Cyan.Opacity(0.2f),
            FlashColour = ZeroVColour.CyanBright,
            Text = this.FormattingDisplayText(this.current.Value),
            Action = this.Action,
        };
        this.current.ValueChanged += this.OnUpdateSettingDisplay;
        return this.button;
    }

    protected override void OnUpdateSettingDisplay(ValueChangedEvent<TValue> value) =>
        this.button.Text = this.FormattingDisplayText(value.NewValue);
}
