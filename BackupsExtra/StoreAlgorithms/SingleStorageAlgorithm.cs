using System.Collections.Generic;
using BackupsExtra.Models;
using BackupsExtra.Repositories;

namespace BackupsExtra.StoreAlgorithms
{
    public class SingleStorageAlgorithm : IStoreAlgorithm
    {
        public List<Storage> MakeStorages(List<JobObject> jobObjects, string restorePointName, string backupName, IBackupRepository repository)
        {
            return new List<Storage> { repository.CreateStorage(jobObjects, restorePointName, backupName) };
        }
    }
}