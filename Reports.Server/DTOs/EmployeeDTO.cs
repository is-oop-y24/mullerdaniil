using System;
using Reports.Server.Domain.Entities.Employee;

namespace Reports.Server.DTOs
{
    public class EmployeeDTO
    {
        public EmployeeDTO(Employee employee)
        {
            Id = employee.Id;
            Name = employee.Name;
            SuperiorId = employee.SuperiorId;
            State = employee.State.ToString();
            CreationTime = employee.CreationTime;
        }
        
        public Guid Id { get; }
        public string Name { get; }
        public Guid SuperiorId { get; }
        public string State { get; }
        public DateTime CreationTime { get; }
    }
}