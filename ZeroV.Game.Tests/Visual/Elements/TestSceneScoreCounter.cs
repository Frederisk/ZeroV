using NUnit.Framework;

using ZeroV.Game.Elements.Counters;

namespace ZeroV.Game.Tests.Visual.Elements;

[TestFixture]
public partial class TestSceneScoreCounter : ZeroVTestScene {
    private ScoreCounter scoreCounter = new();

    public TestSceneScoreCounter() {
        this.Add(this.scoreCounter);
    }

    [Test]
    public void TestScoreCounterIncrementing() {
        unchecked {
            this.AddStep("Set score counter to 0", () => this.scoreCounter.Current.Value = 0);
            this.AddStep("Add score counter", () => this.scoreCounter.Current.Value += 1000);
            this.AddStep("Subtract score counter", () => this.scoreCounter.Current.Value -= 1000);
        }
    }

    [Test]
    public void TestScoreCounterHasLargeScore() {
        unchecked {
            this.AddStep("Set score counter to 1000000", () => this.scoreCounter.Current.Value = 1000000);
            this.AddStep("Add large score counter", () => this.scoreCounter.Current.Value += 1000000);
            this.AddStep("Subtract large score counter", () => this.scoreCounter.Current.Value -= 1000000);
        }
    }
}
