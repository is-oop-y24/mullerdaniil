using System.Collections.Generic;
using Backups.Models;

namespace Backups.Repositories
{
    public interface IBackupRepository
    {
        Storage CreateStorage(JobObject jobObject, string restorePointName, string backupName);
        Storage CreateStorage(List<JobObject> jobObjects, string restorePointName, string backupName);
    }
}