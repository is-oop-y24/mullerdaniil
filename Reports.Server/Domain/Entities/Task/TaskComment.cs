using System;
using Reports.Server.Tools;

namespace Reports.Server.Domain.Entities.Task
{
    public class TaskComment
    {
        public TaskComment()
        {
        }
        public TaskComment(string text, Employee.Employee author, DateTime creationTime)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ReportsException("Task comment's text must not be null, empty or whitespace only.");
            }
            Id = Guid.NewGuid();
            CreationTime = creationTime;
            Text = text;
            Author = author ?? throw new ReportsException("Task comment's author must not be null");
        }
        
        public Guid Id { get; }
        public DateTime CreationTime { get; }
        public string Text { get; }
        public Employee.Employee Author { get; }
    }
}