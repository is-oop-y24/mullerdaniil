using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Server.Domain.Entities.Report;
using Reports.Server.Tools;

namespace Reports.Server.Domain.Entities.Employee
{
    public class Employee
    {
        private List<Task.Task> _tasks;

        public Employee()
        {
        }
        public Employee(string name, EmployeeState state, DateTime creationTime)
        {
            Id = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ReportsException("Employee's name must not be null, empty or whitespace only.");
            }

            Name = name;
            State = state;
            Report = new Report.Report(Id, creationTime, "");
            Report.SetState(ReportState.Open);
            CreationTime = creationTime;
            _tasks = new List<Task.Task>();
        }
        
        public Guid Id { get; }
        public string Name { get; }
        public Guid SuperiorId { get; private set; }
        public Report.Report Report { get; private set; }

        public EmployeeState State { get; }
        public DateTime CreationTime { get; }

        public void SetSuperior(Employee superior)
        {
            if (superior is null)
            {
                throw new ReportsException("Superior must not be null.");
            }

            if (superior.Id == Id)
            {
                throw new ReportsException("Superior can not be the same employee.");
            }

            switch (superior.State)
            {
                case EmployeeState.TeamLead:
                    SuperiorId = superior.Id;
                    break;
                case EmployeeState.Supervisor:
                    if (State == EmployeeState.TeamLead)
                    {
                        throw new ReportsException("Supervisor can not be a superior of the team lead");
                    }
                    SuperiorId = superior.Id;
                    break;
                case EmployeeState.Employee:
                    throw new ReportsException("Employee can not be a superior");
            }
        }
        
        public void FinishReport(Report.Report report)
        {
            Report = report ?? throw new ReportsException("Report must not be null");
            Report.SetState(ReportState.Finished);
        }

        public void AddTask(Task.Task task)
        {
            if (task is null)
            {
                throw new ReportsException("Task must not be null.");
            }

            if (_tasks.Any(t => t.Id == task.Id))
            {
                throw new ReportsException("Employee has the task already.");
            }
            
            _tasks.Add(task);
        }

        public void RemoveTask(Task.Task task)
        {
            if (task is null)
            {
                throw new ReportsException("Task must not be null.");
            }

            if (_tasks.Any(t => t.Id == task.Id))
            {
                throw new ReportsException("Employee does not have the task.");
            }

            _tasks.Remove(task);
        }

        public void SetReportState()
        {
            
        }
    }
}