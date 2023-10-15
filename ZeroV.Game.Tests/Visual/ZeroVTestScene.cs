using osu.Framework.Testing;

namespace ZeroV.Game.Tests.Visual;

public partial class ZeroVTestScene : TestScene {
    protected override ITestSceneTestRunner CreateRunner() => new ZeroVTestSceneTestRunner();

    private partial class ZeroVTestSceneTestRunner : ZeroVGameBase, ITestSceneTestRunner {
        private TestSceneTestRunner.TestRunner runner;

        protected override void LoadAsyncComplete() {
            base.LoadAsyncComplete();
            this.Add(this.runner = new TestSceneTestRunner.TestRunner());
        }

        public void RunTestBlocking(TestScene test) => this.runner.RunTestBlocking(test);
    }
}
