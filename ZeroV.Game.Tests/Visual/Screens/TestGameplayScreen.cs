using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZeroV.Game.Screens;

using NUnit.Framework;
using osu.Framework.Screens;
using osu.Framework.Graphics; 

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestGameplayScreen: ZeroVTestScene {

    public TestGameplayScreen() {
        this.Add(new ScreenStack(new GameplayScreen { RelativeSizeAxes=Axes.Both }) { RelativeSizeAxes = Axes.Both });
    }
}
