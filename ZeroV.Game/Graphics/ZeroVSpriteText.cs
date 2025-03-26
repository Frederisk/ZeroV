using System;

using osu.Framework.Graphics.Sprites;

namespace ZeroV.Game.Graphics;

public partial class ZeroVSpriteText : SpriteText {

    public Single FontSize {
        get => this.Font.Size;
        set => this.Font = this.Font.With(size: value);
    }

    public Boolean IsFixedWidth {
        get => this.Font.FixedWidth;
        set => this.Font = this.Font.With(fixedWidth: value);
    }
}
