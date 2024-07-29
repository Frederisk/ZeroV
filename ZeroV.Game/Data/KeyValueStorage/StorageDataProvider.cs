using System;
using System.Threading.Tasks;

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
public abstract class StorageDataProvider<T>(IKeyValueStorage keyValueStorage) {
    protected IKeyValueStorage KeyValueStorage { get; } = keyValueStorage;

    /// <summary>
    /// The key of the data in the storage.
    /// </summary>
    protected abstract String StorageKey { get; }

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

    public virtual T? Get() =>
        this.KeyValueStorage.Get<T>(this.StorageKey);

    public virtual void Set(T value) =>
        this.KeyValueStorage.Set<T>(this.StorageKey, value);
}
