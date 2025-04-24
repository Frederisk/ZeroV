using System;
using System.IO;
using System.IO.Compression;

using osu.Framework.Logging;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Data.IO;

public static class ArchiveReading {

    public static Boolean ExtractZeroVFile(String archiveFilePath, String storagePath) {
        try {
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
                if (ZeroVPath.BEATMAPS_INFO_FILE.Equals(entry.Name, StringComparison.Ordinal)) {
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
                    if (!targetDestinationPath.StartsWith(Path.GetFullPath(targetDirectoryInfo.FullName) + Path.DirectorySeparatorChar, StringComparison.Ordinal)) {
                        Logger.Log($"Skip unsafe archive entry: {entry.FullName}");
                        continue;
                    }

                    try {
                        // If it's a directory, create it.
                        if (entry.Name is "") {
                            Directory.CreateDirectory(targetDestinationPath);
                            // Or it's a file, extract it.
                        } else {
                            FileInfo entryTargetFile = new(targetDestinationPath);
                            entryTargetFile.Directory?.Create();
                            entry.ExtractToFile(entryTargetFile.FullName, true);
                        }
                    } catch (Exception ex) when (isNormalIOException(ex)) {
                        Logger.Error(ex, $"This entry `{entry.FullName}` is abnormal and will be skipped.");
                    }
                }
            }
            return true;
        } catch (Exception ex) when (isNormalIOException(ex)) {
            Logger.Error(ex, "An Exception was encountered while extracting. The extracting process terminated.");
            return false;
        } catch (Exception ex) {
            Logger.Error(ex, "An Unexpected exception was encountered while extracting.");
            throw;
        }
    }

    private static Boolean isNormalIOException(Exception ex) {
        return ex is FileNotFoundException // IOException
            or DirectoryNotFoundException // IOException
            or PathTooLongException // IOException
            or IOException
            or InvalidDataException;
    }
}
