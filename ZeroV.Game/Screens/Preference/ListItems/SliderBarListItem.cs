using System;
using System.Numerics;

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;

using ZeroV.Game.Graphics;

namespace ZeroV.Game.Screens.Preference.ListItems;

public partial class SliderBarListItem<T> : BasePreferenceListItem<T> where T : struct, INumber<T>, IMinMaxValue<T> {
    public required T MaxValue { get; init; }

    public required T MinValue { get; init; }

    public required T Precision { get; init; }

    public override Bindable<T> Current => this.sliderBar.Current;

    private BasicSliderBar<T> sliderBar = null!;
    private ZeroVSpriteText displayText = null!;

    protected override Drawable LoadInputController() {
        this.sliderBar = new BasicSliderBar<T> {
            Size = new osuTK.Vector2(200, 25),
            Current = new BindableNumber<T>() {
                MaxValue = this.MaxValue,
                MinValue = this.MinValue,
                Precision = this.Precision,
            },
        };
        this.displayText = new ZeroVSpriteText {
            //Anchor = Anchor.CentreRight,
            //Origin = Anchor.CentreRight,
            Text = this.FormattingDisplayText(this.sliderBar.Current.Value),
            FontSize = 25,
        };
        this.sliderBar.Current.ValueChanged += this.updateDisplayText;
        return new FillFlowContainer {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Direction = FillDirection.Horizontal,
            Spacing = new osuTK.Vector2(10, 0),
            AutoSizeAxes = Axes.Both,
            Children = [
                this.displayText,
                this.sliderBar,
            ]
        };
    }

    private void updateDisplayText(ValueChangedEvent<T> value) =>
       this.displayText.Text = this.FormattingDisplayText(value.NewValue);

    public Func<T, LocalisableString> FormattingDisplayText { get; init; } = value => value.ToString() ?? "";
}
