using Newtonsoft.Json;

namespace BackupsExtra.Models
{
    public class JobObject
    {
        public JobObject(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }

        [JsonProperty]
        public string Name { get; }
        [JsonProperty]
        public string FilePath { get; }
    }
}