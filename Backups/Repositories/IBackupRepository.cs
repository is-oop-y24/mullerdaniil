using Backups.Enums;
using Backups.Models;

namespace Backups.Repositories
{
    public interface IBackupRepository
    {
        void SaveBackupJob(BackupJob backupJob);
        void SaveRestorePoint(RestorePoint restorePoint, BackupJob backupJob);
        void SetStoreAlgorithm(StoreAlgorithm storeAlgorithm);
    }
}