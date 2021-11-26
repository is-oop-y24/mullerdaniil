namespace Backups.Models
{
    public class Storage
    {
        private readonly JobObject _jobObject;
        public Storage(string name, JobObject jobObject)
        {
            _jobObject = jobObject;
            Name = name;
        }

        public string OriginalFilePath => _jobObject.FilePath;
        public string Name { get; }
    }
}