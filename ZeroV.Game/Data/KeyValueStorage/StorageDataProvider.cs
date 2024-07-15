using System;
using System.Threading.Tasks;

namespace ZeroV.Game.Data.KeyValueStorage;
public abstract class StorageDataProvider<T>(IKeyValueStorage keyValueStorage) {
    protected IKeyValueStorage KeyValueStorage { get; } = keyValueStorage;

    protected abstract String StorageKey { get; }

    public virtual async Task SetAsync(T value) =>
        await this.KeyValueStorage.SetAsync<T>(this.StorageKey, value);

    public virtual async Task<T?> GetAsync() =>
        await this.KeyValueStorage.GetAsync<T>(this.StorageKey);

}
