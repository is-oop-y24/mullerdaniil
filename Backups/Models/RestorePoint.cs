using System.Collections.Generic;

namespace Backups.Models
{
    public class RestorePoint
    {
        public RestorePoint(string name, string storageSuffix, List<JobObject> jobObjects)
        {
            Name = name;
            JobObjects = jobObjects;

            // to-do : initializing storages
            Storages = new List<Storage>();
            foreach (JobObject jobObject in jobObjects)
            {
                Storages.Add(new Storage(storageSuffix + jobObject.Name, jobObject));
            }
        }

        public string Name { get; }
        public List<JobObject> JobObjects { get; }
        public List<Storage> Storages { get; }
    }
}