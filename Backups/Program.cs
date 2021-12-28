using System.IO;
using Backups.Models;
using Backups.Models.StoreAlgorithms;
using Backups.Repositories;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            string fileName1 = "fileA.txt";
            string fileName2 = "fileB.txt";
            IBackupRepository backupRepository = new FileBackupRepository();
            BackupJob backupJob = new BackupJob("backup4", backupRepository);
            backupJob.StoreAlgorithm = new SingleStorageAlgorithm();
            JobObject jobObject1 = new JobObject(fileName1, Path.Combine(Directory.GetCurrentDirectory(), fileName1));
            JobObject jobObject2 = new JobObject(fileName2, Path.Combine(Directory.GetCurrentDirectory(), fileName2));
            backupJob.AddJobObject(jobObject1);
            backupJob.AddJobObject(jobObject2);
            RestorePoint restorePoint = backupJob.Run("RestPnt1");
            backupJob.RemoveJobObject(jobObject1);
            backupJob.StoreAlgorithm = new SplitStoragesAlgorithm();
            RestorePoint restorePoint2 = backupJob.Run("RestPnt2");
        }
    }
}
