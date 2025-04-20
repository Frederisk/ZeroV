using System;
using System.Diagnostics.CodeAnalysis;

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

    private PauseActionButton quitButton = null!;
    private PauseActionButton resumeButton = null!;
    private PauseActionButton retryButton = null!;

    public FillFlowContainer<DiamondButton> ButtonsContainer = null!;

    public ZeroVSpriteText CountdownDisplay = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.Both;

        this.quitButton = new PauseActionButton("Quit") {
            Action = this.OnQuit,
        };
        this.resumeButton = new PauseActionButton("Resume") {
            Action = this.OnResume,
        };
        this.retryButton = new PauseActionButton("Retry") {
            Action = this.OnRetry,
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

    private partial class PauseActionButton : DiamondButton {

        [SetsRequiredMembers]
        public PauseActionButton(String labelText) : base() {
            this.Size = new(100);
            this.DiamondPadding = 3;
            this.InnerColour = Colour4.WhiteSmoke.MultiplyAlpha(0.4f);
            this.OuterColour = Colour4.White;
            this.Text = new ZeroVSpriteText {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Colour = Colour4.Black,
                Text = labelText,
                FontSize = 24,
            };
        }
    }

    protected override void PopIn() => this.FadeIn(TRANSITION_DURATION, Easing.In);

    protected override void PopOut() => this.FadeOut(TRANSITION_DURATION, Easing.In);

    // Don't let touch down events through the overlay or people can touch particle while paused.
    protected override Boolean OnTouchDown(TouchDownEvent e) => true;

    protected override Boolean OnClick(ClickEvent e) => true;
}
