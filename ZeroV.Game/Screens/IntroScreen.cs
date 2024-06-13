using System;
using System.Threading.Tasks;

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

        this.Schedule(async () => {
            var task = Task.Run(this.fakeLoad);
            while (!task.IsCompletedSuccessfully) {
                await Task.Delay(Random.Shared.Next(50, 500));
                this.textFlow.AddParagraph("Hello");
            }
            this.textFlow.FadeOut(1000).Schedule(continueToMain);
        });
    }

    private void continueToMain() {
        this.Push(new MainScreen());
    }

    /// <summary>
    /// A slow method.
    /// </summary>
    private void fakeLoad() {
        Task.Delay(10000).Wait();
    }
}
