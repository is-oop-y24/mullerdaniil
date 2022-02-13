using System.Collections.Generic;

namespace BackupsExtra.Models.RestorePointsFilters
{
    public interface IRestorePointsFilter
    {
        List<RestorePoint> Filter(List<RestorePoint> restorePoints);
    }
}