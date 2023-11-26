using System;
using System.Collections.Generic;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;

namespace ZeroV.Game.Elements;
public abstract partial class RollingCounter<T> : Container, IHasCurrentValue<T> where T : struct, IEquatable<T> {
    private readonly BindableWithCurrent<T> current = new();

    public Bindable<T> Current {
        get => this.current.Current;
        set => this.current.Current = value;
    }

    private IHasText? displayedCountText;

    public Drawable? DrawableCount { get; private set; }

    /// <summary>
    /// If true, the roll-up duration will be proportional to change in value.
    /// </summary>
    protected virtual Boolean IsRollingProportional => false;
    /// <summary>
    /// If IsRollingProportional = false, duration in milliseconds for the counter roll-up animation for each
    /// element; else duration in milliseconds for the counter roll-up animation in total.
    /// </summary>
    protected virtual Double RollingDuration => 0;

    /// <summary>
    /// Easing for the counter rollover animation.
    /// </summary>
    protected virtual Easing RollingEasing => Easing.OutQuint;

    private T displayedCount;

    /// <summary>
    /// Value shown at the current moment.
    /// </summary>
    public virtual T DisplayedCount {
        get => this.displayedCount;
        set {
            if (EqualityComparer<T>.Default.Equals(this.displayedCount, value)) {
                return;
            }

            this.displayedCount = value;
            this.UpdateDisplay();
        }
    }

    /// <summary>
    /// Skeleton of a numeric counter which value rolls over time.
    /// </summary>
    protected RollingCounter() {
        this.AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load() {
        // IHasText displayedCountText
        this.displayedCountText = this.CreateText();
        this.UpdateDisplay();
        // Drawable Child, DrawableCount
        this.Child = this.DrawableCount = (Drawable)this.displayedCountText;
    }

    protected void UpdateDisplay() {
        if (this.displayedCountText != null) {
            this.displayedCountText.Text = this.FormatCount(this.DisplayedCount);
        }
    }

    protected override void LoadComplete() {
        base.LoadComplete();
        this.Current.BindValueChanged(val => this.TransformCount(this.DisplayedCount, val.NewValue), true);
    }

    /// <summary>
    /// Calculates the duration of the roll-up animation by using the difference between the current visible value
    /// and the new final value.
    /// </summary>
    /// <remarks>
    /// To be used in conjunction with IsRollingProportional = true.
    /// Unless a derived class needs to have a proportional rolling, it is not necessary to override this function.
    /// </remarks>
    /// <param name="currentValue">Current visible value.</param>
    /// <param name="newValue">New final value.</param>
    /// <returns>Calculated rollover duration in milliseconds.</returns>
    protected virtual Double GetProportionalDuration(T currentValue, T newValue) => this.RollingDuration;

    /// <summary>
    /// Used to format counts.
    /// </summary>
    /// <param name="count">Count to format.</param>
    /// <returns>Count formatted as a localisable string.</returns>
    protected virtual LocalisableString FormatCount(T count) {
        return count.ToString()!;
    }

    /// <summary>
    /// Called when the count is updated to add a transformer that changes the value of the visible count (i.e.
    /// implement the rollover animation).
    /// </summary>
    /// <param name="currentValue">Count value before modification.</param>
    /// <param name="newValue">Expected count value after modification.</param>
    protected virtual void TransformCount(T currentValue, T newValue) {
        Double rollingTotalDuration =
            this.IsRollingProportional
                ? this.GetProportionalDuration(currentValue, newValue)
                : this.RollingDuration;

        this.TransformTo(nameof(this.DisplayedCount), newValue, rollingTotalDuration, this.RollingEasing);
    }

    /// <summary>
    /// Creates the text. Delegates to <see cref="CreateSpriteText"/> by default.
    /// </summary>
    protected virtual IHasText CreateText() => this.CreateSpriteText();

    /// <summary>
    /// Creates an <see cref="SpriteText"/> which may be used to display this counter's text.
    /// May not be called if <see cref="CreateText"/> is overridden.
    /// </summary>
    protected virtual SpriteText CreateSpriteText() => new() {
        // Font = OsuFont.Numeric.With(size: 40f),
        Size = new(40f),
    };
}
