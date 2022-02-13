using System.IO;
using System.IO.Compression;
using BackupsExtra.Models;

namespace BackupsExtra.Services
{
    public class PointRestorer
    {
        public void RestoreToOriginalLocation(RestorePoint restorePoint)
        {
            string bufferDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "bufferDirectory");
            if (Directory.Exists(bufferDirectoryPath))
            {
                Directory.Delete(bufferDirectoryPath);
            }

            foreach (var storage in restorePoint.Storages)
            {
                Directory.CreateDirectory(bufferDirectoryPath);
                ZipFile.ExtractToDirectory(storage.FilePath, bufferDirectoryPath, true);
                foreach (var jobObject in storage.JobObjects)
                {
                    if (File.Exists(jobObject.FilePath))
                    {
                        File.Delete(jobObject.FilePath);
                    }

                    File.Move(
                        Path.Combine(bufferDirectoryPath, Path.GetFileName(jobObject.FilePath)),
                        jobObject.FilePath,
                        true);
                }

                Directory.Delete(bufferDirectoryPath, true);
            }
        }

        public void RestoreToDifferentLocation(RestorePoint restorePoint, string directoryLocation)
        {
            foreach (var storage in restorePoint.Storages)
            {
                ZipFile.ExtractToDirectory(storage.FilePath, directoryLocation, true);
            }
        }
    }
}