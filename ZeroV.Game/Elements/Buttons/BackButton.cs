using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;

using osuTK;

namespace ZeroV.Game.Elements.Buttons;

public partial class BackButton : BasicButton {

    public BackButton(IScreen screen) {
        this.Text = "< Back";
        this.Size = new Vector2(140, 44);
        this.Margin = new MarginPadding(24);
        this.Masking = true;
        this.BorderThickness = 2.5f;
        this.BorderColour = Colour4.Cyan;
        this.BackgroundColour = Colour4.LightBlue;
        this.Action = screen.Exit;
    }

    protected override SpriteText CreateText() {
        SpriteText text = base.CreateText();
        text.Colour = Colour4.DeepPink;
        return text;
    }
}
