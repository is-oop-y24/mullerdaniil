using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Server.DAL.Database;
using Reports.Server.Domain.Entities.Task;

namespace Reports.Server.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ReportsDatabaseContext _context;

        public TaskRepository(ReportsDatabaseContext context)
        {
            _context = context;
        }
        
        public IReadOnlyList<Task> GetTasks()
        {
            return _context.Tasks.ToList();
        }

        public Task GetTaskById(Guid id)
        {
            return _context.Tasks.Find(id);
        }

        public void InsertTask(Task Task)
        {
            _context.Tasks.Add(Task);
            _context.SaveChanges();
        }

        public void DeleteTask(Guid id)
        {
            Task Task = _context.Tasks.Find(id);
            _context.Tasks.Remove(Task);
            _context.SaveChanges();
        }

        public void UpdateTask(Task Task)
        {
            Task found = _context.Tasks.Find(Task.Id);
            _context.Entry(found).CurrentValues.SetValues(Task);
            _context.SaveChanges();
        }
    }
}