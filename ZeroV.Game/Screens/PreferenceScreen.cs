using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Configs;
using ZeroV.Game.Elements.Buttons;

namespace ZeroV.Game.Screens;
public partial class PreferenceScreen : Screen {

    [BackgroundDependencyLoader]
    private void load(ZeroVConfigManager configManager) {
        this.InternalChildren = [
            new Container {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding{
                    Top = 64,
                    //Bottom = 128,
                    Bottom = 32,
                    Left = 32,
                    Right = 32,
                },
                Child = new BasicScrollContainer<FillFlowContainer>(Direction.Vertical) {
                    RelativeSizeAxes = Axes.Both,
                    Child = new FillFlowContainer{
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0,10),
                        Children = [
                            // TODO: Add controllers.
                            //new Box{
                            //    RelativeSizeAxes = Axes.X,
                            //    Height = 100,
                            //},
                            //new Box{
                            //    RelativeSizeAxes = Axes.X,
                            //    Height = 100,
                            //},
                            //new Box{
                            //    RelativeSizeAxes = Axes.X,
                            //    Height = 100,
                            //},
                            //new Box{
                            //    RelativeSizeAxes = Axes.X,
                            //    Height = 100,
                            //},
                            //new Box{
                            //    RelativeSizeAxes = Axes.X,
                            //    Height = 100,
                            //},
                            //new Box{
                            //    RelativeSizeAxes = Axes.X,
                            //    Height = 100,
                            //},
                            //new Box{
                            //    RelativeSizeAxes = Axes.X,
                            //    Height = 100,
                            //},
                            //new Box{
                            //    RelativeSizeAxes = Axes.X,
                            //    Height = 100,
                            //},
                        ],
                    },
                },
            },
            new ZeroVSpriteText {
                X = 32,
                Text = "Preference",
                FontSize = 64,
            },
            //new DiamondButton {
            //    Y = -64,
            //    X = -200,
            //    Origin = Anchor.Centre,
            //    Anchor = Anchor.BottomCentre,
            //    Size = new Vector2(100),
            //    InnerColour = Colour4.White,
            //    OuterColour = Colour4.Red,
            //    DiamondPadding = 3,
            //    Text = new ZeroVSpriteText {
            //        Origin = Anchor.Centre,
            //        Anchor = Anchor.Centre,
            //        Colour = Colour4.Black,
            //        Text = "Apply",
            //        FontSize = 24,
            //    },
            //}
        ];
    }
}
