using System;
using System.Numerics;

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

using ZeroV.Game.Graphics;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens.Preference.ListItems;

public partial class SliderBarListItem<TValue, TSetting> : BasePreferenceListItem<TValue, TSetting> where TValue : struct, INumber<TValue>, IMinMaxValue<TValue> where TSetting : struct, Enum {
    public required TValue MaxValue { get; init; }

    public required TValue MinValue { get; init; }

    public required TValue Precision { get; init; }

    public override Bindable<TValue> Current => this.sliderBar.Current;

    private BasicSliderBar<TValue> sliderBar = null!;
    private ZeroVSpriteText displayText = null!;

    protected override Drawable LoadInputController() {
        this.sliderBar = new BasicSliderBar<TValue> {
            Size = new osuTK.Vector2(220, 24),
            SelectionColour = ZeroVColour.Cyan,
            BackgroundColour = ZeroVColour.BgCardSelected,
            CornerRadius = 0,
            Masking = true,
            Current = new BindableNumber<TValue>() {
                MaxValue = this.MaxValue,
                MinValue = this.MinValue,
                Precision = this.Precision,
            },
        };
        this.displayText = new ZeroVSpriteText {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Text = this.FormattingDisplayText(this.sliderBar.Current.Value),
            FontSize = 20,
            Colour = ZeroVColour.Cyan,
        };
        this.sliderBar.Current.ValueChanged += this.OnUpdateSettingDisplay;
        return new FillFlowContainer {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Direction = FillDirection.Horizontal,
            Spacing = new osuTK.Vector2(16, 0),
            AutoSizeAxes = Axes.Both,
            Children = [
                this.displayText,
                this.sliderBar,
            ],
        };
    }

    protected override void OnUpdateSettingDisplay(ValueChangedEvent<TValue> value) =>
       this.displayText.Text = this.FormattingDisplayText(value.NewValue);
}
