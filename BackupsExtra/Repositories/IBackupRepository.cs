using System.Collections.Generic;
using BackupsExtra.Models;

namespace BackupsExtra.Repositories
{
    public interface IBackupRepository
    {
        Storage CreateStorage(JobObject jobObject, string restorePointName, string backupName);
        Storage CreateStorage(List<JobObject> jobObjects, string restorePointName, string backupName);
        void DeleteStorage(Storage storage);
        void MoveStorage(Storage storage, RestorePoint restorePointBefore, RestorePoint restorePointAfter, Backup backup);

        void CopyStorage(Storage storage, RestorePoint restorePointBefore, RestorePoint restorePointAfter, Backup backup);
        void DeleteRestorePoint(RestorePoint restorePoint, Backup backup);
    }
}