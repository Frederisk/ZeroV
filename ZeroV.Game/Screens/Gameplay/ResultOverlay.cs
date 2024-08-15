using System;
using System.Globalization;

using osu.Framework.Allocation;
using osu.Framework.Extensions.LocalisationExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;

using ZeroV.Game.Scoring;

namespace ZeroV.Game.Screens.Gameplay;
public partial class ResultOverlay : OverlayContainer {

    protected const Int32 TRANSITION_DURATION = 200;
    private const Single background_alpha = 0.75f;

    [Resolved]
    public ScoringCalculator ScoringCalculator { get; protected set; } = null!;

    private ZeroVSpriteText scoringNumber = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.scoringNumber = new ZeroVSpriteText() {
            Origin = Anchor.CentreRight,
            Anchor = Anchor.CentreRight,
            X = 256,
            FontSize = 76,
        };
        this.InternalChildren = [
            new Box() {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black,
                Alpha = background_alpha,
            },
            this.scoringNumber
        ];
    }

    protected override void PopIn() {
        this.scoringNumber.Text = this.ScoringCalculator.DisplayScoring.ToString(new String('0', 7), CultureInfo.InvariantCulture);
        //String formatString = new ('0', 7);
        //LocalisableString formattedCount = this.ScoringCalculator.DisplayScoring.ToLocalisableString(formatString);
        //this.scoringNumber.Text = formattedCount;
        this.FadeIn(TRANSITION_DURATION, Easing.In);
    }

    protected override void PopOut() => this.FadeOut(TRANSITION_DURATION, Easing.In);

    protected override Boolean OnTouchDown(TouchDownEvent e) => true;

    //protected override Boolean OnClick(ClickEvent e) => true;
}
