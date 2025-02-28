using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;

namespace ZeroV.Game.Elements.Buttons;

public partial class BackButton : BasicButton {

    public BackButton(IScreen screen) {
        this.Action = screen.Exit;
    }
}
