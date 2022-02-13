using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Tools;
using Newtonsoft.Json;

namespace BackupsExtra.Models.RestorePointsFilters
{
    public class NumberRestorePointsFilter : IRestorePointsFilter
    {
        public NumberRestorePointsFilter(int numberLimit)
        {
            if (numberLimit > 0)
            {
                NumberLimit = numberLimit;
            }
            else
            {
                throw new BackupsExtraException("Number limit must be a positive number");
            }
        }

        [JsonProperty]
        public int NumberLimit { get; }
        public List<RestorePoint> Filter(List<RestorePoint> restorePoints)
        {
            var result = restorePoints.TakeLast(NumberLimit).ToList();
            if (result.Count == 0)
            {
                throw new BackupsExtraException("Number of restore points meeting the limit is 0");
            }

            return result;
        }
    }
}