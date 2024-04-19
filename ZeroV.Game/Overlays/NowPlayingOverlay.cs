using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

using osuTK.Graphics;

namespace ZeroV.Game.Overlays;

[Cached]
public partial class NowPlayingOverlay : FocusedOverlayContainer {
    private const Double transition_length = 800;

    private FillFlowContainer container = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.container = new FillFlowContainer() {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new osuTK.Vector2(0, 2)
        };

        this.Child = new BasicScrollContainer() {
            RelativeSizeAxes = Axes.Both,
            Child = this.container
        };

        this.container.Add(new ListItem() { Colour = Color4.Red });
        this.container.Add(new ListItem() { Colour = Color4.Orange });
        this.container.Add(new ListItem() { Colour = Color4.Yellow });
        this.container.Add(new ListItem() { Colour = Color4.Green });
        this.container.Add(new ListItem() { Colour = Color4.Cyan });
        this.container.Add(new ListItem() { Colour = Color4.Blue });
        this.container.Add(new ListItem() { Colour = Color4.Purple });
    }

    private ListItem? selectedItem;
    public void OnSelect(ListItem item) {
        this.selectedItem?.OnSelectCancel();
        this.selectedItem = item;
    }

    protected override void PopIn() {
        this.FadeIn(transition_length, Easing.OutQuint);
    }
    protected override void PopOut() {
        this.FadeOut(transition_length, Easing.OutQuint);
    }

    public partial class ListItem : CompositeDrawable {

        private Boolean selected;

        [Resolved]
        private NowPlayingOverlay baseOverlay { get; set; } = null!;

        [BackgroundDependencyLoader]
        private void load() {
            this.RelativeSizeAxes = Axes.X;
            this.Height = 300;

            this.Masking = true;
            this.BorderColour = Color4.Black;

            this.AddInternal(new Box() { RelativeSizeAxes = Axes.Both });
        }

        public void OnSelect() {
            if (!this.selected) {
                this.baseOverlay.OnSelect(this);

                this.selected = true;
                this.BorderThickness = 3;
            } else {
                //TODO: Confirm seelect
                this.baseOverlay.Hide();
            }
        }
        public void OnSelectCancel() {
            this.selected = false;
            this.BorderThickness = 0;
        }

        protected override Boolean OnClick(ClickEvent e) {
            this.OnSelect();
            return base.OnClick(e);
        }
    }
}
