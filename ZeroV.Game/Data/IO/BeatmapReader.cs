using System;
using System.IO;
using System.Collections.Generic;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Data.IO;

public static class BeatmapReader {

    public static List<FileInfo> GetAllMapFile(String beatmapsFolder) {
        var info = new DirectoryInfo(beatmapsFolder);
        List<FileInfo> children = [];
        if (!info.Exists) {
            info.Create(); // FIXME: IO Exception
            return children;
        }
        foreach (DirectoryInfo child in info.GetDirectories()) {
            FileInfo[] xmlFiles = child.GetFiles(ZeroVPath.BEATMAPS_INFO_FILE);
            if (xmlFiles.Length > 0 && xmlFiles[0].Exists) {
                children.Add(xmlFiles[0]);
            }
        }
        return children;
    }
}
