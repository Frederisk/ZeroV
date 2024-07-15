using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Objects;
using ZeroV.Game.Data;

namespace ZeroV.Game.Screens;

[Cached]
public partial class PlaySongSelectScreen : Screen {
    private Sprite background = null!;
    private FillFlowContainer container = null!;

    [Resolved]
    private TrackInfoProvider beatmapWrapperProvider { get; set; } = null!;
    [Resolved]
    private LargeTextureStore textureStore { get; set; } = null!;

    [BackgroundDependencyLoader]
    private async void load() {
        this.RelativeSizeAxes = Axes.Both;

        this.background = new Sprite() {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            FillMode = FillMode.Fill
        };
        this.AddInternal(this.background);

        this.container = new FillFlowContainer() {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0, 10),
            Padding = new MarginPadding(10)
        };

        foreach (TrackInfo item in await this.beatmapWrapperProvider.GetTrackInfoListAsync() ?? []) {
            this.container.Add(new TrackInfoListItem(item));
        }

        var child = new BasicScrollContainer() {
            RelativeSizeAxes = Axes.Both,
            Child = this.container
        };
        this.AddInternal(child);
    }

    private MapInfoListItem? selectedItem;
    public void OnSelect(MapInfoListItem item) {
        this.selectedItem?.OnSelectCancel();
        this.selectedItem = item;
    }

    private TrackInfoListItem? expandedItem;
    public void OnExpanded(TrackInfoListItem item) {
        if (this.expandedItem != item && this.expandedItem != null) {
            this.expandedItem.IsExpanded = false;
        }
        this.expandedItem = item;

        //var trackInfo = item.TrackInfo;
        //TODO: TrackInfo Background
        Texture? texture = this.textureStore.Get("test-background.webp");
        this.background.Texture = texture;
    }

    public void ConfirmSelect() {
        //TODO: Confirm select
    }

    [Cached]
    public partial class TrackInfoListItem(TrackInfo info) : CompositeDrawable {
        private Boolean isExpanded;
        private FillFlowContainer container = null!;

        public TrackInfo TrackInfo => info;

        [BackgroundDependencyLoader]
        private void load() {
            this.Masking = true;
            this.BorderColour = Colour4.White;
            this.RelativeSizeAxes = Axes.X;
            this.AutoSizeAxes = Axes.Y;

            this.container = new FillFlowContainer() {
                RelativeSizeAxes = Axes.X,
                Height = 0,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 0),
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
                if (value) {
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
            private PlaySongSelectScreen songSelect { get; set; } = null!;

            [Resolved]
            private TrackInfoListItem listItem { get; set; } = null!;

            [BackgroundDependencyLoader]
            private void load() {
                this.RelativeSizeAxes = Axes.X;
                this.Height = 100;

                this.AddInternal(new Box() {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Blue
                });

                var trackInfo = this.listItem.TrackInfo;
                this.AddInternal(new SpriteText() {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Text = $"{trackInfo.Title} - {trackInfo.Artists} - {trackInfo.Album}",
                    Colour = Colour4.Black,
                    Font = FontUsage.Default.With(size: 52)
                });
            }

            protected override Boolean OnClick(ClickEvent e) {
                this.listItem.IsExpanded = !this.listItem.IsExpanded;
                if (this.listItem.IsExpanded) {
                    this.songSelect.OnExpanded(this.listItem);
                }
                return base.OnClick(e);
            }
        }
    }

    public partial class MapInfoListItem(MapInfo mapInfo) : CompositeDrawable {
        private Boolean selected;

        public MapInfo MapInfo => mapInfo;

        [Resolved]
        private PlaySongSelectScreen songSelect { get; set; } = null!;

        [BackgroundDependencyLoader]
        private void load() {
            this.Masking = true;
            this.BorderColour = Colour4.White;
            this.RelativeSizeAxes = Axes.X;
            this.Height = 50;
            this.Margin = new MarginPadding(0) {
                Top = 5
            };

            this.AddInternal(new Box() {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Green
            });
            this.AddInternal(new SpriteText() {
                Origin = Anchor.TopCentre,
                Anchor = Anchor.TopCentre,
                Text = $"Difficulty: {mapInfo.Difficulty}",
                Colour = Colour4.Black,
                Font = FontUsage.Default.With(size: 52)
            });
        }

        public void OnSelect() {
            if (!this.selected) {
                this.songSelect.OnSelect(this);

                this.selected = true;
                this.BorderThickness = 3;
            } else {
                this.songSelect.ConfirmSelect();
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
