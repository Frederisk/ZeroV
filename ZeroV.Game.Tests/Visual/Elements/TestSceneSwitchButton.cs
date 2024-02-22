using osu.Framework.Allocation;
using osu.Framework.Graphics;

using ZeroV.Game.Elements;

namespace ZeroV.Game.Tests.Visual.Elements;

public partial class TestSceneSwitchButton : ZeroVTestScene {

    [BackgroundDependencyLoader]
    private void load() {
        this.ChangeBackgroundColour(Colour4.White);
        this.Add(new SwitchButton {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
        });
    }
}
