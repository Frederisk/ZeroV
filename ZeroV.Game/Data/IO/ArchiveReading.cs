using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Logging;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Data.IO;

public static class ArchiveReading {

    public static Boolean ExtractZeroVFile(String archiveFilePath, String storagePath) {
        // Prepare the archive object and the target directory.
        FileInfo archiveFileInfo = new FileInfo(archiveFilePath);
        DirectoryInfo storageDirectoryInfo = new DirectoryInfo(storagePath);
        if (!archiveFileInfo.Exists) {
            return false;
        }
        if (!storageDirectoryInfo.Exists) {
            storageDirectoryInfo.Create();
        }
        using FileStream stream = archiveFileInfo.OpenRead();
        using ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read);

        // Find the location of the entries that need to be extracted.
        ZipArchiveEntry? infoFileEntry = null;
        foreach (ZipArchiveEntry entry in archive.Entries) {
            if (String.CompareOrdinal(entry.Name, ZeroVPath.BEATMAPS_INFO_FILE) is 0) {
                infoFileEntry = entry;
                break;
            }
        }
        if (infoFileEntry is null) {
            return false;
        }
        Int32 lastSeparatorIndex = infoFileEntry.FullName.LastIndexOfAny(['/', '\\']);
        String sourceDirectoryPath = lastSeparatorIndex >= 0 ? infoFileEntry.FullName[..(lastSeparatorIndex + 1)] : "";

        // TODO: Already exists.
        DirectoryInfo targetDirectoryInfo = storageDirectoryInfo.CreateSubdirectory(Path.GetFileNameWithoutExtension(archiveFileInfo.Name));

        foreach (ZipArchiveEntry entry in archive.Entries) {
            // Only entries within the target location are extracted.
            if (entry.FullName.StartsWith(sourceDirectoryPath, StringComparison.Ordinal)
                && entry.FullName.Length > sourceDirectoryPath.Length) {
                String sourceEntryRelativePath = entry.FullName[sourceDirectoryPath.Length..]
                    .Replace('/', Path.DirectorySeparatorChar)
                    .Replace('\\', Path.DirectorySeparatorChar);
                String targetDestinationPath = Path.GetFullPath(Path.Combine(targetDirectoryInfo.FullName, sourceEntryRelativePath));

                // Skip all entries where the extraction destination is not in the target path.
                if(!targetDestinationPath.StartsWith(Path.GetFullPath(targetDirectoryInfo.FullName) + Path.DirectorySeparatorChar, StringComparison.Ordinal)) {
                    Logger.Log($"Skip unsafe archive entry: {entry.FullName}");
                    continue;
                }

                // If it's a directory, create it.
                if (entry.Name is "") {
                    Directory.CreateDirectory(targetDestinationPath);
                // Or it's a file, extract it.
                } else {
                    FileInfo entryTargetFile = new(targetDestinationPath);
                    entryTargetFile.Directory?.Create();
                    entry.ExtractToFile(entryTargetFile.FullName, true);
                }
            }
        }
        return true;
    }
}
