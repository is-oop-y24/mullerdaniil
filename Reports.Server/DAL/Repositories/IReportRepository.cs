using System;
using System.Collections.Generic;
using Reports.Server.Domain.Entities.Report;

namespace Reports.Server.DAL.Repositories
{
    public interface IReportRepository
    {
        IReadOnlyList<Report> GetReports();
        Report GetReportById(Guid id);
        void InsertReport(Report report);
        void DeleteReport(Guid id);
        void UpdateReport(Report report);
    }
}