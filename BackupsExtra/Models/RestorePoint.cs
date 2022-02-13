using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BackupsExtra.Models
{
    public class RestorePoint
    {
        internal RestorePoint(string name, List<Storage> storages, DateTime creationTime)
        {
            Name = name;
            Storages = storages;
            CreationTime = creationTime;
        }

        [JsonProperty]
        public string Name { get; }
        [JsonProperty]
        public List<Storage> Storages { get; }
        [JsonProperty]
        public DateTime CreationTime { get; }

        internal void AddStorage(Storage storage)
        {
            Storages.Add(storage);
        }

        internal void RemoveStorage(Storage storage)
        {
            Storages.Remove(storage);
        }
    }
}