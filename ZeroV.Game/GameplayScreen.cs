using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Elements;

namespace ZeroV.Game;
internal partial class GameplayScreen : Screen {

    public GameplayScreen() {
        this.Anchor = Anchor.BottomCentre;
        this.Origin = Anchor.BottomCentre;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = new Drawable[] {
            new PlayfieldBackground(),
            new Orbit(){
                Position = new Vector2(0,0),
            },
        };
    }




}
