using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;

using osuTK;

namespace ZeroV.Game.Elements.Buttons;

public partial class BackButton : BasicButton {

    public BackButton(IScreen screen) {
        this.Text = "◀  BACK";
        this.Size = new Vector2(140, 44);
        this.Margin = new MarginPadding(24);
        this.Masking = true;
        this.CornerRadius = 6;
        this.BorderThickness = 1.5f;
        this.BorderColour = Colour4.FromHex("00d2d3").Opacity(0.5f);
        this.BackgroundColour = Colour4.FromHex("121622").Opacity(0.85f);
        this.HoverColour = Colour4.FromHex("00d2d3").Opacity(0.35f);
        this.FlashColour = Colour4.FromHex("00d2d3");
        this.Action = screen.Exit;
    }
}
