using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

using ZeroV.Game.Graphics;
using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Screens.PlaySongSelect.ListItems;
public partial class ResultInfoListItem : CompositeDrawable {

    private ResultInfo result;

    public ResultInfoListItem(ResultInfo result) {
        this.result = result;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.Margin = new MarginPadding(3);
        this.RelativeSizeAxes = Axes.X;
        this.AutoSizeAxes = Axes.Y;
        this.InternalChildren = [
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Gray.Opacity(0.2f),
            },
            new ZeroVSpriteText {
                FontSize = 64,
                Text = this.result.Scoring.ToDisplayScoring(),
                Colour = this.result.ToResultColour(),
            }
        ];
    }
}
