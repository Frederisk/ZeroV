using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;

using osuTK;

namespace ZeroV.Game.Elements.Buttons;

public partial class BackButton : BasicButton {

    public BackButton(IScreen screen) {
        this.Text = "< Back";
        this.Size = new Vector2(180, 64);
        this.Action = screen.Exit;
    }
}
