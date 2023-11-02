using System;
using System.Collections.Generic;
using System.Linq;

using osu.Framework.Input.StateChanges;
using osu.Game.Replays;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.ZeroV.Replays;

public class ZeroVFramedReplayInputHandler : FramedReplayInputHandler<ZeroVReplayFrame> {
    public ZeroVFramedReplayInputHandler(Replay replay)
        : base(replay) {
    }

    protected override Boolean IsImportant(ZeroVReplayFrame frame) => frame.Actions.Any();

    protected override void CollectReplayInputs(List<IInput> inputs) {
        inputs.Add(new ReplayState<ZeroVAction> {
            PressedActions = this.CurrentFrame?.Actions ?? new List<ZeroVAction>(),
        });
    }
}
