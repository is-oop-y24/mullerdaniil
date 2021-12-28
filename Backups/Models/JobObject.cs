using System.IO;

namespace Backups.Models
{
    public class JobObject
    {
        public JobObject(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }

        public string Name { get; }
        public string FilePath { get; }
    }
}