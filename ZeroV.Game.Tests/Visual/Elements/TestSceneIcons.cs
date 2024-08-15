using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

using osuTK;

using ZeroV.Game.Graphics.Icons;

namespace ZeroV.Game.Tests.Visual.Elements;

[TestFixture]
public partial class TestSceneIcons : ZeroVTestScene {

    [BackgroundDependencyLoader]
    private void load() {
        FillFlowContainer flower = new() {
            //Position = new Vector2(100, 100),
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Direction = FillDirection.Horizontal,
        };
        this.Child = flower;

        flower.AddRange([
            getIcon<StartIcon>(),
            getIcon<PauseIcon>(),
            getIcon<StopIcon>(),
            getIcon<CrossIcon>(),
            getIcon<NextIcon>(),
        ]);
    }


    private static Container getIcon<T>() where T : IconBase, new() {
        //T icon = new();
        return new Container {
            Size = new Vector2(56),
            //RelativeSizeAxes = Axes.Both,
            Children = [
                new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Red,
                },
                new Circle {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Blue,
                },
                new T(){
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.White,
                },
            ],
        };
    }
}
