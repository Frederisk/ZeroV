using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;

namespace ZeroV.Game.KeyValueStorage;

public partial class JsonKeyValueStorage : CompositeDrawable, IKeyValueStorage {
    private const String floder_path = "JsonKeyValueStorage";
    private readonly SearchValues<Char> invalidFileNameChars = SearchValues.Create(Path.GetInvalidFileNameChars());

    protected Storage Storage { get; private set; } = null!;

    [BackgroundDependencyLoader]
#pragma warning disable IDE0051
    private void load(Storage storage) {
#pragma warning restore IDE0051
        if (!storage.ExistsDirectory(floder_path)) {
            this.Storage = storage.GetStorageForDirectory(floder_path);
        }
    }

    public async ValueTask<T?> GetAsync<T>(String key) {
        if (key.AsSpan().ContainsAny(this.invalidFileNameChars)) {
            throw new ArgumentException("Invalid key.", nameof(key));
        }

        var fileName = $"{key}.json";
        if (!this.Storage.Exists(fileName)) {
            return default;
        }

        using Stream stream = this.Storage.GetStream(fileName, FileAccess.Read, FileMode.Open);
        return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(stream);
    }

    public async ValueTask SetAsync<T>(String key, T value) {
        if (key.AsSpan().ContainsAny(invalidFileNameChars)) {
            throw new ArgumentException("Invalid key.", nameof(key));
        }

        var fileName = $"{key}.json";
        using Stream stream = this.Storage.GetStream(fileName, FileAccess.Write, FileMode.Truncate);
        await System.Text.Json.JsonSerializer.SerializeAsync(stream, value);

        await stream.FlushAsync();
    }
}
