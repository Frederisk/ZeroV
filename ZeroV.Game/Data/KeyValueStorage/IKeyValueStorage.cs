using System;
using System.Threading.Tasks;

namespace ZeroV.Game.Data.KeyValueStorage;

public interface IKeyValueStorage {
    ValueTask<T?> GetAsync<T>(String key);
    ValueTask SetAsync<T>(String key, T value);
}
