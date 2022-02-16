using System;
using System.Collections.Generic;
using Reports.Server.Domain.Entities.Report;

namespace Reports.Server.Domain.Services
{
    public interface IReportService
    {
        Report CreateTeamReport(string description, DateTime creationTime, TimeSpan sprintDuration);
        Report CreateReport(string description, Guid authorId, DateTime creationTime, TimeSpan sprintDuration);
        IReadOnlyList<Report> FindAll();
        Report FindById(Guid id);
        Report FindByAuthorId(Guid id);
        IReadOnlyList<Report> FindFinishedReportsOfSubordinatesBySuperiorIdAndTimeSpan(Guid superiorId, TimeSpan timeSpan);
        void UpdateDescription(Guid id, string description);
        void UpdateState(Guid id, ReportState state);
        void AddTask(Guid reportId, Guid taskId);
    }
}