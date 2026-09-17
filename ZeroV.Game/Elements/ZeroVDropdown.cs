using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK;
using osuTK.Graphics;
using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Elements;

public partial class ZeroVDropdown<T> : Dropdown<T> {
    protected override DropdownMenu CreateMenu() => new ZeroVDropdownMenu();
    protected override DropdownHeader CreateHeader() => new ZeroVDropdownHeader();

    public partial class ZeroVDropdownHeader : DropdownHeader {
        private readonly SpriteText labelText;
        private readonly Diamond diamondIcon;
        private readonly Box backgroundBox;

        protected override LocalisableString Label {
            get => this.labelText.Text;
            set => this.labelText.Text = value;
        }

        public ZeroVDropdownHeader() {
            this.AutoSizeAxes = Axes.None;
            this.Height = 42;
            this.Masking = true;
            this.CornerRadius = 0;
            this.BorderThickness = 1.5f;
            this.BorderColour = ZeroVColour.Cyan.Opacity(0.5f);

            this.Foreground.Alpha = 0;
            this.Children = [
                this.backgroundBox = new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = ZeroVColour.BgCard,
                },
                new Container {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Left = 16, Right = 36 },
                    Child = this.labelText = new ZeroVSpriteText {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        FontSize = 18,
                        Colour = ZeroVColour.TextDark,
                    },
                },
                this.diamondIcon = new Diamond {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Size = new Vector2(10),
                    Margin = new MarginPadding { Right = 14 },
                    Colour = ZeroVColour.Cyan,
                },
            ];
        }

        protected override Boolean OnHover(HoverEvent e) {
            this.BorderColour = ZeroVColour.Cyan;
            this.backgroundBox.FadeColour(ZeroVColour.BgCardHover, 150, Easing.OutQuint);
            this.diamondIcon.RotateTo(45, 200, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e) {
            this.BorderColour = ZeroVColour.Cyan.Opacity(0.5f);
            this.backgroundBox.FadeColour(ZeroVColour.BgCard, 150, Easing.OutQuint);
            this.diamondIcon.RotateTo(0, 200, Easing.OutQuint);
            base.OnHoverLost(e);
        }

        protected override DropdownSearchBar CreateSearchBar() => new ZeroVDropdownSearchBar();

        private partial class ZeroVDropdownSearchBar : DropdownSearchBar {
            protected override TextBox CreateTextBox() => new ZeroVSearchTextBox();

            protected override void PopIn() => this.FadeIn(150, Easing.OutQuint);
            protected override void PopOut() => this.FadeOut(150, Easing.InSine);

            private partial class ZeroVSearchTextBox : BasicTextBox {
                public ZeroVSearchTextBox() {
                    this.Height = 32;
                    this.RelativeSizeAxes = Axes.X;
                    this.CornerRadius = 0;
                    this.Masking = true;
                    this.BackgroundUnfocused = ZeroVColour.BgLight;
                    this.BackgroundFocused = Colour4.White;
                }
            }
        }
    }

    public partial class ZeroVDropdownMenu : DropdownMenu {
        public ZeroVDropdownMenu() {
            this.Masking = true;
            this.CornerRadius = 0;
            this.BorderThickness = 1.5f;
            this.BorderColour = ZeroVColour.Cyan;
            this.MaxHeight = 220;
        }

        protected override DrawableDropdownMenuItem CreateDrawableDropdownMenuItem(MenuItem item) => new ZeroVDrawableDropdownMenuItem(item);

        protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction) =>
            new BasicScrollContainer(direction);

        protected override Menu CreateSubMenu() => new BasicMenu(Direction.Vertical);

        private partial class ZeroVDrawableDropdownMenuItem : DrawableDropdownMenuItem {
            public ZeroVDrawableDropdownMenuItem(MenuItem item) : base(item) {
                this.BackgroundColour = ZeroVColour.BgCard;
                this.BackgroundColourHover = ZeroVColour.Cyan.Opacity(0.18f);
                this.BackgroundColourSelected = ZeroVColour.Cyan.Opacity(0.12f);
            }

            protected override Drawable CreateContent() => new ZeroVSpriteText {
                FontSize = 17,
                Colour = ZeroVColour.TextDark,
                Margin = new MarginPadding { Top = 8, Bottom = 8, Left = 16, Right = 16 },
            };
        }
    }
}
