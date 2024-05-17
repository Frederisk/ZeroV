using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

using osuTK.Graphics;

using ZeroV.Game.Data;

namespace ZeroV.Game.Overlays;

[Cached]
public partial class NowPlayingOverlay : FocusedOverlayContainer {
    private const Double transition_length = 800;

    private FillFlowContainer container = null!;

    [Resolved]
    private BeatmapWrapperProvider beatmapWrapperProvider { get; set; } = null!;

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
    }

    private ListItem? selectedItem;
    public void OnSelect(ListItem item) {
        this.selectedItem?.OnSelectCancel();
        this.selectedItem = item;
    }

    protected override void PopIn() {
        this.container.Clear();
        foreach (BeatmapWrapper item in beatmapWrapperProvider.BeatmapWrappers) {
            this.container.Add(new ListItem(item) { Colour = Color4.Blue });
        }

        this.FadeIn(transition_length, Easing.OutQuint);
    }
    protected override void PopOut() {
        this.FadeOut(transition_length, Easing.OutQuint);
    }

    public partial class ListItem(BeatmapWrapper beatmap) : CompositeDrawable {
        private Boolean selected;

        [Resolved]
        private NowPlayingOverlay baseOverlay { get; set; } = null!;

        [BackgroundDependencyLoader]
        private void load() {
            var trackinfo = beatmap.GetTrackInfo();

            this.RelativeSizeAxes = Axes.X;
            this.Height = 300;

            this.Masking = true;
            this.BorderColour = Color4.Black;

            this.AddInternal(new Box() { RelativeSizeAxes = Axes.Both });
            this.AddInternal(new SpriteText() {
                Text = trackinfo.Title
            });
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
