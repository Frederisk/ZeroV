using System;

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics.Containers;

using osuTK;

namespace osu.Game.Rulesets.ZeroV.UI;

public partial class ZeroVCharacter : BeatSyncedContainer, IKeyBindingHandler<ZeroVAction> {

    public readonly BindableInt LanePosition = new() {
        Value = 0,
        MinValue = 0,
        MaxValue = ZeroVPlayfield.LANE_COUNT - 1,
    };

    [BackgroundDependencyLoader]
    private void load(TextureStore textures) {
        this.Size = new Vector2(ZeroVPlayfield.LANE_HEIGHT);

        this.Child = new Sprite {
            FillMode = FillMode.Fit,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Scale = new Vector2(1.2f),
            RelativeSizeAxes = Axes.Both,
            Texture = textures.Get("character")
        };

        this.LanePosition.BindValueChanged(e => { this.MoveToY(e.NewValue * ZeroVPlayfield.LANE_HEIGHT); });
    }

    protected override void OnNewBeat(Int32 beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, ChannelAmplitudes amplitudes) {
        if (effectPoint.KiaiMode) {
            Boolean direction = beatIndex % 2 == 1;
            Double duration = timingPoint.BeatLength / 2;

            this.Child.RotateTo(direction ? 10 : -10, duration * 2, Easing.InOutSine);

            this.Child.Animate(i => i.MoveToY(-10, duration, Easing.Out))
                 .Then(i => i.MoveToY(0, duration, Easing.In));
        } else {
            this.Child.ClearTransforms();
            this.Child.RotateTo(0, 500, Easing.Out);
            this.Child.MoveTo(Vector2.Zero, 500, Easing.Out);
        }
    }

    public Boolean OnPressed(KeyBindingPressEvent<ZeroVAction> e) {
        switch (e.Action) {
            case ZeroVAction.MoveUp:
                this.changeLane(-1);
                return true;

            case ZeroVAction.MoveDown:
                this.changeLane(1);
                return true;

            default:
                return false;
        }
    }

    public void OnReleased(KeyBindingReleaseEvent<ZeroVAction> e) {
    }

    private void changeLane(Int32 change) => this.LanePosition.Value = (this.LanePosition.Value + change + ZeroVPlayfield.LANE_COUNT) % ZeroVPlayfield.LANE_COUNT;
}
