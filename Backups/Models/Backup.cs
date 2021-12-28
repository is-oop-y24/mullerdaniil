using System.Collections.Generic;

namespace Backups.Models
{
    public class Backup
    {
        public Backup(string name)
        {
            Name = name;
        }

        public List<RestorePoint> RestorePoints { get; } = new ();
        public string Name { get; }

        public void AddRestorePoint(RestorePoint restorePoint)
        {
            RestorePoints.Add(restorePoint);
        }
    }
}