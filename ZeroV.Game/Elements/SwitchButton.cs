using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;

using osuTK;

namespace ZeroV.Game.Elements;

public partial class SwitchButton : Checkbox {

    public SwitchButton() {
        this.Size = new Vector2(150, 50);
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChild = new Container {
            RelativeSizeAxes = Axes.Both,
            Children = [
                new Box{
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Yellow,
                },
                // Background
                new BufferedContainer {
                    RelativeSizeAxes = Axes.Both,
                    Children = [
                        new Box {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Red,
                        },
                        new Container {
                            Origin = Anchor.CentreLeft,
                            Anchor = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.Both,
                            FillMode = FillMode.Fit,
                            Child = new Box {
                                Origin = Anchor.CentreLeft,
                                Anchor = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.Both,
                                Colour = Colour4.White,
                                Size = new Vector2(0.5f, 1),
                            },
                            Blending = new BlendingParameters {
                                SourceAlpha = BlendingType.Zero,
                                DestinationAlpha = BlendingType.OneMinusSrcAlpha
                            },
                        },
                        new Container {
                            Origin = Anchor.CentreRight,
                            Anchor = Anchor.CentreRight,
                            RelativeSizeAxes = Axes.Both,
                            FillMode = FillMode.Fit,
                            Child = new Box {
                                Origin = Anchor.CentreRight,
                                Anchor = Anchor.CentreRight,
                                RelativeSizeAxes = Axes.Both,
                                Colour = Colour4.Transparent,
                                Size = new Vector2(0.5f, 1),
                            },
                            Blending = new BlendingParameters {
                                SourceAlpha = BlendingType.Zero,
                                DestinationAlpha = BlendingType.OneMinusSrcAlpha
                            },
                        },
                    ],
                },
                new Diamond {
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                    Colour = Colour4.Black,
                },
                new Diamond {
                    Origin = Anchor.CentreRight,
                    Anchor = Anchor.CentreRight,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                    Colour = Colour4.Black,
                },
            ],
        };
    }
}
