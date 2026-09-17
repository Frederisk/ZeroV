using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Elements.Buttons;

public partial class BackButton : BasicButton {

    public BackButton(IScreen screen) {
        this.Text = "◀  BACK";
        this.Size = new Vector2(140, 44);
        this.Margin = new MarginPadding(24);
        this.Masking = true;
        this.CornerRadius = 0;
        this.BorderThickness = 1.5f;
        this.BorderColour = ZeroVColour.Cyan;
        this.BackgroundColour = ZeroVColour.BgCard;
        this.HoverColour = ZeroVColour.Cyan.Opacity(0.2f);
        this.FlashColour = ZeroVColour.CyanBright;
        this.Action = screen.Exit;
    }
}
