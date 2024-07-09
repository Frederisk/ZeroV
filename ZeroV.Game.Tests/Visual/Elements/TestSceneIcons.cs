using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

using osuTK;

using ZeroV.Game.Graphics;

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
        //flower.Add(new SpriteIcon {
        //    Size = new Vector2(20),
        //    Icon = ZeroVIcon.RulesetOsu,
        //});
        // flower.Add(
        //     new ZeroVIcon {
        //         IconType = ZeroIconType.Exit,
        //         Size = new Vector2(100),
        //     });
        // flower.Add(
        //     new ZeroVIcon {
        //         IconType = ZeroIconType.Exit,
        //         Size = new Vector2(100),
        //     });
        flower.Add(
            new ZeroIcon {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Size = new Vector2(100),
            });
    }
}
