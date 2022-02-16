using System;
using System.Collections.Generic;
using Reports.Server.Domain.Entities.Task;

namespace Reports.Server.DAL.Repositories
{
    public interface ITaskRepository
    {
        IReadOnlyList<Task> GetTasks();
        Task GetTaskById(Guid id);
        void InsertTask(Task task);
        void DeleteTask(Guid id);
        void UpdateTask(Task task);
    }
}