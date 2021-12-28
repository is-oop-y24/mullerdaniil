using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Models;
using Backups.Tools;

namespace Backups.Repositories
{
    public class FileBackupRepository : IBackupRepository
    {
        private readonly string _backupDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "backups");

        public Storage CreateStorage(JobObject jobObject, string restorePointName, string backupName)
        {
            CreateRestorePointDirectory(restorePointName, backupName);
            string storageFilePath =
                Path.Combine(_backupDirectoryPath, backupName, restorePointName, jobObject.Name + ".zip");
            if (File.Exists(storageFilePath))
            {
                throw new BackupException("Storage with name " + jobObject.Name + " already exists.");
            }

            using (FileStream fileStream = new FileStream(storageFilePath, FileMode.Create))
            using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
            {
                zipArchive.CreateEntryFromFile(jobObject.FilePath, jobObject.Name);
            }

            return new Storage(jobObject.Name, storageFilePath);
        }

        public Storage CreateStorage(List<JobObject> jobObjects, string restorePointName, string backupName)
        {
            CreateRestorePointDirectory(restorePointName, backupName);
            string restorePointDirectoryPath = Path.Combine(_backupDirectoryPath, backupName, restorePointName);
            string restorePointArchiveFilePath =
                Path.Combine(restorePointDirectoryPath, restorePointName + ".zip");
            using (FileStream fileStream = new FileStream(restorePointArchiveFilePath, FileMode.Create))
            using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
            {
                foreach (JobObject jobObject in jobObjects)
                {
                    zipArchive.CreateEntryFromFile(jobObject.FilePath, jobObject.Name);
                }
            }

            return new Storage(restorePointName, restorePointArchiveFilePath);
        }

        private void CreateRestorePointDirectory(string restorePointName, string backupName)
        {
            string directoryPath = Path.Combine(_backupDirectoryPath, backupName, restorePointName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}