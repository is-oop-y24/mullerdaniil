using System.Collections.Generic;

namespace BackupsExtra.Models.DeleteAlgorithms
{
    public interface IDeleteAlgorithm
    {
        void DeleteRestorePoints(List<RestorePoint> allRestorePoints, List<RestorePoint> filteredRestorePoints, Backup backup);
    }
}