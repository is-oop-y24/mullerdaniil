using System.Collections.Generic;
using System.Linq;
using Backups.Repositories;

namespace Backups.Models.StoreAlgorithms
{
    public class SplitStoragesAlgorithm : IStoreAlgorithm
    {
        public List<Storage> MakeStorages(List<JobObject> jobObjects, string restorePointName, string backupName, IBackupRepository repository)
        {
            return jobObjects.Select(jobObject => repository.CreateStorage(jobObject, restorePointName, backupName)).ToList();
        }
    }
}