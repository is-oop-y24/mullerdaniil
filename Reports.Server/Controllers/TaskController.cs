using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reports.Server.Domain.Services;
using Reports.Server.DTOs;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [Route("/tasks/create")]
        public void CreateTask([FromQuery] string description,
            [FromQuery] Guid employeeId)
        {
            _taskService.CreateTask(description, employeeId, DateTime.Now);
        }

        [HttpGet]
        [Route("/tasks/find-all")]
        public List<TaskDTO> FindAll()
        {
            return _taskService.FindAll()
                .Select(task => new TaskDTO(task)).ToList();
        }

        [HttpGet]
        [Route("/tasks/find-by-id")]
        public TaskDTO FindById([FromQuery] Guid id)
        {
            return new TaskDTO(_taskService.FindById(id));
        }

        [HttpGet]
        [Route("/tasks/find-by-creation-time")]
        public List<TaskDTO> FindByCreationTime([FromQuery] DateTime dateTime)
        {
            return _taskService.FindByCreationTime(dateTime)
                .Select(task => new TaskDTO(task)).ToList();
        }
        
        [HttpGet]
        [Route("/tasks/find-by-last-modification-time")]
        public List<TaskDTO> FindByLastModificationTime([FromQuery] DateTime dateTime)
        {
            return _taskService.FindByLastModificationTime(dateTime)
                .Select(task => new TaskDTO(task)).ToList();
        }
        
        [HttpGet]
        [Route("/tasks/find-by-employee-id")]
        public List<TaskDTO> FindByEmployeeId([FromQuery] Guid id)
        {
            return _taskService.FindByEmployeeId(id)
                .Select(task => new TaskDTO(task)).ToList();
        }

        [HttpGet]
        [Route("/tasks/find-by-modificator-id")]
        public List<TaskDTO> FindByModificatorId([FromQuery] Guid modificatorId)
        {
            return _taskService.FindByModificator(modificatorId)
                .Select(task => new TaskDTO(task)).ToList();
        }
        
        [HttpGet]
        [Route("/tasks/find-tasks-of-subordinates-by-superior-id")]
        public List<TaskDTO> FindTasksOfSubordinatesBySuperiorId([FromQuery] Guid superiorId)
        {
            return _taskService.FindTasksOfSubordinatesBySuperiorId(superiorId)
                .Select(task => new TaskDTO(task)).ToList();
        }

        [HttpPost]
        [Route("/tasks/add-comment")]
        public void AddComment([FromQuery] string commentText,
            [FromQuery] Guid taskId,
            [FromQuery] Guid authorId)
        {
            _taskService.AddComment(commentText, taskId, authorId, DateTime.Now);
        }

        [HttpPut]
        [Route("/tasks/update-assigned-employee")]
        public void UpdateAssignedEmployee([FromQuery] Guid taskId,
            [FromQuery] Guid employeeId)
        {
            _taskService.UpdateAssignedEmployee(taskId, employeeId);
        }

        [HttpPut]
        [Route("/tasks/update-state")]
        public void UpdateState([FromQuery] Guid taskId)
        {
            _taskService.UpdateState(taskId, DateTime.Now);
        }

        [HttpPut]
        [Route("/tasks/update-description")]
        public void UpdateDescription([FromQuery] string description,
            [FromQuery] Guid taskId)
        {
            _taskService.UpdateDescription(description, taskId, DateTime.Now);
        }
    }
}