using System;
using System.Collections.Generic;
using Reports.Server.Domain.Entities.Task;

namespace Reports.Server.Domain.Services
{
    public interface ITaskService
    {
        void CreateTask(string description, Guid employeeId, DateTime creationTime);
        IReadOnlyList<Task> FindAll();
        Task FindById(Guid id);
        IReadOnlyList<Task> FindByCreationTime(DateTime dateTime);
        IReadOnlyList<Task> FindByLastModificationTime(DateTime dateTime);
        IReadOnlyList<Task> FindByEmployeeId(Guid id);
        IReadOnlyList<Task> FindByModificator(Guid modificatorId);
        IReadOnlyList<Task> FindTasksOfSubordinatesBySuperiorId(Guid superiorId);
        void AddComment(string commentText, Guid taskId, Guid authorId, DateTime creationTime);
        void UpdateAssignedEmployee(Guid taskId, Guid employeeId);
        void UpdateState(Guid taskId, DateTime modificationTime);
        void UpdateDescription(string description, Guid taskId, DateTime modificationTime);
    }
}