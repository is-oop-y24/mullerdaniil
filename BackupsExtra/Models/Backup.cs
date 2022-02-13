using System.Collections.Generic;
using Newtonsoft.Json;

namespace BackupsExtra.Models
{
    public class Backup
    {
        public Backup(string name)
        {
            RestorePoints = new List<RestorePoint>();
            Name = name;
        }

        [JsonProperty]
        public List<RestorePoint> RestorePoints { get; private set; }
        [JsonProperty]
        public string Name { get; }

        public void AddRestorePoint(RestorePoint restorePoint)
        {
            RestorePoints.Add(restorePoint);
        }

        public void SetRestorePoints(List<RestorePoint> restorePoints)
        {
            RestorePoints = restorePoints;
        }
    }
}