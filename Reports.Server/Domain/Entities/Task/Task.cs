using System;
using System.Collections.Generic;
using Reports.Server.Domain.Entities.Task.TaskUpdate;
using Reports.Server.Tools;

namespace Reports.Server.Domain.Entities.Task
{
    public class Task
    {
        private readonly List<TaskComment> _comments;
        private readonly List<TaskUpdate.TaskUpdate> _updateHistory;

        public Task()
        {
        }

        public Task(string description, DateTime creationTime, Employee.Employee employee)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ReportsException("Description must not be null, empty or whitespace only.");
            }
            
            Id = Guid.NewGuid();
            Description = description;
            CreationTime = creationTime;
            LastModificationTime = creationTime;
            State = TaskState.Open;
            _comments = new List<TaskComment>();
            _updateHistory = new List<TaskUpdate.TaskUpdate>();
            Employee = employee;
            
            _updateHistory.Add(new TaskUpdate.TaskUpdate("The task has been created.",
                TaskUpdateType.Created, creationTime, employee.Id));
        }
        public Guid Id { get; }
        public Employee.Employee Employee { get; private set; }
        public string Description { get; private set; }
        public DateTime CreationTime { get; }
        public DateTime LastModificationTime { get; private set; }
        public TaskState State { get; private set; }
        public IReadOnlyList<TaskComment> Comments => _comments;
        public IReadOnlyList<TaskUpdate.TaskUpdate> UpdateHistory => _updateHistory;

        public void UpdateState(DateTime modificationTime)
        {
            switch (State)
            {
                case TaskState.Open:
                    State = TaskState.Active;
                    break;
                case TaskState.Active:
                    State = TaskState.Resolved;
                    break;
                case TaskState.Resolved:
                    State = TaskState.Open;
                    break;
            }

            LastModificationTime = modificationTime;
            _updateHistory.Add(new TaskUpdate.TaskUpdate(State.ToString(), TaskUpdateType.StateUpdated, modificationTime, Employee.Id));
        }

        public void SetEmployee(Employee.Employee employee)
        {
            Employee = employee ?? throw new ReportsException("Employee must not be null.");
        }

        public void AddComment(TaskComment comment, DateTime creationTime)
        {
            if (comment is null)
            {
                throw new ReportsException("Comment must not be null.");
            }
            
            _comments.Add(comment);
            LastModificationTime = creationTime;
            _updateHistory.Add(new TaskUpdate.TaskUpdate(comment.Text, TaskUpdateType.CommentAdded, creationTime, Employee.Id));
        }

        public void SetDescription(string description, DateTime modificationTime)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ReportsException("Description must not be null, empty or whitespace only.");
            }

            Description = description;
            LastModificationTime = modificationTime;
            _updateHistory.Add(new TaskUpdate.TaskUpdate(description, TaskUpdateType.DescriptionUpdated, modificationTime, Employee.Id));
        }
    }
}