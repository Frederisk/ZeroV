using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Configs;
using ZeroV.Game.Elements.Buttons;

namespace ZeroV.Game.Screens.Preference;

public partial class PreferenceScreen : Screen {

    [BackgroundDependencyLoader]
    private void load(ZeroVConfigManager configManager) {
        this.InternalChildren = [
            new Container {
                X = 32,
                Padding = new MarginPadding(32),
                Child = new BasicScrollContainer<FillFlowContainer>(Direction.Vertical) {
                    RelativeSizeAxes = Axes.Both,
                    Child = new FillFlowContainer{
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0,10),
                        Children = [
                            // TODO: Add controllers.
                        ],
                    },
                },
            },
            new BackButton(this) {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Height = 52,
                Width = 108,
                Text = "< Back",
            },
        ];
    }
}
