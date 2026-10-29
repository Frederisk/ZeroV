using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace ZeroV.Game.Graphics;

public partial class AutoScrollContainer : Container {
    private Container contentContainer;
    protected override Container<Drawable> Content => this.contentContainer;
    private Single lastContainerWidth;
    private Single lastContainerHeight;
    private Single lastContentWidth;
    private Single lastContentHeight;

    //private Boolean isPingPong = true;

    //public Boolean IsPingPong {
    //    get => this.isPingPong;
    //    set {
    //        if (this.isPingPong == value) { return; }
    //        this.isPingPong = value;
    //        this.ResetAndRestartAnimation();
    //    }
    //}

    private AutoScrollMode mode = AutoScrollMode.LeftRightPingPong;

    public AutoScrollMode Mode {
        get => this.mode;
        set {
            if (this.mode == value) { return; }
            this.mode = value;
            this.ResetAndRestartAnimation();
        }
    }

    private Double speed = 45f;

    public Double Speed {
        get => this.speed;
        set {
            if (this.speed == value) { return; }
            this.speed = value;
            this.ResetAndRestartAnimation();
        }
    }

    private Double startDelay = 1200f;

    public Double StartDelay {
        get => this.startDelay;
        set {
            if (this.startDelay == value) { return; }
            this.startDelay = value;
            this.ResetAndRestartAnimation();
        }
    }
    private Double endDelay = 1200f;

    public Double EndDelay {
        get => this.endDelay;
        set {
            if (this.endDelay == value) { return; }
            this.endDelay = value;
            this.ResetAndRestartAnimation();
        }
    }

    public Easing? ScrollEasing { get; set; }

    public AutoScrollContainer() {
        this.Masking = true;
        this.contentContainer = new Container {
            AutoSizeAxes = Axes.Both,
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
        };
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChild = this.contentContainer;
    }

    protected override void LoadComplete() {
        base.LoadComplete();
        this.ResetAndRestartAnimation();
    }

    protected override void UpdateAfterChildren() {
        base.UpdateAfterChildren();

        if (this.DrawWidth != this.lastContainerWidth || this.contentContainer.DrawWidth != this.lastContentWidth) {
            this.lastContainerWidth = this.DrawWidth;
            this.lastContentWidth = this.contentContainer.DrawWidth;
            this.ResetAndRestartAnimation();
        }
    }

    public void ResetAndRestartAnimation() {
        if (!this.IsLoaded) { return; }

        this.contentContainer.ClearTransforms();
        this.contentContainer.X = 0;
        Single containerWidth = this.DrawWidth;
        Single contentWidth = this.contentContainer.DrawWidth;

        if (containerWidth <= 0 || contentWidth <= 0 || contentWidth <= containerWidth) { return; }

        Single overflow = contentWidth - containerWidth;
        Double duration = overflow / this.speed * 1000;
        Double outDuration = contentWidth / this.speed * 1000;
        Double inDuration = containerWidth / this.speed * 1000;
        Easing pingPongEasing = this.ScrollEasing ?? Easing.InOutSine;
        Easing loopEasing = this.ScrollEasing ?? Easing.None;

        switch (this.mode) {
            case AutoScrollMode.LeftRightPingPong:
                this.contentContainer
                    .Delay(this.startDelay)
                    .MoveToX(-overflow, duration, pingPongEasing)
                    .Then(this.endDelay)
                    .MoveToX(0, duration, pingPongEasing)
                    .Loop(this.startDelay);
                break;

            // case AutoScrollMode.RightLeftPingPong:
            // case AutoScrollMode.UpDownPingPong:
            // case AutoScrollMode.DownUpPingPong:

            case AutoScrollMode.LeftRightLoop:
                this.contentContainer
                    .Delay(this.startDelay)
                    .MoveToX(-contentWidth, outDuration, loopEasing)
                    .Then()
                    .MoveToX(contentWidth)
                    .Then()
                    .MoveToX(0, inDuration, loopEasing)
                    .Loop();
                break;

            // case AutoScrollMode.RightLeftLoop:
            // case AutoScrollMode.UpDownLoop:
            // case AutoScrollMode.DownUpLoop:

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum AutoScrollMode {
    LeftRightPingPong,
    LeftRightLoop,
    //RightLeftPingPong,
    //RightLeftLoop,
    //UpDownPingPong,
    //UpDownLoop,
}
