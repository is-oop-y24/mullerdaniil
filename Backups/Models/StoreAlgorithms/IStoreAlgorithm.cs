using System.Collections.Generic;
using Backups.Repositories;

namespace Backups.Models.StoreAlgorithms
{
    public interface IStoreAlgorithm
    {
        List<Storage> MakeStorages(List<JobObject> jobObjects, string restorePointName, string backupName, IBackupRepository repository);
    }
}