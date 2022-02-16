using System;
using System.Collections.Generic;
using Reports.Server.Domain.Entities.Employee;

namespace Reports.Server.DAL.Repositories
{
    public interface IEmployeeRepository
    {
        IReadOnlyList<Employee> GetEmployees();
        Employee GetEmployeeById(Guid id);
        void InsertEmployee(Employee employee);
        void DeleteEmployee(Guid id);
        void UpdateEmployee(Employee employee);
    }
}