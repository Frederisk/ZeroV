using System;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

using ZeroV.Game.Graphics;

namespace ZeroV.Game.Elements.Counters;

public partial class ComboCounter : Container {
    public Bindable<UInt32> Current = new();

    public ZeroVSpriteText? DrawableCount { get; private set; }

    public ComboCounter() {
        this.AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.DrawableCount = new() { FontSize = 40 };
        this.Child = this.DrawableCount; // = this.displayedCountText;
    }

    protected override void LoadComplete() {
        base.LoadComplete();
        this.Current.BindValueChanged(val => this.TransformCount(val.OldValue, val.NewValue), true);
    }

    protected void TransformCount(UInt32 current, UInt32 newValue) {
        if (this.DrawableCount is null || current == newValue) {
            return;
        }
        if (newValue <= 2) {
            this.Hide();
            return;
        }
        this.Show();
        this.DrawableCount.Text = newValue.ToString();
    }
}
