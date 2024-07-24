using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

namespace ZeroV.Game.Screens;

public partial class PauseOverlay : OverlayContainer {

    protected const Int32 TRANSITION_DURATION = 200;
    private const Single background_alpha = 0.75f;

    public Action? OnResume { get; init; }
    public Action? OnRetry { get; init; }
    public Action? OnQuit { get; init; }

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.Both;

        this.Children = [
            new Box() {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black,
                Alpha = background_alpha,
            },
            new FillFlowContainer() {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new osuTK.Vector2(10, 0),
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Children = [
                    new BasicButton() {
                        Height = 100,
                        Width = 100,
                        Text = "Quit",
                        Action = this.OnQuit
                    },
                    new BasicButton() {
                        Height = 100,
                        Width = 100,
                        Text = "Resume",
                        Action = this.OnResume
                    },
                    new BasicButton() {
                        Height = 100,
                        Width = 100,
                        Text = "Retry",
                        Action = this.OnRetry
                    }
                ]
            }
        ];
    }

    protected override void PopIn() => this.FadeIn(TRANSITION_DURATION, Easing.In);
    protected override void PopOut() => this.FadeOut(TRANSITION_DURATION, Easing.In);

    // Don't let touch down events through the overlay or people can touch particle while paused.
    protected override Boolean OnTouchDown(TouchDownEvent e) => true;

}
