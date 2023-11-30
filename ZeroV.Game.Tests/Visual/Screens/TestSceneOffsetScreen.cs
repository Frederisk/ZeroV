using System;

using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace ZeroV.Game.Tests.Visual.Screens;

public partial class TestSceneOffsetScreen: ZeroVTestScene {

    public TestSceneOffsetScreen(){
        this.Add(new ScreenStack(new OffsetScreen()) { RelativeSizeAxes = Axes.Both });
    }
}
