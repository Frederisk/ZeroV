using System;

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.ZeroV.Objects;
using osu.Game.Rulesets.ZeroV.UI;

namespace osu.Game.Rulesets.ZeroV.Replays;

public class ZeroVAutoGenerator : AutoGenerator<ZeroVReplayFrame> {
    public new Beatmap<ZeroVHitObject> Beatmap => (Beatmap<ZeroVHitObject>)base.Beatmap;

    public ZeroVAutoGenerator(IBeatmap beatmap)
        : base(beatmap) {
    }

    protected override void GenerateFrames() {
        var currentLane = 0;

        this.Frames.Add(new ZeroVReplayFrame());

        foreach (ZeroVHitObject hitObject in this.Beatmap.HitObjects) {
            if (currentLane == hitObject.Lane) {
                continue;
            }

            Int32 totalTravel = Math.Abs(hitObject.Lane - currentLane);
            ZeroVAction direction = hitObject.Lane > currentLane ? ZeroVAction.MoveDown : ZeroVAction.MoveUp;

            Double time = hitObject.StartTime - 5;

            if (totalTravel == ZeroVPlayfield.LANE_COUNT - 1) {
                this.addFrame(time, direction == ZeroVAction.MoveDown ? ZeroVAction.MoveUp : ZeroVAction.MoveDown);
            } else {
                time -= totalTravel * KEY_UP_DELAY;

                for (var i = 0; i < totalTravel; i++) {
                    this.addFrame(time, direction);
                    time += KEY_UP_DELAY;
                }
            }

            currentLane = hitObject.Lane;
        }
    }

    private void addFrame(Double time, ZeroVAction direction) {
        this.Frames.Add(new ZeroVReplayFrame(direction) { Time = time });
        this.Frames.Add(new ZeroVReplayFrame { Time = time + KEY_UP_DELAY }); //Release the keys as well
    }
}
