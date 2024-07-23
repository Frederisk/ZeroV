using System;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;

namespace ZeroV.Game.Elements.Buttons;

public partial class BackButton(IScreen screen) : BasicButton {
    protected override Boolean OnClick(ClickEvent e) {
        screen.Exit();
        return base.OnClick(e);
    }
}
