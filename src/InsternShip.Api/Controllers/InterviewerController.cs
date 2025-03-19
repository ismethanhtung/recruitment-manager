using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    public class InterviewerController : BaseAPIController
    {
        private readonly IInterviewerService _interviewerService;
        private readonly IPermissionService _permissionService;
        private readonly IInterviewService _interviewService;

        public InterviewerController(IInterviewerService interviewerService, IPermissionService permissionService, IInterviewService interviewService)
        {
            _interviewerService = interviewerService;
            _permissionService = permissionService;
            _interviewService = interviewService;
        }
        [Authorize(Roles = "Admin, Recruiter")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(string? search, int page, int limit)
        {
            var interviewerList = await _interviewerService.GetAll(search, page, limit,false);
            return Ok(interviewerList);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDeleted(string? search, int page, int limit)
        {
            var interviewerList = await _interviewerService.GetAll(search, page, limit, true);
            return Ok(interviewerList);
        }
        [Authorize(Roles = "Admin, Recruiter, Interviewer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetInterviewerById(Guid interviewerId)
        {
            var result = await _interviewerService.GetById(interviewerId);
            return Ok(result);
        }
        
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMyInterviews(int page, int limit)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var interviewList = await _interviewService.GetByInterviewerId(inforToken.UserClaimId, page, limit);
            return Ok(interviewList);
        }
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetInterviewerInterviews(Guid interviewerId, int page, int limit)
        {
            var interviewList = await _interviewService.GetByInterviewerId(interviewerId, page, limit);
            return Ok(interviewList);
        }

        [Authorize(Roles = "Interviewer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMySessions(int page, int limit)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _interviewerService.GetAllSessions(inforToken.UserClaimId, page, limit);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Recruiter")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetInterviewerSessions(Guid interviewerId, int page, int limit)
        {
            var result = await _interviewerService.GetAllSessions(interviewerId, page, limit);
            return Ok(result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> CheckDuplicate(Guid interviewerId, DateTime startTime, DateTime endTime)
        {
            var result = await _interviewerService.CheckDuplicate(interviewerId, startTime, endTime);
            return Ok(result);
        }
        
        [Authorize(Roles = "Interviewer")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(InterviewerUserInfoUpdateModel request)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _interviewerService.Update(inforToken.UserId, request);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Restore(Guid interviewerId)
        {
            var result = await _interviewerService.Restore(interviewerId);
            return Ok(result);
        }
        
        [Authorize(Roles = "Interviewer")]
        [HttpPut("[action]")]
        public async Task<IActionResult> SaveScore(Guid interviewSessionId, UpdatedInterviewSessionModel request)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _interviewerService.SaveScore(inforToken.UserClaimId, interviewSessionId, request);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(Guid interviewerId)
        {
            var result = await _interviewerService.Delete(interviewerId);
            return Ok(result);
        }
    }
}
