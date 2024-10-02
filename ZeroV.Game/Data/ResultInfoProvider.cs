using System;
using System.Collections.Generic;

using ZeroV.Game.Data.KeyValueStorage;
using ZeroV.Game.Objects;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Data;

public class ResultInfoProvider(IKeyValueStorage keyValueStorage) : StorageDataProvider<List<ResultInfo>>(keyValueStorage) {
    protected override String StorageKey => ZeroVPath.RESULT_INFO_JSON_FILE;
}
