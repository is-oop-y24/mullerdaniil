using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Tools;
using Newtonsoft.Json;

namespace BackupsExtra.Models.RestorePointsFilters
{
    public class DateRestorePointsFilter : IRestorePointsFilter
    {
        public DateRestorePointsFilter(DateTime dateLimit)
        {
            DateLimit = dateLimit;
        }

        [JsonProperty]
        public DateTime DateLimit { get; }
        public List<RestorePoint> Filter(List<RestorePoint> restorePoints)
        {
            var result = restorePoints.Where(point => point.CreationTime >= DateLimit).ToList();
            if (result.Count == 0)
            {
                throw new BackupsExtraException("Number of restore points meeting the limit is 0");
            }

            return result;
        }
    }
}