using System.IO;
using Backups.Enums;
using Backups.Models;
using Backups.Repositories;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            string fileName1 = "fileA.txt";
            string fileName2 = "fileB.txt";
            IBackupRepository backupRepository = new FileBackupRepository("backups");
            BackupJob backupJob = new BackupJob("job3", backupRepository);
            JobObject jobObject1 = new JobObject(fileName1, Path.Combine(Directory.GetCurrentDirectory(), fileName1));
            JobObject jobObject2 = new JobObject(fileName2, Path.Combine(Directory.GetCurrentDirectory(), fileName2));
            backupJob.AddJobObject(jobObject1);
            backupJob.AddJobObject(jobObject2);
            RestorePoint restorePoint = backupJob.Run("RestPnt1");
            backupJob.RemoveJobObject(jobObject1);
            backupRepository.SetStoreAlgorithm(StoreAlgorithm.SplitStorages);
            RestorePoint restorePoint2 = backupJob.Run("RestPnt2");
        }
    }
}
