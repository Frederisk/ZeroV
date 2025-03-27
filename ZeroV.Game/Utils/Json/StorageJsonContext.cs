using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Utils.Json;

[JsonSerializable(typeof(IReadOnlyList<TrackInfo>))]
[JsonSerializable(typeof(TrackInfo))]
//[JsonSerializable(typeof(Guid))]
//[JsonSerializable(typeof(String))]
//[JsonSerializable(typeof(Int32?))]
//[JsonSerializable(typeof(TimeSpan))]
//[JsonSerializable(typeof(Version))]
//[JsonSerializable(typeof(IReadOnlyList<MapInfo>))]
//[JsonSerializable(typeof(MapInfo))]
//[JsonSerializable(typeof(FileInfo))]
//[JsonSerializable(typeof(Double))]
internal partial class StorageJsonContext: JsonSerializerContext {
}
