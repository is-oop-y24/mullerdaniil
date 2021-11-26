using System.Collections.Generic;
using System.IO;
using Backups.Models;
using Backups.Repositories;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        private IBackupRepository _backupRepository;
        
        [SetUp]
        public void SetUp()
        {
            _backupRepository = new DummyRepository();
        }
        [Test]
        public void AddFilesToJob_FilesAreInJob()
        {
            string name1 = "fileA.txt";
            string name2 = "fileB.txt";
            BackupJob backupJob = new BackupJob("MyJob1", _backupRepository);
            backupJob.AddJobObject(new JobObject(name1, Path.Combine(Directory.GetCurrentDirectory(), name1)));
            backupJob.AddJobObject(new JobObject(name2, Path.Combine(Directory.GetCurrentDirectory(), name2)));
            Assert.AreEqual(2, backupJob.JobObjects.Count);
            Assert.AreEqual(name1, backupJob.JobObjects[0].Name);
            Assert.AreEqual(name2, backupJob.JobObjects[1].Name);
        }

        [Test]
        public void CreateRestorePointWithFiles_FilesAreInRestorePointAsStorages()
        {
            string name1 = "fileA.txt";
            string name2 = "fileB.txt";
            BackupJob backupJob = new BackupJob("MyJob2", _backupRepository);
            backupJob.AddJobObject(new JobObject(name1, Path.Combine(Directory.GetCurrentDirectory(), name1)));
            backupJob.AddJobObject(new JobObject(name2, Path.Combine(Directory.GetCurrentDirectory(), name2)));
            RestorePoint restorePoint = backupJob.Run("TwoFilesAB");
            Assert.AreEqual(1, backupJob.RestorePoints.Count);
            Assert.AreEqual(2, restorePoint.Storages.Count);
        }

        [Test]
        public void RemoveObjectFromJob_JobObjectIsRemoved()
        {
            string name1 = "fileX.txt";
            string name2 = "fileY.txt";
            JobObject jobObject = new JobObject(name2, Path.Combine(Directory.GetCurrentDirectory(), name2));
            BackupJob backupJob = new BackupJob("MyJob3", _backupRepository);
            backupJob.AddJobObject(jobObject);
            backupJob.AddJobObject(new JobObject(name1, Path.Combine(Directory.GetCurrentDirectory(), name1)));
            backupJob.RemoveJobObject(jobObject);
            List<JobObject> jobObjects = backupJob.JobObjects;
            Assert.IsFalse(jobObjects.Contains(jobObject));
        }
    }
}