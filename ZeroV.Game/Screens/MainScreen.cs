using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Elements;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens;

public partial class MainScreen : Screen {

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Box {
                Colour = Colour4.Violet,
                RelativeSizeAxes = Axes.Both,
            },
            new Container {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(ZeroVMath.SCREEN_DRAWABLE_X, ZeroVMath.SCREEN_DRAWABLE_Y),
                Children = [
                    new Box {
                        Colour = Colour4.White,
                        RelativeSizeAxes = Axes.Both,
                    },
                    // TODO: Logo here
                    new ZeroVSpriteText {
                        Text = "ZeroV",
                        FontSize = 128,
                        Colour = Colour4.Black,
                        X = 136,
                        Y = 100,
                    },
                    new DiamondButton {
                        X = 1106,
                        Y = 204,
                        Size = new Vector2(250),
                        Text = "Play",
                        Action = () => this.Push(new PlaySongSelectScreen()),
                    },
                    new DiamondButton {
                        X = 986,
                        Y = 324,
                        Size = new Vector2(250),
                        Text = "Options",
                    },
                    new DiamondButton {
                        X = 1106,
                        Y = 444,
                        Size = new Vector2(250),
                        Text = "Credits",
                    },
                    new DiamondButton {
                        X = 986,
                        Y = 564,
                        Size = new Vector2(250),
                        Text = "Exit",
                        Action = this.Game.Exit,
                    },
                ],
            },
        ];
    }
}
