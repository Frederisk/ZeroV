using System;
using System.IO;

using osu.Framework.Logging;

namespace ZeroV.Game.Data.IO;
public static class FileManager {
    public static Boolean MoveFolder(DirectoryInfo sourceInfo, DirectoryInfo destinationInfo, Boolean overwriteFiles = true) {
        try {
            if (!sourceInfo.Exists) {
                return false;
            }
            if (!destinationInfo.Exists) {
                destinationInfo.Create();
            }

            foreach (FileInfo file in sourceInfo.GetFiles()) {
                file.CopyTo(Path.Combine(destinationInfo.FullName, file.Name), overwriteFiles);
            }
            foreach (DirectoryInfo directory in sourceInfo.GetDirectories()) {
                DirectoryInfo nextInfo = destinationInfo.CreateSubdirectory(directory.Name);
                MoveFolder(directory, nextInfo, overwriteFiles);
            }
            return true;
        } catch (Exception ex) when (ex is IOException) {
            Logger.Error(ex, "An IO exception was encountered while moving folders.");
            return false;
        } catch (Exception ex) {
            Logger.Error(ex, "An Unexpected exception was encountered while extracting.");
            throw;
        }
    }

    public static Boolean DeleteFolder(DirectoryInfo directoryInfo) {
        try {
            if (directoryInfo.Exists) {
                directoryInfo.Delete(true);
            }
            return true;
        } catch (Exception ex) when (ex is IOException) {
            Logger.Error(ex, "An IO exception was encountered while deleting a folder.");
            return false;
        } catch (Exception ex) {
            Logger.Error(ex, "An Unexpected exception was encountered while deleting");
            throw;
        }
    }
}
