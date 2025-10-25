using System;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;

using osuTK;

namespace ZeroV.Game.Elements.Buttons;
public partial class FlyoutButton: CompositeDrawable {
    public enum FlyoutDirection {
        Up, Down, Left, Right,
    }
    public enum FlyoutAlignment {
        Start, Center, End,
    }

    public LocalisableString Text {
        get => this.button.Text;
        set => this.button.Text = value;
    }

    public FlyoutDirection Direction { get; set; } = FlyoutDirection.Down;

    public FlyoutAlignment Alignment { get; set; } = FlyoutAlignment.Start;

    public Int32 TransitionDuration { get; set; } = 250;

    public required FillFlowContainer MenuItemsContainer { get; init; }
    private BasicButton button  = null!;
    private FlyoutMenuContainer menu = null!;

    public FlyoutButton() {
        this.AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.button = new BasicButton {
            AutoSizeAxes = Axes.Both,
            Action = () => this.menu.ToggleVisibility(),
        };
        this.menu = new FlyoutMenuContainer(this) {
            Child = this.MenuItemsContainer,
        };

        this.InternalChild = new Container {
            AutoSizeAxes = Axes.Both,
            Children = [
                this.button,
                this.menu,
            ],
        };

        this.updateMenuLayout();
    }

    protected override Boolean OnClick(ClickEvent e) => true;

    protected override Boolean OnHover(HoverEvent e) => true;

    public override Boolean Contains(Vector2 screenSpacePos)
        => base.Contains(screenSpacePos) || this.menu.Contains(screenSpacePos);

    private void updateMenuLayout() {
        (this.menu.Origin, this.menu.Anchor) = (this.Direction, this.Alignment) switch {
            (FlyoutDirection.Down, FlyoutAlignment.Start) => (Anchor.TopLeft, Anchor.BottomLeft),
            (FlyoutDirection.Down, FlyoutAlignment.Center) => (Anchor.TopCentre, Anchor.BottomCentre),
            (FlyoutDirection.Down, FlyoutAlignment.End) => (Anchor.TopRight, Anchor.BottomRight),
            (FlyoutDirection.Up, FlyoutAlignment.Start) => (Anchor.BottomLeft, Anchor.TopLeft),
            (FlyoutDirection.Up, FlyoutAlignment.Center) => (Anchor.BottomCentre, Anchor.TopCentre),
            (FlyoutDirection.Up, FlyoutAlignment.End) => (Anchor.BottomRight, Anchor.TopRight),
            (FlyoutDirection.Left, FlyoutAlignment.Start) => (Anchor.TopRight, Anchor.TopLeft),
            (FlyoutDirection.Left, FlyoutAlignment.Center) => (Anchor.CentreRight, Anchor.CentreLeft),
            (FlyoutDirection.Left, FlyoutAlignment.End) => (Anchor.BottomRight, Anchor.BottomLeft),
            (FlyoutDirection.Right, FlyoutAlignment.Start) => (Anchor.TopLeft, Anchor.TopRight),
            (FlyoutDirection.Right, FlyoutAlignment.Center) => (Anchor.CentreLeft, Anchor.CentreRight),
            (FlyoutDirection.Right, FlyoutAlignment.End) => (Anchor.BottomLeft, Anchor.BottomRight),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    private partial class FlyoutMenuContainer: VisibilityContainer {
        private FlyoutButton parent;
        public FlyoutMenuContainer(FlyoutButton parent) {
            this.parent = parent;
            this.AutoSizeAxes = Axes.Both;
        }

        protected override void PopIn() {
            Int32 duration = this.parent.TransitionDuration;
            this.FadeIn(duration, Easing.OutQuint);
            switch (this.parent.Direction) {
                case FlyoutDirection.Down:
                case FlyoutDirection.Up:
                    this.MoveToY(0, duration, Easing.OutQuint);
                    break;
                case FlyoutDirection.Left:
                case FlyoutDirection.Right:
                    this.MoveToX(0, duration, Easing.OutQuint);
                    break;
            }
        }

        protected override void PopOut() {
            Int32 duration = this.parent.TransitionDuration;
            this.FadeOut(duration, Easing.InSine);
            switch (this.parent.Direction) {
                case FlyoutDirection.Up:
                    this.MoveToY(this.DrawSize.Y / 2, duration, Easing.InSine);
                    break;
                case FlyoutDirection.Down:
                    this.MoveToY(-this.DrawSize.Y / 2, duration, Easing.InSine);
                    break;
                case FlyoutDirection.Left:
                    this.MoveToX(this.DrawSize.X/ 2, duration, Easing.InSine);
                    break;
                case FlyoutDirection.Right:
                    this.MoveToX(-this.DrawSize.X / 2, duration, Easing.InSine);
                    break;
            }
        }

        protected override void UpdateState(ValueChangedEvent<Visibility> state) {
            base.UpdateState(state);

            if (state.NewValue is Visibility.Visible) {
                switch (this.parent.Direction) {
                    case FlyoutDirection.Up:
                        this.Y = this.DrawSize.Y / 2;
                        break;
                    case FlyoutDirection.Down:
                        this.Y = -this.DrawSize.Y / 2;
                        break;
                    case FlyoutDirection.Left:
                        this.X = this.DrawSize.X / 2;
                        break;
                    case FlyoutDirection.Right:
                        this.X = -this.DrawSize.X / 2;
                        break;
                }
            }
        }
    }
}
