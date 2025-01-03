using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

using osuTK;

namespace ZeroV.Game.Screens.Gameplay;

public partial class PauseOverlay : OverlayContainer {
    protected const Int32 TRANSITION_DURATION = 200;
    private const Single background_alpha = 0.75f;

    public required Action OnResume { get; init; }
    public required Action OnRetry { get; init; }
    public required Action OnQuit { get; init; }

    private BasicButton quitButton = null!;
    private BasicButton resumeButton = null!;
    private BasicButton retryButton = null!;

    public FillFlowContainer ButtonsContainer = null!;

    public ZeroVSpriteText CountdownDisplay = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.Both;

        this.quitButton = new BasicButton() {
            Height = 100,
            Width = 100,
            Text = "Quit",
            Action = this.OnQuit,
        };
        this.resumeButton = new BasicButton() {
            Height = 100,
            Width = 100,
            Text = "Resume",
            Action = this.OnResume
        };
        this.retryButton = new BasicButton() {
            Height = 100,
            Width = 100,
            Text = "Retry",
            Action = this.OnRetry
        };

        this.ButtonsContainer = new FillFlowContainer() {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(10, 0),
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            Children = [
                    this.quitButton,
                    this.resumeButton,
                    this.retryButton,
            ],
        };

        this.CountdownDisplay = new ZeroVSpriteText {
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            FontSize = 48,
        };
        this.CountdownDisplay.Hide();

        this.Children = [
            new Box() {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black,
                Alpha = background_alpha,
            },
            this.CountdownDisplay,
            this.ButtonsContainer,
        ];
    }

    protected override void PopIn() => this.FadeIn(TRANSITION_DURATION, Easing.In);

    protected override void PopOut() => this.FadeOut(TRANSITION_DURATION, Easing.In);

    // Don't let touch down events through the overlay or people can touch particle while paused.
    protected override Boolean OnTouchDown(TouchDownEvent e) => true;
    //protected override Boolean OnClick(ClickEvent e) => true;
}
