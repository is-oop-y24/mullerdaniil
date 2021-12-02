using System.Collections.Generic;
using Backups.Models;

namespace Backups.Repositories
{
    public class DummyRepository : IBackupRepository
    {
        public Storage CreateStorage(JobObject jobObject, string restorePointName, string backupName)
        {
            // no storage saving.
            return new Storage(jobObject.Name, string.Empty);
        }

        public Storage CreateStorage(List<JobObject> jobObjects, string restorePointName, string backupName)
        {
            // no storage saving.
            return new Storage(restorePointName, string.Empty);
        }
    }
}