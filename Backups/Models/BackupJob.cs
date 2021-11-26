using System.Collections.Generic;
using Backups.Repositories;

namespace Backups.Models
{
    public class BackupJob
    {
        private readonly IBackupRepository _backupRepository;
        public BackupJob(string name, IBackupRepository backupRepository)
        {
            Name = name;
            _backupRepository = backupRepository;
            RestorePoints = new List<RestorePoint>();
            JobObjects = new List<JobObject>();
            _backupRepository.SaveBackupJob(this);
        }

        public string Name { get; }
        public List<RestorePoint> RestorePoints { get; }
        public List<JobObject> JobObjects { get; }

        public void AddJobObject(JobObject jobObject)
        {
            JobObjects.Add(jobObject);
        }

        public void RemoveJobObject(JobObject jobObject)
        {
            JobObjects.Remove(jobObject);
        }

        public RestorePoint Run(string restorePointName)
        {
            RestorePoint restorePoint = new RestorePoint(restorePointName, GenerateStorageSuffix(restorePointName), new List<JobObject>(JobObjects));
            _backupRepository.SaveRestorePoint(restorePoint, this);
            RestorePoints.Add(restorePoint);
            return restorePoint;
        }

        private static string GenerateStorageSuffix(string restorePointName)
        {
            return restorePointName + "_";
        }
    }
}