using System;
using System.Collections.Generic;
using System.Globalization;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

using ZeroV.Game.Data;
using ZeroV.Game.Graphics;
using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Screens.Gameplay;

public partial class ResultOverlay : OverlayContainer {
    public required Action OnQuit { get; init; }
    public required Action OnRetry { get; init; }

    protected const Int32 TRANSITION_DURATION = 1000;
    private const Single background_alpha = 0.75f;

    [Resolved]
    private IGameplayInfo screen { get; set; } = null!;

    [Resolved]
    private ResultInfoProvider resultInfoProvider { get; set; } = null!;

    private ZeroVSpriteText title = null!;
    private ZeroVSpriteText version = null!;
    private ZeroVSpriteText difficulty = null!;
    private ZeroVSpriteText scoringNumber = null!;
    private BasicButton quitButton = null!;
    private BasicButton retryButton = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.Both;
        this.title = new ZeroVSpriteText {
            Origin = Anchor.CentreLeft,
            Anchor = Anchor.CentreLeft,
            Y = -196,
            X = 24,
            FontSize = 96,
        };
        this.version = new ZeroVSpriteText {
            Origin = Anchor.CentreLeft,
            Anchor = Anchor.CentreLeft,
            Y = -130,
            X = 24,
            FontSize = 64,
        };
        this.difficulty = new ZeroVSpriteText {
            Origin = Anchor.CentreLeft,
            Anchor = Anchor.CentreLeft,
            Y = -84,
            X = 24,
            FontSize = 64,
        };
        this.scoringNumber = new ZeroVSpriteText {
            Origin = Anchor.CentreRight,
            Anchor = Anchor.CentreRight,
            Y = 64,
            X = -24,
            FontSize = 128,
        };
        this.quitButton = new BasicButton {
            Origin = Anchor.CentreRight,
            Anchor = Anchor.CentreRight,
            Y = 180,
            X = -50,
            Height = 50,
            Width = 120,
            Text = "Next",
            Action = this.OnQuit,
        };
        this.retryButton = new BasicButton {
            Origin = Anchor.CentreRight,
            Anchor = Anchor.CentreRight,
            Y = 180,
            X = -170,
            Height = 50,
            Width = 120,
            Text = "Retry",
            Action = this.OnRetry,
        };

        this.InternalChildren = [
            new Box() {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black,
                Alpha = background_alpha,
            },
            this.title,
            this.version,
            this.difficulty,
            this.scoringNumber,
            this.quitButton,
            this.retryButton,
        ];
    }

    protected override void PopIn() {
        ScoringCalculator scoringCalculator = this.screen.ScoringCalculator;
        ResultInfo result = new() {
            UUID = this.screen.TrackInfo.UUID,
            GameVersion = this.screen.TrackInfo.GameVersion,
            Index = this.screen.MapInfo.Index,
            IsAllDone = scoringCalculator.IsAllDone,
            IsAllPerfect = scoringCalculator.IsAllPerfect,
            IsFullCombo = scoringCalculator.IsFullCombo,
            Scoring = scoringCalculator.Scoring,
            FinishTime = DateTime.Now,
        };
        List<ResultInfo> infoList = this.resultInfoProvider.Get() ?? [];
        infoList.Add(result);
        this.resultInfoProvider.Set(infoList);

        this.title.Text = this.screen.TrackInfo.Title;
        this.version.Text = this.screen.TrackInfo.GameVersion.ToString();
        this.difficulty.Text = this.screen.MapInfo.Difficulty.ToString("#.##");
        this.scoringNumber.Text = scoringCalculator.Scoring.ToDisplayScoring();
        this.scoringNumber.Colour = result.ToResultColour();

        //String formatString = new ('0', 7);
        //LocalisableString formattedCount = this.ScoringCalculator.DisplayScoring.ToLocalisableString(formatString);
        //this.scoringNumber.Text = formattedCount;
        this.FadeIn(TRANSITION_DURATION, Easing.In);
    }

    protected override void PopOut() => this.FadeOut(TRANSITION_DURATION, Easing.Out);

    protected override Boolean OnTouchDown(TouchDownEvent e) => true;

    protected override Boolean OnClick(ClickEvent e) => true;
}
