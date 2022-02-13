using System.IO;
using BackupsExtra.Models;
using Newtonsoft.Json;

namespace BackupsExtra.Services
{
    public class BackupJobLoader
    {
        private static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        };

        public void Save(BackupJob backupJob, string name)
        {
            string json = JsonConvert.SerializeObject(backupJob, _serializerSettings);
            File.WriteAllText(name, json);
        }

        public BackupJob Load(string name)
        {
            string json = File.ReadAllText(name);
            return JsonConvert.DeserializeObject<BackupJob>(json, _serializerSettings);
        }
    }
}