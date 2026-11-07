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
    //private SingleTrackResourceStore store;

    public readonly Track Track;

    public TrackLoader(FileInfo file, AudioManager audioManager) {
        NativeStorage storage = new(file.Directory!.FullName);
        this.backedStore = new StorageBackedResourceStore(storage);
        this.trackStore = audioManager.GetTrackStore(this.backedStore);
        this.Track = this.trackStore.Get(file.Name);
        //this.store = new SingleTrackResourceStore(file);
        //this.trackStore = audioManager.GetTrackStore(this.store);
        //this.Track = this.trackStore.Get(file.Name);
    }

    protected void Dispose(Boolean disposing) {
        if (this.disposedValue) {
            return;
        }

        if (disposing) {
            this.Track.Dispose();
            this.trackStore.Dispose();
            this.backedStore.Dispose();
        }

        this.disposedValue = true;
    }

    public void Dispose() {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    //internal class SingleTrackResourceStore : IResourceStore<Byte[]?> {
    //    private readonly FileInfo fileInfo;

    //    public SingleTrackResourceStore(FileInfo fileInfo) {
    //        this.fileInfo = fileInfo;
    //    }

    //    public Byte[]? Get(String name) {
    //        if (name != this.fileInfo.Name) {
    //            return null;
    //        }
    //        return File.ReadAllBytes(this.fileInfo.FullName);
    //    }

    //    public Task<Byte[]?> GetAsync(String name, CancellationToken cancellationToken = default) {
    //        if (name != this.fileInfo.Name) {
    //            return Task.FromResult<Byte[]?>(null);
    //        }
    //        return File.ReadAllBytesAsync(this.fileInfo.FullName, cancellationToken);
    //    }

    //    public Stream GetStream(String name) {
    //        if (name != this.fileInfo.Name) {
    //            return Stream.Null;
    //        }
    //        return File.OpenRead(this.fileInfo.FullName);
    //    }

    //    public IEnumerable<String> GetAvailableResources() {
    //        yield return this.fileInfo.Name;
    //    }

    //    public void Dispose() {
    //        // No resources to dispose
    //    }
    //}
}

//public static Track LoadTrack(FileInfo file, AudioManager audioManager) {
//    NativeStorage storage = new(file.Directory!.FullName);
//    using StorageBackedResourceStore store = new(storage);
//    ITrackStore trackStore = audioManager.GetTrackStore(store);
//    return trackStore.Get(file.Name);
//}
