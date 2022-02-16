using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Server.DAL.Repositories;
using Reports.Server.Domain.Entities.Employee;
using Reports.Server.Domain.Entities.Report;
using Reports.Server.Domain.Entities.Task;
using Reports.Server.Tools;

namespace Reports.Server.Domain.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ReportService(IReportRepository reportRepository, ITaskRepository taskRepository, IEmployeeRepository employeeRepository)
        {
            _reportRepository = reportRepository;
            _taskRepository = taskRepository;
            _employeeRepository = employeeRepository;
        }
        
        public Report CreateTeamReport(string description, DateTime creationTime, TimeSpan sprintDuration)
        {
            Employee teamLead = _employeeRepository.GetEmployees().ToList()
                .Find(employee => employee.State == EmployeeState.TeamLead);
            if (teamLead is null)
            {
                throw new ReportsException("Unable to create a team report. Team lead no found.");
            }

            if (_employeeRepository.GetEmployees().ToList().Any(employee => employee.Report.State == ReportState.Open))
            {
                throw new ReportsException(
                    "Unable to create a team report. Not every employee has finished the report.");
            }

            Report teamReport = CreateReport(description, teamLead.Id, creationTime, sprintDuration);
            return teamReport;
        }

        public Report CreateReport(string description, Guid authorId, DateTime creationTime, TimeSpan sprintDuration)
        {
            DateTime timeLimit = DateTime.Now - sprintDuration;
            Employee author = _employeeRepository.GetEmployeeById(authorId);
            IReadOnlyList<Task> finishedTasksOfAuthorAndSubordinates =
                _taskRepository.GetTasks()
                    .Where(task => task.State == TaskState.Resolved &&
                                   task.LastModificationTime > timeLimit &&
                                   (task.Employee.Id == authorId || task.Employee.SuperiorId == authorId)).ToList();

            var report = new Report(author.Id, creationTime, description);
            foreach (Task task in finishedTasksOfAuthorAndSubordinates)
            {
                report.AddTask(task);
            }
            author.FinishReport(report);
            _reportRepository.InsertReport(report);
            return report;
        }

        public IReadOnlyList<Report> FindAll()
        {
            return _reportRepository.GetReports();
        }

        public Report FindById(Guid id)
        {
            return _reportRepository.GetReportById(id);
        }

        public Report FindByAuthorId(Guid id)
        {
            return _reportRepository.GetReports().ToList().Find(report => report.AuthorId == id);
        }

        public IReadOnlyList<Report> FindFinishedReportsOfSubordinatesBySuperiorIdAndTimeSpan(Guid superiorId, TimeSpan timeSpan)
        {
            DateTime limitTime = DateTime.Now - timeSpan;
            return _reportRepository.GetReports().Where(report => report.State == ReportState.Finished &&
                                                                  _employeeRepository.GetEmployeeById(report.AuthorId).SuperiorId == superiorId &&
                                                                  report.FinishTime > limitTime).ToList();
        }

        public void UpdateDescription(Guid id, string description)
        {
            Report updatedReport = _reportRepository.GetReportById(id);
            updatedReport.SetDescription(description);
            _reportRepository.UpdateReport(updatedReport);
        }

        public void UpdateState(Guid id, ReportState state)
        {
            Report updatedReport = _reportRepository.GetReportById(id);
            updatedReport.SetState(state);
            _reportRepository.UpdateReport(updatedReport);
        }

        public void AddTask(Guid reportId, Guid taskId)
        {
            Report updatedReport = _reportRepository.GetReportById(reportId);
            Task task = _taskRepository.GetTaskById(taskId);
            updatedReport.AddTask(task);
            _reportRepository.UpdateReport(updatedReport);
        }
    }
}