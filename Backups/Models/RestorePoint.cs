using System.Collections.Generic;

namespace Backups.Models
{
    public class RestorePoint
    {
        public RestorePoint(string name, List<Storage> storages)
        {
            Name = name;
            Storages = storages;
        }

        public string Name { get; }
        public List<Storage> Storages { get; }
    }
}