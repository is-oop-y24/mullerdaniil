using System;
using System.Collections.Generic;
using BackupsExtra.Loggers;
using BackupsExtra.Models.DeleteAlgorithms;
using BackupsExtra.Models.RestorePointsFilters;
using BackupsExtra.Repositories;
using BackupsExtra.StoreAlgorithms;
using Newtonsoft.Json;

namespace BackupsExtra.Models
{
    public class BackupJob
    {
        public BackupJob(
            string name,
            IBackupRepository backupRepository,
            IDeleteAlgorithm deleteAlgorithm,
            IRestorePointsFilter restorePointsFilter,
            ILogger logger)
        {
            Name = name;
            BackupRepository = backupRepository;
            JobObjects = new List<JobObject>();
            Backup = new Backup(Name);
            DeleteAlgorithm = deleteAlgorithm;
            RestorePointsFilter = restorePointsFilter;
            Logger = logger;
            Logger.Log("Backup job " + Name + " has been initialized.");
        }

        [JsonConstructor]
        private BackupJob(
            string name,
            IBackupRepository backupRepository,
            IDeleteAlgorithm deleteAlgorithm,
            IRestorePointsFilter restorePointsFilter,
            ILogger logger,
            List<JobObject> jobObjects,
            Backup backup)
        {
            Name = name;
            BackupRepository = backupRepository;
            DeleteAlgorithm = deleteAlgorithm;
            RestorePointsFilter = restorePointsFilter;
            Logger = logger;
            JobObjects = jobObjects;
            Backup = backup;
            Logger.Log("Backup job " + Name + " has been loaded.");
        }

        [JsonProperty]
        public string Name { get; }
        [JsonProperty]
        public IBackupRepository BackupRepository { get; }
        [JsonProperty]
        public List<JobObject> JobObjects { get; }
        [JsonProperty]
        public IStoreAlgorithm StoreAlgorithm { get; set; } = new SplitStoragesAlgorithm();
        [JsonProperty]
        public Backup Backup { get; }
        [JsonProperty]
        public IDeleteAlgorithm DeleteAlgorithm { get; set; }
        [JsonProperty]
        public IRestorePointsFilter RestorePointsFilter { get; set; }
        [JsonProperty]
        public ILogger Logger { get; set; }

        public void AddJobObject(JobObject jobObject)
        {
            JobObjects.Add(jobObject);
            Logger.Log("Job object " + jobObject.Name + " has been added.");
        }

        public void RemoveJobObject(JobObject jobObject)
        {
            JobObjects.Remove(jobObject);
            Logger.Log("Job object " + jobObject.Name + " has been removed.");
        }

        public RestorePoint Run(string restorePointName)
        {
            List<Storage> storages = StoreAlgorithm.MakeStorages(JobObjects, restorePointName, Backup.Name, BackupRepository);
            RestorePoint restorePoint = new RestorePoint(restorePointName, storages, DateTime.Now);
            Backup.AddRestorePoint(restorePoint);
            Logger.Log("Restore point " + restorePointName + " has been created.");
            return restorePoint;
        }

        public void CleanRestorePoints()
        {
            List<RestorePoint> filteredRestorePoints = RestorePointsFilter.Filter(Backup.RestorePoints);
            DeleteAlgorithm.DeleteRestorePoints(Backup.RestorePoints, filteredRestorePoints, Backup);
            Logger.Log("Restore points have been cleaned.");
        }
    }
}