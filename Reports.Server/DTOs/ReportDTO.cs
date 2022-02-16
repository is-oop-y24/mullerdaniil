using System;
using Reports.Server.Domain.Entities.Report;

namespace Reports.Server.DTOs
{
    public class ReportDTO
    {
        public ReportDTO(Report report)
        {
            Id = report.Id;
            State = report.State.ToString();
            AuthorId = report.AuthorId;
            CreationTime = report.CreationTime;
            FinishTime = report.FinishTime;
            Description = report.Description;
        }
        
        public Guid Id { get; }
        public string State { get; }
        public Guid AuthorId { get; }
        public DateTime CreationTime { get; }
        public DateTime FinishTime { get; }
        public string Description { get; }
    }
}