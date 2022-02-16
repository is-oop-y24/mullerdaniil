using System;
using Reports.Server.Domain.Entities.Task;

namespace Reports.Server.DTOs
{
    public class TaskDTO
    {
        public TaskDTO(Task task)
        {
            Id = task.Id;

            if (task.Employee is null)
            {
                EmployeeId = default;
            }
            else
            {
                EmployeeId = task.Employee.Id;
            }
            Description = task.Description;
            CreationTime = task.CreationTime;
            LastModificationTime = task.LastModificationTime;
            State = task.State.ToString();
        }
        
        public Guid Id { get; }
        public Guid EmployeeId { get; }
        public string Description { get; }
        public DateTime CreationTime { get; }
        public DateTime LastModificationTime { get; }
        public string State { get; }
    }
}