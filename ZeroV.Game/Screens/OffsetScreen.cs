using System;

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Configs;
using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;

namespace ZeroV.Game.Screens;

public partial class OffsetScreen : BaseUserInterfaceScreen {

    private Bindable<Double> offset = null!;

    private Track offsetBeatTrack = null!;

    private BlinkDiamond movingLine = null!;

    private Container tempDisplayContainer = null!;

    private ZeroVSpriteText offsetText = null!;

    [BackgroundDependencyLoader]
    private void load(ITrackStore trackStore, ZeroVConfigManager configManager) {
        this.offset = configManager.GetBindable<Double>(ZeroVSetting.GlobalSoundOffset);

        this.offsetBeatTrack = trackStore.Get("OffsetBeats.flac");
        //this.offsetBeatTrack = trackStore.GetVirtual();
        this.offsetBeatTrack.Looping = true;

        this.movingLine = new BlinkDiamond() {
            RelativePositionAxes = Axes.Both,
        };

        this.tempDisplayContainer = new Container {
            RelativeSizeAxes = Axes.Both,
        };

        this.offsetText = new ZeroVSpriteText {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativePositionAxes = Axes.Both,
            Y = 0.25f,
            FontSize = 64,
            Text = this.offset.Value + "ms",
        };

        ArrowButton leftArrow = new(OrientedTriangle.Orientation.Left) {
            Y = 0.25f,
            X = -0.2f,
            TouchDown = e => this.offset.Value--,
        };
        ArrowButton rightArrow = new(OrientedTriangle.Orientation.Right) {
            Y = 0.25f,
            X = +0.2f,
            TouchDown = e => this.offset.Value++,
        };
        //ArrowButton leftFastArrow = new(OrientedTriangle.Orientation.Left) {
        //    Y = 0.25f,
        //    X = -0.25f,
        //    TouchDown = e => this.offset.Value -= 10,
        //};
        //ArrowButton rightFastArrow = new(OrientedTriangle.Orientation.Right) {
        //    Y = 0.25f,
        //    X = +0.25f,
        //    TouchDown = e => this.offset.Value += 10,
        //};

        Drawable[] items = [
            // Visual offset
            new Container {
                RelativeSizeAxes = Axes.Both,
                Width = 0.8f,
                Height = 0.25f,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                CornerRadius = 32,
                Masking = true,
                Children = [
                    new Box {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.LightBlue,
                    },
                    new BlinkDiamond() {
                        RelativePositionAxes = Axes.Both,
                        X = -0.25f,
                        Alpha = 0.5f,
                    },
                    new BlinkDiamond() {
                        RelativePositionAxes = Axes.Both,
                        X = +0.25f,
                        Alpha = 0.5f,
                    },
                    new BlinkDiamond() {
                        RelativePositionAxes = Axes.Both,
                        X = 0.00f,
                        Alpha = 0.75f,
                    },
                    this.tempDisplayContainer,
                    this.movingLine,
                ],
            },
            // User interface
            this.offsetText,
            leftArrow,
            rightArrow,
            //leftFastArrow,
            //rightFastArrow,
        ];
        this.InternalChildren = items;
    }

    protected override void LoadComplete() {
        this.offset.ValueChanged += this.offsetChanged;
        base.LoadComplete();
    }

    public override void OnEntering(ScreenTransitionEvent e) {
        this.offsetBeatTrack.Start();
    }

    public override Boolean OnExiting(ScreenExitEvent e) {
        this.offsetBeatTrack.Stop();
        return base.OnExiting(e);
    }

    /// <summary>
    /// Beat per minute.
    /// </summary>
    private const Double bpm = 128; // 128 bpm

    /// <summary>
    /// One beat length in milliseconds.
    /// </summary>
    private const Double beat_length = 60_000 / bpm; // 468.75 ms

    /// <summary>
    /// One bar length in milliseconds. Its value is 4 * <see cref="beat_length"/>.
    /// </summary>
    private const Double four_beats_bar = beat_length * 4; // 1875 ms

    /// <summary>
    /// Track delay in milliseconds. This value indicates when the first beat of the entire track starts.
    /// </summary>
    private const Double track_delay = 0; // 0ms // 50; // 50 ms

    /// <summary>
    /// Real track beats time total.
    /// </summary>
    /// <remarks>
    /// track.time - offset - track_delay
    /// </remarks>
    private Double time => this.offsetBeatTrack.CurrentTime - this.offset.Value - track_delay;

    private Double timeSinceLastBeat => (this.time % four_beats_bar);

    /// <summary>
    /// Relative position of the movingLine.
    /// </summary>
    private Double relativePosition {
        get {
            // range: 0 ~ 1.0
            var relativePositionOfBar = this.timeSinceLastBeat / four_beats_bar;
            // range: -0.5 ~ +0.5
            return (relativePositionOfBar < 0.5) ? relativePositionOfBar : (relativePositionOfBar - 1);
        }
    }

    protected override void Update() {
        this.movingLine.X = Convert.ToSingle(this.relativePosition < 0.5 ? this.relativePosition : this.relativePosition - 1);
    }

    private void offsetChanged(ValueChangedEvent<Double> e) {
        // TODD: display offset
        this.offsetText.Text = e.NewValue + "ms";
    }

    protected override Boolean OnClick(ClickEvent e) {
        BlinkDiamond tempLine = new() {
            RelativePositionAxes = Axes.X,
            X = (Single)this.relativePosition,
        };

        this.tempDisplayContainer.Add(tempLine);
        tempLine.FadeIn(100).ScaleTo(Vector2.One, 300, Easing.OutQuint).Delay(400).FadeOut(500).Expire();
        return true;
    }

    protected override void Dispose(Boolean isDisposing) {
        this.offsetBeatTrack.Dispose();
        this.offset.ValueChanged -= this.offsetChanged;
        base.Dispose(isDisposing);
    }

    internal partial class ArrowButton : Container {
        private readonly OrientedTriangle triangle;

        public Action<ClickEvent>? TouchDown;

        public ArrowButton(OrientedTriangle.Orientation orientation) {
            this.Anchor = Anchor.Centre;
            this.Origin = Anchor.Centre;
            this.RelativePositionAxes = Axes.Both;
            this.Size = new Vector2(64);
            //this.AutoSizeAxes = Axes.Both;
            this.triangle = new OrientedTriangle(orientation) {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                //Size = new Vector2(64),
            };
            this.Add(this.triangle);
        }

        protected override Boolean OnClick(ClickEvent e) {
            this.TouchDown?.Invoke(e);
            return true;
        }
    }
}
