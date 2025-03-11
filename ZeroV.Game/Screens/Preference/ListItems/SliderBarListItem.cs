using System.Numerics;

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace ZeroV.Game.Screens.Preference.ListItems;

public partial class SliderBarListItem<T> : BasePreferenceListItem<T> where T : struct, INumber<T>, IMinMaxValue<T> {
    public required T MaxValue { get; init; }

    public required T MinValue { get; init; }

    public required T Precision { get; init; }

    public override Bindable<T> Current => this.sliderBar.Current;

    private BasicSliderBar<T> sliderBar = null!;

    protected override Drawable LoadInputController() {
        this.sliderBar = new BasicSliderBar<T> {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Current = new BindableNumber<T>() {
                MaxValue = this.MaxValue,
                MinValue = this.MinValue,
                Precision = this.Precision,
            },
        };
        return this.sliderBar;
    }
}
