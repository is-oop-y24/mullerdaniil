using Backups.Enums;
using Backups.Models;

namespace Backups.Repositories
{
    public class DummyRepository : IBackupRepository
    {
        public void SaveBackupJob(BackupJob backupJob)
        {
            // no operation.
        }

        public void SaveRestorePoint(RestorePoint restorePoint, BackupJob backupJob)
        {
            // no operation.
        }

        public void SetStoreAlgorithm(StoreAlgorithm storeAlgorithm)
        {
            // no operation.
        }
    }
}