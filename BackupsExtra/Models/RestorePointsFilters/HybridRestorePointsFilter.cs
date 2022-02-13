using System;
using System.Collections.Generic;
using BackupsExtra.Tools;
using Newtonsoft.Json;

namespace BackupsExtra.Models.RestorePointsFilters
{
    public class HybridRestorePointsFilter : IRestorePointsFilter
    {
        public HybridRestorePointsFilter(int numberLimit, DateTime dateLimit, bool matchingBothLimits)
        {
            NumberLimit = numberLimit;
            DateLimit = dateLimit;
            MatchingBothLimits = matchingBothLimits;
        }

        [JsonProperty]
        public int NumberLimit { get; }
        [JsonProperty]
        public DateTime DateLimit { get; }
        [JsonProperty]
        public bool MatchingBothLimits { get; }
        public List<RestorePoint> Filter(List<RestorePoint> restorePoints)
        {
            List<RestorePoint> result = new List<RestorePoint>();
            for (int i = 0; i < restorePoints.Count; i++)
            {
                if (MatchingBothLimits)
                {
                    if (i >= restorePoints.Count - NumberLimit && restorePoints[i].CreationTime >= DateLimit)
                    {
                        result.Add(restorePoints[i]);
                    }
                }
                else
                {
                    if (i >= restorePoints.Count - NumberLimit || restorePoints[i].CreationTime >= DateLimit)
                    {
                        result.Add(restorePoints[i]);
                    }
                }
            }

            if (result.Count == 0)
            {
                throw new BackupsExtraException("Number of restore points meeting the limit is 0");
            }

            return result;
        }
    }
}