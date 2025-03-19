using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ExportDataController : ControllerBase
    {
        private readonly IExportDataService _exportdataService;
        public ExportDataController(IExportDataService exportdataService)
        {
            _exportdataService = exportdataService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CandidateReport()
        {
            byte[] ExcelData = await _exportdataService.CandidateReport();
            return File(ExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CandidateData.xlsx");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> RecruitmentReport()
        {
            byte[] ExcelData = await _exportdataService.RecruitmentReport();
            return File(ExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RecruitmentData.xlsx");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> EventReport()
        {
            byte[] ExcelData = await _exportdataService.EventReport();
            return File(ExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EventData.xlsx");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> InterviewReport()
        {
            byte[] ExcelData = await _exportdataService.InterviewReport();
            return File(ExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InterviewData.xlsx");
        }
    }
}
