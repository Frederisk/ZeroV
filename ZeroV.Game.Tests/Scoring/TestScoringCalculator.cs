using System;

using NUnit.Framework;

using ZeroV.Game.Scoring;

namespace ZeroV.Game.Tests.Scoring;

[TestFixture]
internal class TestScoringCalculator {
    private ScoringCalculator calculator1024 = null!;
    private ScoringCalculator calculator4 = null!;

    [SetUp]
    public void SetUp() {
        this.calculator1024 = new ScoringCalculator(1024);
        this.calculator4 = new ScoringCalculator(4);
    }

    [Test]
    public void Test1024() {
        for (var i = 0; i < 1024; i++) {
            this.calculator1024.AddTarget(TargetResult.MaxPerfect);
            Console.WriteLine(this.calculator1024.DisplayScoring);
        }
        Assert.IsTrue(this.calculator1024.DisplayScoring is 1_000_000);
    }

    [Test]
    public void Test4() {
        for (var i = 0; i < 4; i++) {
            this.calculator4.AddTarget(TargetResult.MaxPerfect);
            Console.WriteLine(this.calculator4.DisplayScoring);
        }
        Assert.IsTrue(this.calculator4.DisplayScoring is 1_000_000);
    }
}
