using System.Collections.Generic;
using BackupsExtra.Models;

namespace BackupsExtra.Repositories
{
    public class DummyBackupRepository : IBackupRepository
    {
        public Storage CreateStorage(JobObject jobObject, string restorePointName, string backupName)
        {
            return new Storage(jobObject.Name, string.Empty, new List<JobObject> { jobObject });
        }

        public Storage CreateStorage(List<JobObject> jobObjects, string restorePointName, string backupName)
        {
            return new Storage(restorePointName, string.Empty, jobObjects);
        }

        public void DeleteStorage(Storage storage)
        {
            // no operation.
        }

        public void MoveStorage(Storage storage, RestorePoint restorePointBefore, RestorePoint restorePointAfter, Backup backup)
        {
            // no operation.
        }

        public void CopyStorage(Storage storage, RestorePoint restorePointBefore, RestorePoint restorePointAfter, Backup backup)
        {
            // no operation.
        }

        public void DeleteRestorePoint(RestorePoint restorePoint, Backup backup)
        {
            // no operation.
        }
    }
}