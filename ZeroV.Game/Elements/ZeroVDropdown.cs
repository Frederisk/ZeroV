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
            this.CornerRadius = 6;
            this.BorderThickness = 1.5f;
            this.BorderColour = Colour4.FromHex("00d2d3").Opacity(0.45f);

            this.Foreground.Alpha = 0;
            this.Children = [
                this.backgroundBox = new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.FromHex("121622").Opacity(0.92f),
                },
                new Container {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Left = 16, Right = 36 },
                    Child = this.labelText = new ZeroVSpriteText {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        FontSize = 18,
                        Colour = Colour4.White,
                    },
                },
                this.diamondIcon = new Diamond {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Size = new Vector2(10),
                    Margin = new MarginPadding { Right = 14 },
                    Colour = Colour4.FromHex("00d2d3"),
                },
            ];
        }

        protected override Boolean OnHover(HoverEvent e) {
            this.BorderColour = Colour4.FromHex("00d2d3");
            this.backgroundBox.FadeColour(Colour4.FromHex("1b2234").Opacity(0.95f), 150, Easing.OutQuint);
            this.diamondIcon.RotateTo(45, 200, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e) {
            this.BorderColour = Colour4.FromHex("00d2d3").Opacity(0.45f);
            this.backgroundBox.FadeColour(Colour4.FromHex("121622").Opacity(0.92f), 150, Easing.OutQuint);
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
                    this.CornerRadius = 4;
                    this.Masking = true;
                    this.BackgroundUnfocused = Colour4.FromHex("0e121a");
                    this.BackgroundFocused = Colour4.FromHex("182030");
                }
            }
        }
    }

    public partial class ZeroVDropdownMenu : DropdownMenu {
        public ZeroVDropdownMenu() {
            this.Masking = true;
            this.CornerRadius = 6;
            this.BorderThickness = 1.5f;
            this.BorderColour = Colour4.FromHex("00d2d3").Opacity(0.4f);
            this.MaxHeight = 220;
        }

        protected override DrawableDropdownMenuItem CreateDrawableDropdownMenuItem(MenuItem item) => new ZeroVDrawableDropdownMenuItem(item);

        protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction) =>
            new BasicScrollContainer(direction);

        protected override Menu CreateSubMenu() => new BasicMenu(Direction.Vertical);

        private partial class ZeroVDrawableDropdownMenuItem : DrawableDropdownMenuItem {
            public ZeroVDrawableDropdownMenuItem(MenuItem item) : base(item) {
                this.BackgroundColour = Colour4.FromHex("141824").Opacity(0.95f);
                this.BackgroundColourHover = Colour4.FromHex("00d2d3").Opacity(0.35f);
                this.BackgroundColourSelected = Colour4.FromHex("00d2d3").Opacity(0.2f);
            }

            protected override Drawable CreateContent() => new ZeroVSpriteText {
                FontSize = 17,
                Colour = Colour4.White,
                Margin = new MarginPadding { Top = 8, Bottom = 8, Left = 16, Right = 16 },
            };
        }
    }
}
