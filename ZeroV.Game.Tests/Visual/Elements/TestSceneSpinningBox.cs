using NUnit.Framework;

using osu.Framework.Graphics;

using ZeroV.Game.Elements;

namespace ZeroV.Game.Tests.Visual.Elements;

[TestFixture]
public partial class TestSceneSpinningBox : ZeroVTestScene {
    // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
    // You can make changes to classes associated with the tests and they will recompile and update immediately.

    public TestSceneSpinningBox() {
        this.Add(new SpinningBox {
            Anchor = Anchor.Centre,
        });
    }
}
