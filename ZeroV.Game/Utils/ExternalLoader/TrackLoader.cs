using System;
using System.IO;

using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;

namespace ZeroV.Game.Utils.ExternalLoader;

public class TrackLoader : IDisposable {
    private Boolean disposedValue = false;
    private StorageBackedResourceStore backedStore;
    private ITrackStore trackStore;

    public Track Track { get; init; }

    public TrackLoader(FileInfo file, AudioManager audioManager) {
        NativeStorage storage = new(file.Directory!.FullName);
        this.backedStore = new(storage);
        this.trackStore = audioManager.GetTrackStore(this.backedStore);
        this.Track = this.trackStore.Get(file.Name);
    }

    protected virtual void Dispose(Boolean disposing) {
        if (!this.disposedValue) {
            if (disposing) {
                this.Track?.Dispose();
                this.trackStore?.Dispose();
                this.backedStore?.Dispose();
            }
            this.disposedValue = true;
        }
    }

    public void Dispose() {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

//public static Track LoadTrack(FileInfo file, AudioManager audioManager) {
//    NativeStorage storage = new(file.Directory!.FullName);
//    using StorageBackedResourceStore store = new(storage);
//    ITrackStore trackStore = audioManager.GetTrackStore(store);
//    return trackStore.Get(file.Name);
//}
