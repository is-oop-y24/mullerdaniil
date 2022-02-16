using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Server.Tools;

namespace Reports.Server.Domain.Entities.Report
{
    public class Report
    {
        private readonly List<Task.Task> _tasks;

        public Report()
        {
        }

        public Report(Guid authorId, DateTime creationTime, string description)
        {
            Id = Guid.NewGuid();
            State = ReportState.Open;
            AuthorId = authorId;
            CreationTime = creationTime;
            Description = description;
            _tasks = new List<Task.Task>();
        }

        public Guid Id { get; }
        public ReportState State { get; private set; }
        public Guid AuthorId { get; }
        public DateTime CreationTime { get; }
        public DateTime FinishTime { get; private set; }
        public string Description { get; private set; }
        public IReadOnlyList<Task.Task> Tasks => _tasks;

        public void Finish(DateTime finishTime)
        {
            FinishTime = finishTime;
            State = ReportState.Finished;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetState(ReportState state)
        {
            State = state;
            if (state is ReportState.Open)
            {
                FinishTime = default;
            }
        }
        
        public void AddTask(Task.Task task)
        {
            if (task is null)
            {
                throw new ReportsException("Task must not be null.");
            }

            if (_tasks.Any(t => t.Id == task.Id))
            {
                throw new ReportsException("Employee has the task already.");
            }
            
            _tasks.Add(task);
        }

        public void RemoveTask(Task.Task task)
        {
            if (task is null)
            {
                throw new ReportsException("Task must not be null.");
            }

            if (_tasks.Any(t => t.Id == task.Id))
            {
                throw new ReportsException("Employee does not have the task.");
            }

            _tasks.Remove(task);
        }
    }
}