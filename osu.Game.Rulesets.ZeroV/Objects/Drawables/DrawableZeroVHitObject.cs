using System;
using System.Collections.Generic;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Audio;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.ZeroV.UI;

using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.ZeroV.Objects.Drawables;

public partial class DrawableZeroVHitObject : DrawableHitObject<ZeroVHitObject> {
    private BindableNumber<Int32> currentLane;

    public DrawableZeroVHitObject(ZeroVHitObject hitObject)
        : base(hitObject) {
        this.Size = new Vector2(40);

        this.Origin = Anchor.Centre;
        this.Y = hitObject.Lane * ZeroVPlayfield.LANE_HEIGHT;
    }

    [BackgroundDependencyLoader]
    private void load(ZeroVPlayfield playfield, TextureStore textures) {
        this.AddInternal(new Sprite {
            RelativeSizeAxes = Axes.Both,
            Texture = textures.Get("coin"),
        });

        this.currentLane = playfield.CurrentLane.GetBoundCopy();
    }

    public override IEnumerable<HitSampleInfo> GetSamples() => new[] {
            new HitSampleInfo(HitSampleInfo.HIT_NORMAL)
    };

    protected override void CheckForResult(Boolean userTriggered, Double timeOffset) {
        if (timeOffset >= 0) {
            this.ApplyResult(r => r.Type = this.currentLane.Value == this.HitObject.Lane ? HitResult.Perfect : HitResult.Miss);
        }
    }

    protected override void UpdateHitStateTransforms(ArmedState state) {
        switch (state) {
            case ArmedState.Hit:
                this.ScaleTo(5, 1500, Easing.OutQuint).FadeOut(1500, Easing.OutQuint).Expire();
                break;

            case ArmedState.Miss:
                const Double duration = 1000;
                this.ScaleTo(0.8f, duration, Easing.OutQuint);
                this.MoveToOffset(new Vector2(0, 10), duration, Easing.In);
                this.FadeColour(Color4.Red, duration / 2, Easing.OutQuint).Then().FadeOut(duration / 2, Easing.InQuint).Expire();
                break;
        }
    }
}
