using System.Collections.Generic;

using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.ZeroV.Replays;

public class ZeroVReplayFrame : ReplayFrame {
    public List<ZeroVAction> Actions = new();

    public ZeroVReplayFrame(ZeroVAction? button = null) {
        if (button.HasValue) {
            this.Actions.Add(button.Value);
        }
    }
}
