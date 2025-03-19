using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    public class InterviewController : BaseAPIController
    {
        private readonly IInterviewService _interviewService;
        private readonly IMailerService _mailerService;
        private readonly IInterviewSessionService _interviewSessionService;

        public InterviewController(IInterviewService interviewService, IMailerService mailerService, IInterviewSessionService interviewSessionService)
        {
            _interviewService = interviewService;
            _mailerService = mailerService;
            _interviewSessionService = interviewSessionService;
        }
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(int page, int limit)
        {
            var interviewList = await _interviewService.GetAll(page, limit,false);
            return Ok(interviewList);
        }
        [Authorize(Roles = "Admin,Recruiter")]
        [HttpGet("[action]")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetInterviewsCurrentWeek()
        {
            //Start from Sunday
            var numOInterv = await _interviewService.GetAllInterviewInWeek();
            return Ok(numOInterv);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<IActionResult> GetRecInterviewsCurrentWeek(Guid recId)
        {
            var interviewList = await _interviewService.GetAllInWeekByRecId(recId);
            return Ok(interviewList);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDeleted(int page, int limit)
        {
            var interviewList = await _interviewService.GetAll(page,limit,true);
            return Ok(interviewList);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetInterviewById(Guid interviewId)
        {
            var interviewList = await _interviewService.GetById(interviewId);
            return Ok(interviewList);
        }
        [HttpGet("[action]/{interviewId}")]
        [Authorize(Roles = "Admin, Recruiter, Interviewer")]
        public async Task<IActionResult> GetDetail(Guid interviewId)
        {
            var sessionDetail = await _interviewSessionService.GetDetail(interviewId);
            return Ok(sessionDetail);
        }
        [Authorize(Roles = "Admin,Recruiter")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateInterviewModel request)
        {
            var result = await _interviewService.Create(request);
            if (result) {
                await _mailerService.SendEmailInterview(request);
            }
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Recruiter")]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateSession(Guid interviewId, Guid interviewerId)
        {
            var result = await _interviewService.CreateSession(interviewId, interviewerId);
            if (result)
            {
                await _mailerService.SendEmailInterview(interviewerId, interviewId);
            }
            return Ok(result);
            //return Ok(result);
        }
        /*[Authorize(Roles = "Admin,Interviewer,Recruiter")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddTestToSession(Guid interviewId, Guid testId, Guid interviewerId)
        {
            var result = await _interviewService.AddTest(interviewId, testId, interviewerId);
            return Ok(result);
        }*/
        [Authorize(Roles = "Admin,Recruiter")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(Guid request)
        {
            var interviewList = await _interviewService.Delete(request);
            return Ok(interviewList);
        }
        [Authorize(Roles = "Admin,Recruiter")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(Guid interviewId, InterviewUpdateModel request)
        {
            var interviewList = await _interviewService.Update(interviewId, request);
            return Ok(interviewList);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Restore(Guid interviewId)
        {
            var interviewList = await _interviewService.Restore(interviewId);
            return Ok(interviewList);
        }

    }
}
