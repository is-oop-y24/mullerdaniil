using System.Collections.Generic;
using BackupsExtra.Repositories;
using Newtonsoft.Json;

namespace BackupsExtra.Models.DeleteAlgorithms
{
    public class MergeDeleteAlgorithm : IDeleteAlgorithm
    {
        [JsonProperty]
        private readonly IBackupRepository _backupRepository;

        public MergeDeleteAlgorithm(IBackupRepository backupRepository)
        {
            _backupRepository = backupRepository;
        }

        public void DeleteRestorePoints(List<RestorePoint> allRestorePoints, List<RestorePoint> filteredRestorePoints, Backup backup)
        {
            foreach (var restorePoint in allRestorePoints)
            {
                if (!filteredRestorePoints.Contains(restorePoint))
                {
                    foreach (var storage in restorePoint.Storages)
                    {
                        foreach (var filteredPoint in filteredRestorePoints)
                        {
                            if (filteredPoint.Storages.Contains(storage))
                            {
                                _backupRepository.DeleteStorage(storage);
                            }
                            else
                            {
                                filteredPoint.AddStorage(storage);
                                _backupRepository.CopyStorage(storage, restorePoint, filteredPoint, backup);
                            }
                        }
                    }
                }
            }

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