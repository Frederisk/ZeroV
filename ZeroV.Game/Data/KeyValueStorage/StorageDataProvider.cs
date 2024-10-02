using System;

namespace ZeroV.Game.Data.KeyValueStorage;

/// <summary>
/// A data provider that stores data in a key-value storage.
/// </summary>
/// <typeparam name="T">
/// The type of data to store.
/// </typeparam>
/// <param name="keyValueStorage">
/// The key-value storage to use.
/// </param>
public abstract class StorageDataProvider<T>(IKeyValueStorage keyValueStorage) where T: class {
    protected IKeyValueStorage KeyValueStorage { get; } = keyValueStorage;

    /// <summary>
    /// The key of the data in the storage.
    /// </summary>
    protected abstract String StorageKey { get; }

    /// <summary>
    /// The cached value of the data. This is <see langword="null"/> if the data is not cached.
    /// </summary>
    protected T? CachedValue { get; set; }

    /// <summary>
    /// Whether to allow caching of the data. If the data you don't want to cache, set this to <see langword="false"/>. The default value is <see langword="true"/>.
    /// </summary>
    protected Boolean IsAllowCache { get; } = true;

    ///// <summary>
    ///// Sets the data in the storage.
    ///// </summary>
    ///// <param name="value">
    ///// The data to store.
    ///// </param>
    ///// <returns>
    ///// A <see cref="Task"/> representing the asynchronous operation.
    ///// </returns>
    //public virtual async Task SetAsync(T value) =>
    //    await this.KeyValueStorage.SetAsync<T>(this.StorageKey, value);

    ///// <summary>
    ///// Gets the data from the storage.
    ///// </summary>
    ///// <returns>
    ///// The data from the storage.
    ///// </returns>
    //public virtual async Task<T?> GetAsync() =>
    //    await this.KeyValueStorage.GetAsync<T>(this.StorageKey);

    public virtual T? Get() {
        if (this.IsAllowCache && this.CachedValue is not null) {
            return this.CachedValue;
        }
        return this.KeyValueStorage.Get<T>(this.StorageKey);
    }

    public virtual void Set(T value) {
        if (this.IsAllowCache) {
            this.CachedValue = value;
        }
        this.KeyValueStorage.Set<T>(this.StorageKey, value);
    }
}
