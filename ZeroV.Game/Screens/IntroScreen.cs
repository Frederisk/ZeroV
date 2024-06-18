using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;

using ZeroV.Game.Configs;
using ZeroV.Game.Data;
using ZeroV.Game.Data.IO;
using ZeroV.Game.Data.KeyValueStorage;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Screens;

public partial class IntroScreen : Screen {
    private TextFlowContainer textFlow = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.textFlow = new TextFlowContainer {
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.BottomLeft,
            //Direction = FillDirection.Vertical,
            AutoSizeAxes = Axes.Both,
            Text = "Loading ZeroV...",
        };
        this.InternalChildren = [
            new Box {
                Colour = Colour4.Black,
                // 1366 * 768
                RelativeSizeAxes = Axes.Both,
            },
            this.textFlow,
        ];
    }

    protected override void LoadComplete() {
        base.LoadComplete();
        // TODO: Add cutscenes.
        this.Schedule(async () => {
            Task<List<TrackInfo>> task = this.loadBeatmapsAsync();
            //Task load = this.LoadComponentAsync(new MainScreen(), this.Push); && !load.IsCompletedSuccessfully
            while (!task.IsCompletedSuccessfully) {
                await Task.Delay(Random.Shared.Next(50, 500));
                this.textFlow.AddParagraph("Reading...");
            }
            //this.textFlow.FadeOut(1000).Schedule(this.continueToMain);
            this.Push(new MainScreen());
            // task.Result;
        });
    }

    //[Resolved]
    //IKeyValueStorage keyValueStorage { get; set; } = null!;

    private async Task<List<TrackInfo>> loadBeatmapsAsync() {
        // TODO: TrackInfoList
        IKeyValueStorage keyValueStorage = this.Dependencies.Get<IKeyValueStorage>();
        List<TrackInfo>? trackInfoList = await keyValueStorage.GetAsync<List<TrackInfo>>("TrackInfoList");
        if (trackInfoList is null) {
            String beatmapStoragePath = this.Dependencies.Get<ZeroVConfigManager>().Get<String>(ZeroVSetting.BeatmapStoragePath);
            List<FileInfo> beatmapInfoFileList = BeatmapReader.GetAllMapFile(beatmapStoragePath);
            List<BeatmapWrapper> beatmapWrapperList = beatmapInfoFileList.ConvertAll(BeatmapWrapper.Create);
            trackInfoList = beatmapWrapperList.ConvertAll(i => i.GetTrackInfo());
            await keyValueStorage.SetAsync("TrackInfoList", trackInfoList);
        }
        return trackInfoList;
    }
}
