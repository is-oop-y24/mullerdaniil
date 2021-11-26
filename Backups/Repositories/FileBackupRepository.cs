using System.IO;
using System.IO.Compression;
using Backups.Enums;
using Backups.Models;
using Backups.Tools;

namespace Backups.Repositories
{
    public class FileBackupRepository : IBackupRepository
    {
        private readonly string _backupDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "backups");

        public FileBackupRepository()
        {
            JobStoreAlgorithm = StoreAlgorithm.SingleStorage;
        }

        public FileBackupRepository(string backupDirectoryPath)
            : this()
        {
            _backupDirectoryPath = backupDirectoryPath;
        }

        public StoreAlgorithm JobStoreAlgorithm { get; set; }
        public void SaveBackupJob(BackupJob backupJob)
        {
            string backupJobDirectoryPath = Path.Combine(_backupDirectoryPath, backupJob.Name);
            if (Directory.Exists(backupJobDirectoryPath))
            {
                throw new BackupException("Backup job with name " + backupJob.Name + " already exists.");
            }

            Directory.CreateDirectory(backupJobDirectoryPath);
        }

        public void SaveRestorePoint(RestorePoint restorePoint, BackupJob backupJob)
        {
            string restorePointDirectoryPath = Path.Combine(_backupDirectoryPath, backupJob.Name, restorePoint.Name);
            if (Directory.Exists(restorePointDirectoryPath))
            {
                throw new BackupException("Restore point with name + " + restorePoint.Name + " already exists.");
            }

            Directory.CreateDirectory(restorePointDirectoryPath);

            if (JobStoreAlgorithm == StoreAlgorithm.SplitStorages)
            {
                foreach (Storage storage in restorePoint.Storages)
                {
                    SaveStorage(storage, restorePoint, backupJob);
                }
            }
            else if (JobStoreAlgorithm == StoreAlgorithm.SingleStorage)
            {
                string restorePointArchiveFilePath =
                    Path.Combine(restorePointDirectoryPath, restorePoint.Name + ".zip");
                using (FileStream fileStream = new FileStream(restorePointArchiveFilePath, FileMode.Create))
                using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                {
                    foreach (Storage storage in restorePoint.Storages)
                    {
                        zipArchive.CreateEntryFromFile(storage.OriginalFilePath, storage.Name);
                    }
                }
            }
        }

        public void SetStoreAlgorithm(StoreAlgorithm storeAlgorithm)
        {
            JobStoreAlgorithm = storeAlgorithm;
        }

        private void SaveStorage(Storage storage, RestorePoint restorePoint, BackupJob backupJob)
        {
            string storageFilePath =
                Path.Combine(_backupDirectoryPath, backupJob.Name, restorePoint.Name, storage.Name + ".zip");
            if (File.Exists(storageFilePath))
            {
                throw new BackupException("Storage with name " + storage.Name + " already exists.");
            }

            // File.Copy(storage.OriginalFilePath, storageFilePath);
            using (FileStream fileStream = new FileStream(storageFilePath, FileMode.Create))
            using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
            {
                zipArchive.CreateEntryFromFile(storage.OriginalFilePath, storage.Name);
            }
        }

        // private void RemoveStorage(Storage storage, RestorePoint restorePoint, BackupJob backupJob)
        // {
        //     string storageFilePath =
        //         Path.Combine(_backupDirectoryPath, backupJob.Name, restorePoint.Name, storage.Name);
        //     if (!File.Exists(storageFilePath))
        //     {
        //         throw new BackupException("Storage with name " + storage.Name + " does not exist.");
        //     }
        //
        //     File.Delete(storageFilePath);
        // }
    }
}