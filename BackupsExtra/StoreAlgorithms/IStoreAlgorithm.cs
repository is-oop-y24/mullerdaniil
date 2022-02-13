using System.Collections.Generic;
using BackupsExtra.Models;
using BackupsExtra.Repositories;

namespace BackupsExtra.StoreAlgorithms
{
    public interface IStoreAlgorithm
    {
        List<Storage> MakeStorages(List<JobObject> jobObjects, string restorePointName, string backupName, IBackupRepository repository);
    }
}