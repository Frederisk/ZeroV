using System;

namespace ZeroV.Game.Data.KeyValueStorage;

public interface IKeyValueStorage {
    //ValueTask<T?> GetAsync<T>(String key);
    //ValueTask SetAsync<T>(String key, T value);

    T? Get<T>(String key);
    void Set<T>(String key, T value);
}
