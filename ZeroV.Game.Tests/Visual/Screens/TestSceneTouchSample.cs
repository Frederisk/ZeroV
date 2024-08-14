using System;

using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

using osuTK;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestSceneTouchSample : ZeroVTestScene {

    [BackgroundDependencyLoader]
    private void load() {
        this.Children = [
            new Box{
                X= 100,
                Y=300,
                Size= new Vector2(100),
            },
            new BasicButton {
                X = 300,
                Y = 300,
                Size = new Vector2(100),
                Action = () => this.Children[0].Colour = Colour4.Green,
            },
            new MyBox() {
                X = 100,
                Y = 100,
                TouchValue = true,
            },
            new MyBox() {
                X = 150,
                Y = 100,
                TouchValue = true,
            },
            new Pentagon() {
                X = 500,
                Y = 500,
                Size = new Vector2(100),
            },
        ];
    }

    protected override Boolean OnTouchDown(TouchDownEvent e) {
        base.OnTouchDown(e);
        //this.Children[0].Colour = Colour4.Red;
        return true;
    }

    protected override void OnTouchUp(TouchUpEvent e) {
        base.OnTouchUp(e);
        //this.Children[0].Colour = Colour4.Blue;
    }

    private partial class MyBox : Box {
        public Boolean TouchValue { get; set; }
        public Action? Action { get; set; }

        public MyBox() {
            //this.TouchValue = false;
            this.Action = null;
            this.Colour = Colour4.Blue;
            this.Size = new Vector2(100);
            //this.Anchor = Anchor.Centre;
            //this.Origin = Anchor.Centre;
        }

        protected override Boolean OnTouchDown(TouchDownEvent e) {
            base.OnTouchDown(e);
            this.Colour = Colour4.Red;
            return this.TouchValue;
        }

        protected override void OnTouchUp(TouchUpEvent e) {
            base.OnTouchUp(e);
            this.Colour = Colour4.Blue;
        }
    }
}
