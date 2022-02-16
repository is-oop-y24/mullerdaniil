using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Server.DAL.Database;
using Reports.Server.Domain.Entities.Employee;

namespace Reports.Server.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ReportsDatabaseContext _context;

        public EmployeeRepository(ReportsDatabaseContext context)
        {
            _context = context;
        }
        
        public IReadOnlyList<Employee> GetEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee GetEmployeeById(Guid id)
        {
            return _context.Employees.Find(id);
        }

        public void InsertEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void DeleteEmployee(Guid id)
        {
            Employee employee = _context.Employees.Find(id);
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }

        public void UpdateEmployee(Employee employee)
        {
            Employee found = _context.Employees.Find(employee.Id);
            _context.Entry(found).CurrentValues.SetValues(employee);
            _context.SaveChanges();
        }
    }
}