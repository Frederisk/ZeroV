using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Platform;

namespace ZeroV.Game.Tests.Visual;

[TestFixture]
public partial class TestSceneZeroVGame : ZeroVTestScene {
    // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
    // You can make changes to classes associated with the tests and they will recompile and update immediately.

    private ZeroVGame game = null!;

    [BackgroundDependencyLoader]
    private void load(GameHost host) {
        this.game = new();
        this.game.SetHost(host);

        this.AddGame(this.game);
    }
}
