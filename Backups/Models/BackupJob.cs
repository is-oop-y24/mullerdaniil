using System.Collections.Generic;
using Backups.Models.StoreAlgorithms;
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
            JobObjects = new List<JobObject>();
            Backup = new Backup(Name);
        }

        public string Name { get; }
        public List<JobObject> JobObjects { get; }
        public IStoreAlgorithm StoreAlgorithm { get; set; } = new SplitStoragesAlgorithm();
        public Backup Backup { get; }

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
            List<Storage> storages = StoreAlgorithm.MakeStorages(JobObjects, restorePointName, Backup.Name, _backupRepository);
            RestorePoint restorePoint = new RestorePoint(restorePointName, storages);
            Backup.AddRestorePoint(restorePoint);
            return restorePoint;
        }
    }
}