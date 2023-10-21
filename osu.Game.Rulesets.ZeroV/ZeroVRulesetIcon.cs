using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.ZeroV;

public partial class ZeroVRulesetIcon : Sprite {
    private readonly Ruleset ruleset;

    public ZeroVRulesetIcon(Ruleset ruleset) {
        this.ruleset = ruleset;

        this.Margin = new MarginPadding { Top = 3 };
    }

    [BackgroundDependencyLoader]
    private void load(IRenderer renderer) {
        this.Texture = new TextureStore(renderer, new TextureLoaderStore(this.ruleset.CreateResourceStore()), false).Get("Textures/coin");
    }
}
