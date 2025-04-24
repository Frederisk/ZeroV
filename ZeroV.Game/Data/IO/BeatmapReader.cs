using System;
using System.IO;
using System.Collections.Generic;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Data.IO;

public static class BeatmapReader {

    /// <summary>
    /// Reads all beatmap files in the specified directory.
    /// </summary>
    /// <param name="beatmapsFolder">
    /// The path to the directory containing the beatmap files.
    /// </param>
    /// <returns>
    /// A list of <see cref="FileInfo"/> objects representing the beatmap files.
    /// </returns>
    public static List<FileInfo> GetAllMapFile(String beatmapsFolder) {
        var info = new DirectoryInfo(beatmapsFolder);
        List<FileInfo> children = [];
        // If the directory does not exist, create it.
        // then return an empty list.
        if (!info.Exists) {
            info.Create(); // FIXME: IO Exception
            return children;
        }
        // Or find all the beatmap files in the directory.
        foreach (DirectoryInfo child in info.GetDirectories()) {
            FileInfo[] xmlFiles = child.GetFiles(ZeroVPath.BEATMAPS_INFO_FILE, new EnumerationOptions {
                MatchCasing = MatchCasing.CaseSensitive,
            });
            if (xmlFiles.Length > 0 && xmlFiles[0].Exists) {
                children.Add(xmlFiles[0]);
            }
        }
        return children;
    }
}
