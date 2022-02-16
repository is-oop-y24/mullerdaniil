using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reports.Server.Domain.Entities.Report;
using Reports.Server.Domain.Services;
using Reports.Server.DTOs;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/reports")]
    public class ReportController : ControllerBase
    {
        private readonly TimeSpan _sprintDuration = TimeSpan.FromDays(7);
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        [Route("/reports/create-team-report")]
        public void CreateTeamReport([FromQuery] string description)
        {
            _reportService.CreateTeamReport(description, DateTime.Now, _sprintDuration);
        }

        [HttpPost]
        [Route("/reports/create-report")]
        public void CreateReport([FromQuery] string description,
            [FromQuery] Guid authorId)
        {
            _reportService.CreateReport(description, authorId, DateTime.Now, _sprintDuration);
        }

        [HttpGet]
        [Route("/reports/find-all")]
        public List<ReportDTO> FindAll()
        {
            return _reportService.FindAll()
                .Select(report => new ReportDTO(report)).ToList();
        }

        [HttpGet]
        [Route("/reports/find-by-id")]
        public ReportDTO FindById([FromQuery] Guid id)
        {
            return new ReportDTO(_reportService.FindById(id));
        }
        
        [HttpGet]
        [Route("/reports/find-by-author-id")]
        public ReportDTO FindByAuthorId([FromQuery] Guid id)
        {
            return new ReportDTO(_reportService.FindByAuthorId(id));
        }

        [HttpGet]
        [Route("/reports/find-finished")]
        public List<ReportDTO> FindFinished([FromQuery] Guid superiorId,
            [FromQuery] int numberOfDays)
        {
            return _reportService.FindFinishedReportsOfSubordinatesBySuperiorIdAndTimeSpan(superiorId, TimeSpan.FromDays(numberOfDays))
                .Select(report => new ReportDTO(report)).ToList();    
        }

        [HttpPut]
        [Route("/reports/update-description")]
        public void UpdateDescription([FromQuery] Guid id,
            [FromQuery] string description)
        {
            _reportService.UpdateDescription(id, description);
        }

        [HttpPut]
        [Route("/reports/update-state")]
        public void UpdateState([FromQuery] Guid id,
            [FromQuery] ReportState state)
        {
            _reportService.UpdateState(id, state);
        }

        [HttpPost]
        [Route("/reports/add-task")]
        public void AddTask([FromQuery] Guid reportId,
            [FromQuery] Guid taskId)
        {
            _reportService.AddTask(reportId, taskId);
        }
    }
}