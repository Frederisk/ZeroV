using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;

using osuTK.Graphics;

namespace ZeroV.Game.Screens;

public partial class IntroScreen : Screen {
    private TextFlowContainer textFlow = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.textFlow = new TextFlowContainer {
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.BottomLeft,
            //Direction = FillDirection.Vertical,
            AutoSizeAxes = Axes.Both,
            Text = "Loading ZeroV...",
        };

        this.InternalChildren = [
            new Box {
                Colour = Color4.Black,
                // 1366 * 768
                RelativeSizeAxes = Axes.Both,
            },

            this.textFlow,
        ];
    }

    protected override void LoadComplete() {
        base.LoadComplete();

        this.Delay(500).Schedule(() => { this.textFlow.AddParagraph("Hello"); })
            .Delay(200).Schedule(() => { this.textFlow.AddParagraph("Hello"); })
            .Delay(100).Schedule(() => { this.textFlow.AddParagraph("Hello"); })
            .Delay(200).Schedule(() => { this.textFlow.AddParagraph("Hello"); })
            .Delay(100).Schedule(() => { this.textFlow.AddParagraph("Hello"); })
            .Delay(50).Schedule(() => { this.textFlow.AddParagraph("Hello"); })
            .Delay(100).Schedule(() => { this.textFlow.AddParagraph("Hello"); })
            .Delay(50).Schedule(() => { this.textFlow.AddParagraph("Hello"); })

            .Delay(1000).Schedule(this.continueToMain);
    }

    private void continueToMain() {
        this.Push(new MainScreen());
    }
}
