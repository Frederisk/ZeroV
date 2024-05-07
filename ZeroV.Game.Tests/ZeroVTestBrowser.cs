using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace ZeroV.Game.Tests;

public partial class ZeroVTestBrowser : ZeroVGameBase {

    protected override void LoadComplete() {
        base.LoadComplete();

        this.AddRange([
            new TestBrowser("ZeroV"),
            new CursorContainer()
        ]);
    }

    public override void SetHost(GameHost host) {
        base.SetHost(host);
        host.Window.CursorState |= CursorState.Hidden;
    }
}
