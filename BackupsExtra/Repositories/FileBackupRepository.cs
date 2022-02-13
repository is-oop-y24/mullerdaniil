using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using BackupsExtra.Models;
using BackupsExtra.Tools;

namespace BackupsExtra.Repositories
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
                throw new BackupsExtraException("Storage with name " + jobObject.Name + " already exists.");
            }

            using (FileStream fileStream = new FileStream(storageFilePath, FileMode.Create))
            using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
            {
                zipArchive.CreateEntryFromFile(jobObject.FilePath, Path.GetFileName(jobObject.FilePath));
            }

            return new Storage(jobObject.Name, storageFilePath, new List<JobObject> { jobObject });
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
                    zipArchive.CreateEntryFromFile(jobObject.FilePath, Path.GetFileName(jobObject.FilePath));
                }
            }

            return new Storage(restorePointName, restorePointArchiveFilePath, jobObjects);
        }

        public void DeleteStorage(Storage storage)
        {
            File.Delete(storage.FilePath);
        }

        public void MoveStorage(Storage storage, RestorePoint restorePointBefore, RestorePoint restorePointAfter, Backup backup)
        {
            string restorePointDirectoryPath = Path.Combine(_backupDirectoryPath, backup.Name, restorePointAfter.Name);
            File.Move(
                storage.FilePath,
                Path.Combine(restorePointDirectoryPath, Path.GetFileName(storage.FilePath)),
                true);
        }

        public void CopyStorage(Storage storage, RestorePoint restorePointBefore, RestorePoint restorePointAfter, Backup backup)
        {
            string restorePointDirectoryPath = Path.Combine(_backupDirectoryPath, backup.Name, restorePointAfter.Name);
            File.Copy(
                storage.FilePath,
                Path.Combine(restorePointDirectoryPath, Path.GetFileName(storage.FilePath)),
                true);
        }

        public void DeleteRestorePoint(RestorePoint restorePoint, Backup backup)
        {
            string directoryPath = Path.Combine(_backupDirectoryPath, backup.Name, restorePoint.Name);
            Directory.Delete(directoryPath, true);
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