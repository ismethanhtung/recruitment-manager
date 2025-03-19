using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    public class MeetingController : BaseAPIController
    {
        private readonly IMeetingService _meetingService;
        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }
        
        [Authorize(Roles="Admin, Recruiter, Interviewer")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateMeetingModel request)
        {
            var result = await _meetingService.CreateZoomMeeting(request);
            return Ok(result);
        }
        [Authorize(Roles = "Admin, Recruiter, Interviewer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(string? type, int? limit, int? page)
        {
            var result = await _meetingService.GetAll(type, limit, page);
            return Ok(result);
        }
        [Authorize(Roles = "Admin, Recruiter, Interviewer")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _meetingService.GetById(id);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _meetingService.Delete(id);
            return Ok(result);
        }

    }
}
