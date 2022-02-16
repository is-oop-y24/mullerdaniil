using System;

namespace Reports.Server.Domain.Entities.Task.TaskUpdate
{
    public class TaskUpdate
    {
        public TaskUpdate()
        {
        }
        public TaskUpdate(string description, TaskUpdateType updateType, DateTime time, Guid authorId)
        {
            Description = description;
            UpdateType = updateType;
            Time = time;
            AuthorId = authorId;
        }
        
        public string Description { get; }
        public TaskUpdateType UpdateType { get; }
        public DateTime Time { get; }
        public Guid AuthorId { get; }
    }
}