using System.Collections.Generic;
using BackupsExtra.Repositories;
using Newtonsoft.Json;

namespace BackupsExtra.Models.DeleteAlgorithms
{
    public class DirectDeleteAlgorithm : IDeleteAlgorithm
    {
        [JsonProperty]
        private readonly IBackupRepository _backupRepository;

        public DirectDeleteAlgorithm(IBackupRepository backupRepository)
        {
            _backupRepository = backupRepository;
        }

        public void DeleteRestorePoints(List<RestorePoint> allRestorePoints, List<RestorePoint> filteredRestorePoints, Backup backup)
        {
            foreach (var restorePoint in allRestorePoints)
            {
                if (!filteredRestorePoints.Contains(restorePoint))
                {
                    _backupRepository.DeleteRestorePoint(restorePoint, backup);
                }
            }

            backup.SetRestorePoints(filteredRestorePoints);
        }
    }
}