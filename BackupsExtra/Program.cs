using System.IO;
using System.Linq;
using BackupsExtra.Loggers;
using BackupsExtra.Models;
using BackupsExtra.Models.DeleteAlgorithms;
using BackupsExtra.Models.RestorePointsFilters;
using BackupsExtra.Repositories;
using BackupsExtra.Services;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            IBackupRepository backupRepository = new FileBackupRepository();
            string fileName1 = "fileA.txt";
            string fileName2 = "fileB.txt";
            BackupJob backupJob = new BackupJob(
                "backupJob1",
                backupRepository,
                new DirectDeleteAlgorithm(backupRepository),
                new NumberRestorePointsFilter(2),
                new ConsoleLogger());
            JobObject obj1 = new JobObject("obj1", Path.Combine(Directory.GetCurrentDirectory(), fileName1));
            JobObject obj2 = new JobObject("obj2", Path.Combine(Directory.GetCurrentDirectory(), fileName2));
            backupJob.AddJobObject(obj1);
            backupJob.AddJobObject(obj2);
            RestorePoint restorePoint = backupJob.Run("RestPnt1");
            backupJob.RemoveJobObject(obj2);
            RestorePoint restorePoint2 = backupJob.Run("RestPnt2");
            backupJob.AddJobObject(obj2);
            RestorePoint restorePoint3 = backupJob.Run("RestPnt3");

            backupJob.CleanRestorePoints();
            PointRestorer pointRestorer = new PointRestorer();
            pointRestorer.RestoreToDifferentLocation(
                backupJob.Backup.RestorePoints.Last(),
                Path.Combine(Directory.GetCurrentDirectory(), "dest"));
            BackupJobLoader backupJobLoader = new BackupJobLoader();
            backupJobLoader.Save(backupJob, "job1");
        }
    }
}
