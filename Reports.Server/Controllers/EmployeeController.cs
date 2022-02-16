using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reports.Server.Domain.Entities.Employee;
using Reports.Server.Domain.Services;
using Reports.Server.DTOs;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        [Route("/employees/create")]
        public void Create([FromQuery] string name,
            [FromQuery] EmployeeState state)
        {
            _employeeService.Create(name, state, DateTime.Now);
        }
        
        [HttpGet]
        [Route("/employees/find-all")]
        public List<EmployeeDTO> FindAll()
        {
            return _employeeService.FindAll().Select(employee => new EmployeeDTO(employee)).ToList();
        }

        [HttpGet]
        [Route("/employees/find-by-id")]
        public EmployeeDTO FindById([FromQuery] Guid id)
        {
            return new EmployeeDTO(_employeeService.FindById(id));
        }

        [HttpGet]
        [Route("/employees/find-subordinates-by-superior-id")]
        public List<EmployeeDTO> FindSubordinatesBySuperiorId([FromQuery] Guid superiorId)
        {
            return _employeeService.FindSubordinatesBySuperiorId(superiorId)
                .Select(employee => new EmployeeDTO(employee)).ToList();
        }

        [HttpGet]
        [Route("/employees/find-employees-with-unfinished-reports")]
        public List<EmployeeDTO> FindEmployeesWithUnfinishedReports()
        {
            return _employeeService.FindEmployeesWithUnfinishedReports()
                .Select(employee => new EmployeeDTO(employee)).ToList();
        }

        [HttpDelete]
        [Route("/employees/delete-by-id")]
        public void DeleteById([FromQuery] Guid id)
        {
            _employeeService.DeleteById(id);
        }

        [HttpPut]
        [Route("/employees/set-superior-by-id")]
        public void SetSuperiorById([FromQuery] Guid employeeId,
            [FromQuery] Guid superiorId)
        {
            _employeeService.SetSuperiorById(employeeId, superiorId);
        }
    }
}