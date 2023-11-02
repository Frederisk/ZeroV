using System.ComponentModel;

using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.ZeroV;

public partial class ZeroVInputManager : RulesetInputManager<ZeroVAction> {
    public ZeroVInputManager(RulesetInfo ruleset)
        : base(ruleset, 0, SimultaneousBindingMode.Unique) {
    }
}

public enum ZeroVAction {

    [Description("Move up")]
    MoveUp,

    [Description("Move down")]
    MoveDown,
}
