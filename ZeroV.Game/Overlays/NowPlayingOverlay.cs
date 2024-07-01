using System;
using System.Linq;
using System.Reflection;

using FFmpeg.AutoGen;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

using osuTK.Graphics;

using ZeroV.Game.Data;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Overlays;

[Cached]
public partial class NowPlayingOverlay : FocusedOverlayContainer {
    private const Double transition_length = 800;

    private FillFlowContainer container = null!;

    [Resolved]
    private TrackInfoProvider beatmapWrapperProvider { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.Both;

        this.container = new FillFlowContainer() {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new osuTK.Vector2(0, 0)
        };

        this.Child = new BasicScrollContainer() {
            RelativeSizeAxes = Axes.Both,
            Child = this.container
        };
    }

    private MapInfoListItem? selectedItem;
    public void OnSelect(MapInfoListItem item) {
        this.selectedItem?.OnSelectCancel();
        this.selectedItem = item;
    }

    protected override void PopIn() {
        this.container.Clear();
        foreach (TrackInfo item in this.beatmapWrapperProvider.TrackInfoList) {
            this.container.Add(new TrackInfoListItem(item));
        }

        this.FadeIn(transition_length, Easing.OutQuint);
    }
    protected override void PopOut() {
        this.FadeOut(transition_length, Easing.OutQuint);
    }

    [Cached]
    public partial class TrackInfoListItem(TrackInfo info) : CompositeDrawable {
        private Boolean isExpanded;
        private FillFlowContainer container = null!;

        public TrackInfo TrackInfo => info;

        [BackgroundDependencyLoader]
        private void load() {
            this.Masking = true;
            this.BorderColour = Color4.White;
            this.RelativeSizeAxes = Axes.X;
            this.AutoSizeAxes = Axes.Y;

            this.container = new FillFlowContainer() {
                RelativeSizeAxes = Axes.X,
                Height = 0,
                Direction = FillDirection.Vertical,
                Spacing = new osuTK.Vector2(0, 0),
                Margin = new MarginPadding(0) {
                    Left = 100
                }
            };

            this.AddInternal(new FillFlowContainer() {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Children = [
                    new TrackInfoListItemHeader(),
                    this.container
                ]
            });

            foreach (MapInfo item in info.Maps) {
                this.container.Add(new MapInfoListItem(item));
            }
        }

        public Boolean IsExpanded {
            get => this.isExpanded;
            set {
                if(value) {
                    this.container.AutoSizeAxes = Axes.Y;
                } else {
                    this.container.AutoSizeAxes = Axes.None;
                    this.container.Height = 0;
                }
                this.isExpanded = value;
            }
        }

        public partial class TrackInfoListItemHeader : CompositeDrawable {

            [Resolved]
            private TrackInfoListItem listItem { get; set; } = null!;

            [BackgroundDependencyLoader]
            private void load() {
                this.RelativeSizeAxes = Axes.X;
                this.Height = 200;

                this.AddInternal(new Box() {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Blue
                });
                this.AddInternal(new SpriteText() {
                    Origin = Anchor.TopCentre,
                    Anchor = Anchor.TopCentre,
                    Text = this.listItem.TrackInfo.Title,
                    Colour = Color4.Black,
                    Font = FontUsage.Default.With(size: 52)
                });
            }

            protected override Boolean OnClick(ClickEvent e) {
                this.listItem.IsExpanded = !this.listItem.IsExpanded;
                return base.OnClick(e);
            }
        }
    }

    public partial class MapInfoListItem(MapInfo mapInfo) : CompositeDrawable {
        private Boolean selected;

        [Resolved]
        private NowPlayingOverlay baseOverlay { get; set; } = null!;

        [BackgroundDependencyLoader]
        private void load() {
            this.Masking = true;
            this.BorderColour = Color4.White;
            this.RelativeSizeAxes = Axes.X;
            this.Height = 50;

            this.AddInternal(new Box() {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Green
            });
            this.AddInternal(new SpriteText() {
                Origin = Anchor.TopCentre,
                Anchor = Anchor.TopCentre,
                Text = mapInfo.Difficulty.ToString(),
                Colour = Color4.Black,
                Font = FontUsage.Default.With(size: 52)
            });
        }

        public void OnSelect() {
            if (!this.selected) {
                this.baseOverlay.OnSelect(this);

                this.selected = true;
                this.BorderThickness = 3;
            } else {
                //TODO: Confirm select
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
