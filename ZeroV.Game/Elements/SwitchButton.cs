using System;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;

using osuTK;

namespace ZeroV.Game.Elements;

public partial class SwitchButton : Checkbox {

    private Drawable innerContainer = null!;
    private Drawable innerDiamond = null!;

    public SwitchButton() {
        this.Size = new Vector2(100, 37);
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChild = new Container {
            RelativeSizeAxes = Axes.Both,
            Children = [
                // Background
                new BufferedContainer {
                    RelativeSizeAxes = Axes.Both,
                    Children = [
                        new Box {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Black,
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
                                AlphaEquation = BlendingEquation.Add,
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
                                Colour = Colour4.White,
                                Size = new Vector2(0.5f, 1),
                            },
                            Blending = new BlendingParameters {
                                AlphaEquation = BlendingEquation.Add,
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
                // Foreground
                this.innerContainer = new Container {
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                    // ((74/2)-(40/2))/2 = 8.5
                    // <see cref="ZeroV.Game.Graphics.Shapes.BlinkDiamond"/>
                    Padding = new MarginPadding(8.5f),
                    Child = this.innerDiamond = new Diamond {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.White,
                    },
                },
            ],
        };
    }


    protected override void LoadComplete() {
        base.LoadComplete();
        this.Current.BindValueChanged(this.updateState, true);
        this.FinishTransforms(true);
    }

    private void updateState(ValueChangedEvent<Boolean> state) {
        this.innerContainer.MoveToX(state.NewValue ? this.DrawWidth - this.innerContainer.DrawWidth : 0, 250, Easing.OutQuint);
        this.innerDiamond.RotateTo(state.NewValue ? 720 : 0, 400, Easing.OutQuint);
    }

}
