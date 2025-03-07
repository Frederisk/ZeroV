using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;

using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Configs;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Screens.Preference.ListItems;

namespace ZeroV.Game.Screens.Preference;

public partial class PreferenceScreen : Screen {

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Container {
                Padding = new MarginPadding(32),
                RelativeSizeAxes = Axes.Both,
                Child = new BasicScrollContainer<FillFlowContainer>(Direction.Vertical) {
                    RelativeSizeAxes = Axes.Both,
                    Child = new FillFlowContainer{
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        //Size = new Vector2(100),
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0,10),
                        Children = [
                            // TODO: Add controllers.
                            //new CheckBoxListItem{
                            //    Setting = ZeroVSetting.Test1,
                            //    LabelText = "1",
                            //},
                            //new CheckBoxListItem{
                            //    Setting = ZeroVSetting.Test2,
                            //    LabelText = "2",
                            //},
                            //new Box() {
                            //    RelativeSizeAxes = Axes.X,
                            //    Height = 100,
                            //    Colour = Colour4.Red,
                            //}
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
