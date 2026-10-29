using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;

using ZeroV.Game.Graphics;

namespace ZeroV.Game.Tests.Visual.Graphics;

[TestFixture]
public partial class TestSceneAutoScrollContainer : ZeroVTestScene {

    [BackgroundDependencyLoader]
    private void load() {
        const string long_text = "This is a test text for AutoScrollContainer. It should scroll automatically.";
        const string short_text = "Test text here.";
        this.Children = [
            new AutoScrollContainer {
                X = 100,
                Y = 100,
                Width = 150,
                Height = 30,
                Mode = AutoScrollMode.LeftRightPingPong,
                Child = new SpriteText {
                    Text = long_text,
                    Font = FontUsage.Default.With(size: 20),
                }
            },
            new AutoScrollContainer {
                X = 100,
                Y = 200,
                Width = 150,
                Height = 30,
                Mode = AutoScrollMode.LeftRightLoop,
                Child = new SpriteText {
                    Text = long_text,
                    Font = FontUsage.Default.With(size: 20),
                }
            },
            new AutoScrollContainer {
                X = 100,
                Y = 300,
                Width = 50,
                Height = 30,
                Mode = AutoScrollMode.LeftRightLoop,
                Child = new SpriteText {
                    Text = long_text,
                    Font = FontUsage.Default.With(size: 20),
                }
            },
            new AutoScrollContainer {
                X = 100,
                Y = 400,
                Width = 200,
                Height = 30,
                Mode = AutoScrollMode.LeftRightLoop,
                Child = new SpriteText {
                    Text = short_text,
                    Font = FontUsage.Default.With(size: 20),
                }
            },
        ];
    }
}
