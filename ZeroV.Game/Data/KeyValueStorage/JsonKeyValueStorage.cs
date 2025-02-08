using System;
using System.Buffers;
using System.IO;
using System.Text.Json;

using osu.Framework.Logging;
using osu.Framework.Platform;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Data.KeyValueStorage;

public partial class JsonKeyValueStorage : IKeyValueStorage {
    private readonly String folder_path = ZeroVPath.JSON_KEY_VALUE_STORAGE_PATH;
    private readonly SearchValues<Char> invalidFileNameChars = SearchValues.Create(Path.GetInvalidFileNameChars());

    protected Storage Storage { get; private set; } = null!;

    //[BackgroundDependencyLoader]
    //private void load(Storage storage) {
    //    if (!storage.ExistsDirectory(folder_path)) {
    //        this.Storage = storage.GetStorageForDirectory(folder_path);
    //    }
    //}

    public JsonKeyValueStorage(Storage storage) {
        //if (!storage.ExistsDirectory(folder_path)) {
        this.Storage = storage.GetStorageForDirectory(this.folder_path);
        //}
    }

    //public async ValueTask<T?> GetAsync<T>(String? key) {
    //    if (String.IsNullOrWhiteSpace(key) || key.AsSpan().ContainsAny(this.invalidFileNameChars)) {
    //        throw new ArgumentException("Invalid key.", nameof(key));
    //    }

    //    var fileName = $"{key}.json";
    //    if (!this.Storage.Exists(fileName)) {
    //        return default;
    //    }

    //    using Stream stream = this.Storage.GetStream(fileName, FileAccess.Read, FileMode.Open);
    //    return await JsonSerializer.DeserializeAsync<T>(stream);
    //}

    //public async ValueTask SetAsync<T>(String key, T? value) {
    //    if (String.IsNullOrWhiteSpace(key) || key.AsSpan().ContainsAny(this.invalidFileNameChars)) {
    //        throw new ArgumentException("Invalid key.", nameof(key));
    //    }

    //    var fileName = $"{key}.json";
    //    using Stream stream = this.Storage.GetStream(fileName, FileAccess.Write, FileMode.Create);
    //    await JsonSerializer.SerializeAsync(stream, value);

    //    await stream.FlushAsync();
    //}

    public T? Get<T>(String key) {
        if (String.IsNullOrWhiteSpace(key) || key.AsSpan().ContainsAny(this.invalidFileNameChars)) {
            throw new ArgumentException("Invalid key.", nameof(key));
        }

        var fileName = $"{key}.json";
        if (!this.Storage.Exists(fileName)) {
            Logger.Log($"File {fileName} does not exist.");
            return default;
        }

        T? result = default;
        try {
            using Stream stream = this.Storage.GetStream(fileName, FileAccess.Read, FileMode.Open);
            result = JsonSerializer.Deserialize<T>(stream);
        }
        catch (Exception e) {
            Logger.Error(e, $"Error while opening or deserializing {fileName}.");
        }
        return result;
    }

    public void Set<T>(String key, T value) {
        if (String.IsNullOrWhiteSpace(key) || key.AsSpan().ContainsAny(this.invalidFileNameChars)) {
            throw new ArgumentException("Invalid key.", nameof(key));
        }

        var fileName = $"{key}.json";
        using Stream stream = this.Storage.GetStream(fileName, FileAccess.Write, FileMode.Create);
        JsonSerializer.Serialize(stream, value);

        stream.Flush();
    }
}
