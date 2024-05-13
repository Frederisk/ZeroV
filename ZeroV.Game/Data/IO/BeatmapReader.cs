using System;
using System.IO;
using System.Collections.Generic;

namespace ZeroV.Game.Data.IO;
public class BeatmapReader {
    public static List<FileInfo> GetAllMapFile(String beatmapsFolder) {
        var info = new DirectoryInfo(beatmapsFolder);
        if (!info.Exists) {
            return [];
        }
        List<FileInfo> children = [];
        foreach (DirectoryInfo child in info.GetDirectories()) {
            FileInfo[] xmlFiles = child.GetFiles("Information.xml");
            if (xmlFiles.Length > 0 && xmlFiles[0].Exists) {
                children.Add(xmlFiles[0]);
            }
        }
        return children;
    }
}
