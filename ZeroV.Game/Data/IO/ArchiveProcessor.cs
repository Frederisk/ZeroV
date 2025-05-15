using System;
using System.IO;
using System.IO.Compression;

using osu.Framework.Logging;

using ZeroV.Game.Data.Schema.ZeroVMap;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Data.IO;

public static class ArchiveProcessor {

    /// <summary>
    /// Extracts the contents of a ZeroV beatmap archive file to a specified directory.
    /// </summary>
    /// <param name="archiveFilePath">The path to the ZeroV beatmap archive file.</param>
    /// <param name="storagePath">
    /// The path to the directory where the contents of the archive will be extracted.
    /// </param>
    /// <returns>
    /// <see langword="true" /> if the extraction was successful;
    /// otherwise, <see langword="false" />.
    /// </returns>
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
            ZipArchiveEntry? infoFileEntry = getInfoFileEntry(archive);
            if (infoFileEntry is null) {
                return false;
            }

            using Stream infoFileStream = infoFileEntry.Open();
            ZeroVMapXml zeroVMap = BeatmapWrapper.DeserializeZeroVMapXml(infoFileStream);
            String targetName = $"{new Guid(zeroVMap.UUID)}-{new Version(zeroVMap.GameInfo.GameVersion)}";
            DirectoryInfo targetDirectoryInfo = storageDirectoryInfo.CreateSubdirectory(targetName);

            Int32 lastSeparatorIndex = infoFileEntry.FullName.LastIndexOfAny(['/', '\\']);
            String sourceDirectoryPath = lastSeparatorIndex >= 0 ? infoFileEntry.FullName[..(lastSeparatorIndex + 1)] : "";

            extractAllFiles(archive, sourceDirectoryPath, targetDirectoryInfo.FullName);
            return true;
        } catch (Exception ex) when (ex is FormatException or InvalidOperationException) {
            Logger.Error(ex, "An Exception was encountered while extracting. May be it's due to an invalid XML file. The extracting process terminated.");
            return false;
        } catch (Exception ex) when (ex is IOException or InvalidDataException) {
            Logger.Error(ex, "An Exception was encountered while extracting. May be it's due to an inability to handle the zip archive. The extracting process terminated.");
            return false;
        } catch (Exception ex) {
            Logger.Error(ex, "An Unexpected exception was encountered while extracting.");
            throw;
        }
    }

    private static ZipArchiveEntry? getInfoFileEntry(ZipArchive archive) {
        foreach (ZipArchiveEntry entry in archive.Entries) {
            if (ZeroVPath.BEATMAPS_INFO_FILE.Equals(entry.Name, StringComparison.Ordinal)) {
                return entry;
            }
        }
        return null;
    }

    private static void extractAllFiles(ZipArchive archive, String sourceDirectoryPath, String targetDirectoryPath) {
        foreach (ZipArchiveEntry entry in archive.Entries) {
            // Only entries within the target location are extracted.
            if (entry.FullName.StartsWith(sourceDirectoryPath, StringComparison.Ordinal)
                && entry.FullName.Length > sourceDirectoryPath.Length) {
                String sourceEntryRelativePath = entry.FullName[sourceDirectoryPath.Length..]
                    .Replace('/', Path.DirectorySeparatorChar)
                    .Replace('\\', Path.DirectorySeparatorChar);
                String targetDestinationPath = Path.GetFullPath(Path.Combine(targetDirectoryPath, sourceEntryRelativePath));

                // Skip all entries where the extraction destination is not in the target path.
                if (!targetDestinationPath.StartsWith(Path.GetFullPath(targetDirectoryPath) + Path.DirectorySeparatorChar, StringComparison.Ordinal)) {
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
                } catch (Exception ex) when (ex is IOException or InvalidDataException) {
                    Logger.Error(ex, $"This entry `{entry.FullName}` is abnormal and will be skipped.");
                }
            }
        }
    }
}
