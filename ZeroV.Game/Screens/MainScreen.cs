using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens;

public partial class MainScreen : Screen {

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore) {
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
                    // TODO: Logo here,
                    new Sprite {
                        //Size = new(256),
                        X = 150,
                        Y = 120,
                        Texture = textureStore.Get(@"Logo.svg"),
                        // Scale = new(1.5f),
                    },
                    //new ZeroVSpriteText {
                    //    Text = "ZeroV",
                    //    FontSize = 128,
                    //    Colour = Colour4.Black,
                    //    X = 136,
                    //    Y = 100,
                    //},
                    new DiamondButton {
                        X = 1106,
                        Y = 204,
                        Size = new Vector2(250),
                        DiamondPadding = 10,
                        InnerColour = Colour4.White,
                        OuterColour = Colour4.Red,
                        Text = new ZeroVSpriteText {
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Colour = Colour4.Black,
                            Text = "Play",
                            FontSize = 48,
                        },
                        Action = () => this.Push(new PlaySongSelectScreen()),
                    },
                    new DiamondButton {
                        X = 986,
                        Y = 324,
                        Size = new Vector2(250),
                        DiamondPadding = 10,
                        InnerColour = Colour4.White,
                        OuterColour = Colour4.Red,
                        Text = new ZeroVSpriteText {
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Colour = Colour4.Black,
                            Text = "Options",
                            FontSize = 48,
                        },
                    },
                    new DiamondButton {
                        X = 1106,
                        Y = 444,
                        DiamondPadding = 10,
                        InnerColour = Colour4.White,
                        OuterColour = Colour4.Red,
                        Size = new Vector2(250),
                        Text = new ZeroVSpriteText {
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Colour = Colour4.Black,
                            Text = "Credits",
                            FontSize = 48,
                        },
                    },
                    new DiamondButton {
                        X = 986,
                        Y = 564,
                        DiamondPadding = 10,
                        InnerColour = Colour4.White,
                        OuterColour = Colour4.Red,
                        Size = new Vector2(250),
                        Text = new ZeroVSpriteText {
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Colour = Colour4.Black,
                            Text = "Exit",
                            FontSize = 48,
                        },
                        Action = this.Game.Exit,
                    },
                ],
            },
        ];
    }
}
