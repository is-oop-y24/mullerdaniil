using System;
using System.Collections.Generic;
using Reports.Server.Domain.Entities.Employee;

namespace Reports.Server.Domain.Services
{
    public interface IEmployeeService
    {
        void Create(string name, EmployeeState state, DateTime creationTime);
        IReadOnlyList<Employee> FindAll();
        Employee FindById(Guid id);
        IReadOnlyList<Employee> FindSubordinatesBySuperiorId(Guid superiorId);
        IReadOnlyList<Employee> FindEmployeesWithUnfinishedReports();
        void DeleteById(Guid id);
        void SetSuperiorById(Guid employeeId, Guid superiorId);
    }
}