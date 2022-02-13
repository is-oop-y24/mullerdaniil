using System.Collections.Generic;
using Newtonsoft.Json;

namespace BackupsExtra.Models
{
    public class Storage
    {
        internal Storage(string name, string filePath, List<JobObject> jobObjects)
        {
            Name = name;
            FilePath = filePath;
            JobObjects = jobObjects;
        }

        [JsonProperty]
        public string Name { get; }
        [JsonProperty]
        public string FilePath { get; }
        [JsonProperty]
        public List<JobObject> JobObjects { get; }
    }
}