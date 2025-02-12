using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

using osuTK;

using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Graphics;

namespace ZeroV.Game.Screens.Gameplay;

public partial class PauseOverlay : OverlayContainer {
    protected const Int32 TRANSITION_DURATION = 200;
    private const Single background_alpha = 0.75f;

    public required Action OnResume { get; init; }
    public required Action OnRetry { get; init; }
    public required Action OnQuit { get; init; }

    private DiamondButton quitButton = null!;
    private DiamondButton resumeButton = null!;
    private DiamondButton retryButton = null!;

    public FillFlowContainer<DiamondButton> ButtonsContainer = null!;

    public ZeroVSpriteText CountdownDisplay = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.Both;

        this.quitButton = new DiamondButton() {
            Size = new(100),
            DiamondPadding = 3,
            InnerColour = Colour4.Transparent,
            OuterColour = Colour4.White,
            Text = new ZeroVSpriteText {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Colour = Colour4.Black,
                Text = "Quit",
                FontSize = 24,
            },
            Action = this.OnQuit,
        };
        this.resumeButton = new DiamondButton() {
            Size = new(100),
            DiamondPadding = 3,
            InnerColour = Colour4.Transparent,
            OuterColour = Colour4.White,
            Text = new ZeroVSpriteText {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Colour = Colour4.Black,
                Text = "Resume",
                FontSize = 24,
            },
            Action = this.OnResume
        };
        this.retryButton = new DiamondButton() {
            Size = new(100),
            DiamondPadding = 3,
            InnerColour = Colour4.Transparent,
            OuterColour = Colour4.White,
            Text = new ZeroVSpriteText {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Colour = Colour4.Black,
                Text = "Retry",
                FontSize = 24,
            },
            Action = this.OnRetry
        };
        this.ButtonsContainer = new FillFlowContainer<DiamondButton>() {
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

    protected override Boolean OnClick(ClickEvent e) => true;
}
