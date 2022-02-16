using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Server.DAL.Repositories;
using Reports.Server.Domain.Entities.Employee;
using Reports.Server.Domain.Entities.Task;

namespace Reports.Server.Domain.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public TaskService(ITaskRepository taskRepository, IEmployeeRepository employeeRepository)
        {
            _taskRepository = taskRepository;
            _employeeRepository = employeeRepository;
        }
        
        public void CreateTask(string description, Guid employeeId, DateTime creationTime)
        {
            Employee employee = _employeeRepository.GetEmployeeById(employeeId);
            var task = new Task(description, creationTime, employee);
            _taskRepository.InsertTask(task);
        }

        public IReadOnlyList<Task> FindAll()
        {
            return _taskRepository.GetTasks();
        }

        public Task FindById(Guid id)
        {
            return _taskRepository.GetTaskById(id);
        }

        public IReadOnlyList<Task> FindByCreationTime(DateTime dateTime)
        {
            return _taskRepository.GetTasks().Where(task => task.CreationTime.Date == dateTime.Date).ToList();
        }

        public IReadOnlyList<Task> FindByLastModificationTime(DateTime dateTime)
        {
            return _taskRepository.GetTasks().Where(task => task.LastModificationTime == dateTime.Date).ToList();
        }

        public IReadOnlyList<Task> FindByEmployeeId(Guid id)
        {
            return _taskRepository.GetTasks().Where(task => task.Employee.Id == id).ToList();
        }

        public IReadOnlyList<Task> FindByModificator(Guid modificatorId)
        {
            return _taskRepository.GetTasks()
                .Where(task => task.UpdateHistory.Any(update => update.AuthorId == modificatorId)).ToList();
        }

        public IReadOnlyList<Task> FindTasksOfSubordinatesBySuperiorId(Guid superiorId)
        {
            IReadOnlyList<Employee> subordinates = _employeeRepository.GetEmployees()
                .Where(employee => employee.SuperiorId == superiorId).ToList();

            return _taskRepository.GetTasks().Where(task => subordinates.Contains(task.Employee)).ToList();
        }

        public void AddComment(string commentText, Guid taskId, Guid authorId, DateTime creationTime)
        {
            Task task = _taskRepository.GetTaskById(taskId);
            Employee author = _employeeRepository.GetEmployeeById(authorId);
            var comment = new TaskComment(commentText, author, creationTime);
            task.AddComment(comment, creationTime);
        }

        public void UpdateAssignedEmployee(Guid taskId, Guid employeeId)
        {
            Task task = _taskRepository.GetTaskById(taskId);
            Employee previousEmployee = task.Employee;
            Employee nextEmployee = _employeeRepository.GetEmployeeById(employeeId);
            task.SetEmployee(nextEmployee);
            previousEmployee.RemoveTask(task);
            nextEmployee.AddTask(task);
            _employeeRepository.UpdateEmployee(previousEmployee);
            _employeeRepository.UpdateEmployee(nextEmployee);
        }

        public void UpdateState(Guid taskId, DateTime modificationTime)
        {
            Task task = _taskRepository.GetTaskById(taskId);
            task.UpdateState(modificationTime);
            _taskRepository.UpdateTask(task);
        }

        public void UpdateDescription(string description, Guid taskId, DateTime modificationTime)
        {
            Task task = _taskRepository.GetTaskById(taskId);
            task.SetDescription(description, modificationTime);
        }
    }
}