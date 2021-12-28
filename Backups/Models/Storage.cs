namespace Backups.Models
{
    public class Storage
    {
        public Storage(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }

        public string Name { get; }
        public string FilePath { get; }
    }
}