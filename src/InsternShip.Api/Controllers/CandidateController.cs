using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    public class CandidateController : BaseAPIController
    {
        private readonly ICandidateService _candidateService;
        private readonly IRecruiterJobPostService _recruiterJobPostService;
        private readonly IPermissionService _permissionService;
        public CandidateController(ICandidateService candidateService, IPermissionService permissionService, IRecruiterJobPostService recruiterJobPostService)
        {
            _candidateService = candidateService;
            _permissionService = permissionService;
            _recruiterJobPostService = recruiterJobPostService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(string? search, int page, int limit)
        {
            var candidateList = await _candidateService.GetAll(search, page, limit,false);
            return Ok(candidateList);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDeleted(string? search, int page, int limit)
        {
            var candidateList = await _candidateService.GetAll(search, page, limit,true);
            return Ok(candidateList);
        }
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetById(Guid CandidateId)
        {
            var result = await _candidateService.GetById(CandidateId);
            return Ok(result);
        }
        /*[HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateCandidateModel request)
        {
            var result = await _candidateService.Create(request);
            return Ok(result);
        }*/
        [Authorize(Roles = "Candidate")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMySessions(int page, int limit)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _candidateService.GetAllSessions(inforToken.UserClaimId, page, limit);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCandidateSessions(Guid candidateId ,int page, int limit)
        {
            var result = await _candidateService.GetAllSessions(candidateId, page, limit);
            return Ok(result);
        }
        [Authorize(Roles = "Candidate")]
        [HttpGet("[action]")]
        public async Task<IActionResult> SuggestionJobPost(int page, int limit)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var listJobPost = await _candidateService.SuggestionJobPost(inforToken.UserClaimId, page, limit);
            return Ok(listJobPost);
        }
        [Authorize(Roles = "Candidate")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMyAppliedJob(int page, int limit, string? status)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var listJobPost = await _recruiterJobPostService.GetAllAppliedOfCandidate(inforToken.UserClaimId, page, limit, status);
            return Ok(listJobPost);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCandidateAppliedJob(Guid candidateId, int page, int limit, string? status)
        {
            var listJobPost = await _recruiterJobPostService.GetAllAppliedOfCandidate(candidateId, page, limit, status);
            return Ok(listJobPost);
        }
        [Authorize(Roles = "Candidate")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMyJoinedEvent(string? search, int page, int limit)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _candidateService.GetAllEventOfCandidate(inforToken.UserClaimId, search, page, limit);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCandidateJoinedEvent(Guid candidateId, string? search, int page, int limit)
        {
            var listJobPost = await _candidateService.GetAllEventOfCandidate(candidateId, search, page, limit);
            return Ok(listJobPost);
        }
        [Authorize(Roles = "Candidate")]
        [HttpPost("[action]")]
        public async Task<IActionResult> JoinEvent(Guid eventId)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _candidateService.JoinEvent(inforToken.UserClaimId, eventId);
            return Ok(result);
        }
        [Authorize(Roles = "Candidate")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(CandidateUserInfoUpdateModel request)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _candidateService.Update(inforToken.UserId, request);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Restore(Guid candidateId)
        {
            var result = await _candidateService.Restore(candidateId);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(Guid candidateId)
        {
            var result = await _candidateService.Delete(candidateId);
            return Ok(result);
        }
        [Authorize(Roles = "Candidate")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> UnregisterEvent(Guid EvenPostId)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _candidateService.CancleEvent(inforToken.UserClaimId, EvenPostId);
            return Ok(result);
        }


    }
}

