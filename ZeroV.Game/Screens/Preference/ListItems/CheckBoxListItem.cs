using System;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;

using ZeroV.Game.Configs;
using ZeroV.Game.Elements.Buttons;

namespace ZeroV.Game.Screens.Preference.ListItems;

public partial class CheckBoxListItem : CompositeDrawable {
    public required ZeroVSetting Setting { get; init; }

    public required String LabelText { get; init; }

    public Bindable<Boolean> Current => this.switchButton.Current;

    [Resolved]
    private ZeroVConfigManager configManger { get; set; } = null!;

    private SwitchButton switchButton = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.X;
        this.AutoSizeAxes = Axes.Y;

        this.InternalChild = new Container {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Children = [
                new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.DarkGray,
                },
                new ZeroVSpriteText {
                    Padding = new(24),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Text = this.LabelText,
                    FontSize = 30,
                },
                new Container {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Padding = new(24),
                    Child = this.switchButton =  new SwitchButton {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                    },
                },
            ],
        };
        this.configManger.BindWith(this.Setting ,this.switchButton.Current);
    }
}
