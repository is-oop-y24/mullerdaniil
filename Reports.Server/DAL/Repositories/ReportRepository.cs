using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Server.DAL.Database;
using Reports.Server.Domain.Entities.Report;

namespace Reports.Server.DAL.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportsDatabaseContext _context;

        public ReportRepository(ReportsDatabaseContext context)
        {
            _context = context;
        }
        
        public IReadOnlyList<Report> GetReports()
        {
            return _context.Reports.ToList();
        }

        public Report GetReportById(Guid id)
        {
            return _context.Reports.Find(id);
        }

        public void InsertReport(Report report)
        {
            _context.Reports.Add(report);
            _context.SaveChanges();
        }

        public void DeleteReport(Guid id)
        {
            Report report = _context.Reports.Find(id);
            _context.Reports.Remove(report);
            _context.SaveChanges();
        }

        public void UpdateReport(Report report)
        {
            Report found = _context.Reports.Find(report.Id);
            _context.Entry(found).CurrentValues.SetValues(report);
            _context.SaveChanges();
        }
    }
}