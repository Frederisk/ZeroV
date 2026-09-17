using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;

using osuTK;

using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;

namespace ZeroV.Game.Elements;

public partial class ZeroVDropdown<T> : Dropdown<T> {
    protected override DropdownHeader CreateHeader() => new ZeroVDropdownHeader();
    protected override DropdownMenu CreateMenu() => new ZeroVDropdownMenu();

    protected partial class ZeroVDropdownHeader : DropdownHeader {
        private readonly ZeroVSpriteText labelText;
        private readonly Diamond diamondIcon;
        private readonly Box backgroundBox;

        protected override LocalisableString Label {
            get => this.labelText.Text;
            set => this.labelText.Text = value;
        }

        public ZeroVDropdownHeader() {
            this.backgroundBox = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.AliceBlue,
            };
            this.diamondIcon = new Diamond {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = new Vector2(16),
                Margin = new MarginPadding { Left = 4 },
                Colour = Colour4.LightBlue,
            };
            this.labelText = new ZeroVSpriteText {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                //FontSize = 18,
                Colour = Colour4.DarkBlue,
            };
            this.Children = [
                this.backgroundBox,
                new Container {
                    RelativeSizeAxes = Axes.X,
                    Height = 32,
                    Padding = new MarginPadding { Left = 22, Right = 36 },
                    Child = this.labelText,
                },
                this.diamondIcon,
            ];
        }

        protected override DropdownSearchBar CreateSearchBar() => new ZeroVDropdownSearchBar();

        protected partial class ZeroVDropdownSearchBar : DropdownSearchBar {
            protected override TextBox CreateTextBox() => new ZeroVSearchTextBox();

            protected override void PopIn() => this.FadeIn(150, Easing.OutQuint);
            protected override void PopOut() => this.FadeOut(150, Easing.InSine);

            protected partial class ZeroVSearchTextBox : BasicTextBox {
                //public ZeroVSearchTextBox() {
                //    //this.Height = 32;
                //    this.RelativeSizeAxes = Axes.Both;
                //    //this.CornerRadius = 4;
                //    //this.Masking = true;
                //    //this.BackgroundUnfocused = Colour4.FromHex("0e121a");
                //    //this.BackgroundFocused = Colour4.FromHex("182030");
                //}
            }
        }
    }

    protected partial class ZeroVDropdownMenu : DropdownMenu {
        public ZeroVDropdownMenu() {
            //this.Masking = true;
            //this.CornerRadius = 6;
            //this.BorderThickness = 1.5f;
            //this.BorderColour = Colour4.FromHex("00d2d3").Opacity(0.4f);
            this.MaxHeight = 220;
        }

        protected override DrawableDropdownMenuItem CreateDrawableDropdownMenuItem(MenuItem item) => new ZeroVDrawableDropdownMenuItem(item);

        protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction) =>
            new BasicScrollContainer(direction);

        protected override Menu CreateSubMenu() => new BasicMenu(Direction.Vertical);

        private partial class ZeroVDrawableDropdownMenuItem : DrawableDropdownMenuItem {
            public ZeroVDrawableDropdownMenuItem(MenuItem item) : base(item) {
                //this.BackgroundColour = Colour4.FromHex("141824").Opacity(0.95f);
                //this.BackgroundColourHover = Colour4.FromHex("00d2d3").Opacity(0.35f);
                //this.BackgroundColourSelected = Colour4.FromHex("00d2d3").Opacity(0.2f);
            }

            protected override Drawable CreateContent() => new ZeroVSpriteText {
                //FontSize = 17,
                Colour = Colour4.White,
                Margin = new MarginPadding { Top = 8, Bottom = 8, Left = 16, Right = 16 },
            };
        }
    }
}
