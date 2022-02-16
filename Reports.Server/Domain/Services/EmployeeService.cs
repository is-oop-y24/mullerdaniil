using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Server.DAL.Repositories;
using Reports.Server.Domain.Entities.Employee;
using Reports.Server.Domain.Entities.Report;
using Reports.Server.Tools;

namespace Reports.Server.Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        
        public void Create(string name, EmployeeState state, DateTime creationTime)
        {
            int teamLeadCount = _repository.GetEmployees()
                .Where(employee => employee.State == EmployeeState.TeamLead)
                .ToList().Count;

            if (teamLeadCount == 1 && state == EmployeeState.TeamLead)
            {
                throw new ReportsException("Unable to create a team lead. One team lead already exists.");
            }
            
            _repository.InsertEmployee(new Employee(name, state, creationTime));
        }

        public IReadOnlyList<Employee> FindAll()
        {
            return _repository.GetEmployees();
        }

        public Employee FindById(Guid id)
        {
            return _repository.GetEmployeeById(id);
        }


        public IReadOnlyList<Employee> FindSubordinatesBySuperiorId(Guid superiorId)
        {
            return _repository.GetEmployees().Where(employee => employee.SuperiorId == superiorId).ToList();
        }

        public IReadOnlyList<Employee> FindEmployeesWithUnfinishedReports()
        {
            return _repository.GetEmployees().Where(employee => employee.Report.State == ReportState.Open).ToList();
        }

        public void DeleteById(Guid id)
        {
            _repository.DeleteEmployee(id);
        }

        public void SetSuperiorById(Guid employeeId, Guid superiorId)
        {
            Employee updatedEmployee = _repository.GetEmployeeById(employeeId);
            Employee superior = _repository.GetEmployeeById(superiorId);
            updatedEmployee.SetSuperior(superior);
            _repository.UpdateEmployee(updatedEmployee);
        }
    }
}