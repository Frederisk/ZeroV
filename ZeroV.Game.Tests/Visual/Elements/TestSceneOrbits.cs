using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

using ZeroV.Game.Elements;

namespace ZeroV.Game.Tests.Visual.Elements;

public partial class TestSceneOrbits : ZeroVTestScene {
    private Container<Orbit> orbits = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.orbits = new Container<Orbit>() {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.BottomCentre,
            Children = [
                // new OrbitWithColorTest { X = 0, Width = 128 },
                // new OrbitWithColorTest { X = 100, Width = 256 },
            ],
        };

        this.Children = [
            this.orbits,
        ];
    }
}

// internal partial class OrbitWithColorTest : Orbit {
//     private Colour4[] colors = [
//         Colour4.White,
//         Colour4.Red,
//         Colour4.Orange,
//         Colour4.Yellow,
//         Colour4.Green,
//         Colour4.Cyan,
//         Colour4.Blue,
//         Colour4.Purple,
//     ];

//     private void updateColor() {
//         var colorIndex = this.TouchCount % 8;
//         this.InnerBox.Colour = this.colors[colorIndex];
//     }

//     public override void TouchEnter(Boolean isTouchDown) {
//         base.TouchEnter(isTouchDown);
//         this.updateColor();
//         throw new Exception();
//     }

//     public override void TouchLeave() {
//         base.TouchLeave();
//         this.updateColor();
//         throw new Exception();
//     }

// }
