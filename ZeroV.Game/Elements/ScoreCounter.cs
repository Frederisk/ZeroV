using System;

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Framework.Extensions.LocalisationExtensions;

namespace ZeroV.Game.Elements;

public partial class ScoreCounter : RollingCounter<UInt32> {
    protected override Double RollingDuration => 500;
    protected override Easing RollingEasing => Easing.Out;

    public Bindable<Int32> RequiredDisplayDigits { get; } = new();
    private String? formatString;

    /// <summary>
    /// Displays score.
    /// </summary>
    /// <param name="leading">How many leading zeroes the counter will have.</param>
    public ScoreCounter(Int32 leading = 7) {
        this.RequiredDisplayDigits.Value = leading;
        this.RequiredDisplayDigits.BindValueChanged(this.displayDigitsChanged, true);
    }

    private void displayDigitsChanged(ValueChangedEvent<Int32> _) {
        this.formatString = new String('0', this.RequiredDisplayDigits.Value);
        this.UpdateDisplay();
    }

    protected override Double GetProportionalDuration(UInt32 currentValue, UInt32 newValue) => Math.Abs(currentValue - newValue);

    protected override LocalisableString FormatCount(UInt32 count) => count.ToLocalisableString(this.formatString);

    protected override ZeroVSpriteText CreateSpriteText()
        => base.CreateSpriteText().With(spriteText => spriteText.IsFixedWidth = true);
}
