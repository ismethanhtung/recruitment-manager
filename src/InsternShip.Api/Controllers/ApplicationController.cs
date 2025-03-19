using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    public class ApplicationController : BaseAPIController
    {
        private readonly IApplicationService _applicationService;
        private readonly IApplicationStatusUpdateService _applicationStatusUpdateService;
        private readonly IPermissionService _permissionService;

        public ApplicationController(IApplicationService applicationService, IApplicationStatusUpdateService applicationStatusUpdateService, IPermissionService permissionService)
        {
            _applicationService = applicationService;
            _applicationStatusUpdateService = applicationStatusUpdateService;
            _permissionService = permissionService;
        }

        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpGet("[action]")] 
        public async Task<IActionResult>GetAll(Guid? jobpostId, Guid? candidateId, string? search, int page, int limit, string? status)
        {
            var applicationList = await _applicationService.GetAll(search, page, limit, status, candidateId, jobpostId, false);
            return Ok(applicationList);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByCandidateId(Guid candidateId, int page, int limit, string? status)
        {
            var applicationList = await _applicationService.GetAll("", page, limit, status, candidateId, null, false);
            return Ok(applicationList);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByStatus(string? search, int page, int limit, string? status)
        {
            var applicationList = await _applicationService.GetAll(search, page, limit, status, null, null, false);
            return Ok(applicationList);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDeleted(Guid? jobpostId,Guid? candidateId, string? search, int page, int limit, string? status)
        {
            var applicationList = await _applicationService.GetAll(search, page, limit, status, candidateId, jobpostId, true);
            return Ok(applicationList);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetById(Guid applicationId)
        {
            var applicationList = await _applicationService.GetById(applicationId);
            return Ok(applicationList);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetScore(Guid applicationId)
        {
            var applicationList = await _applicationService.GetScoreByApplicationId(applicationId);
            return Ok(applicationList);
        }
        [Authorize(Roles = "Candidate")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(Guid jobPostId)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _applicationService.Create(inforToken.UserClaimId, jobPostId);
            return Ok(result);
        }
        [Authorize(Roles = "Interviewer,Recruiter")]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateStatus(string statusRequest, Guid applicationId)
        {
            var applicationList = await _applicationStatusUpdateService.UpdateStatus(statusRequest, applicationId);
            return Ok(applicationList);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Restore(Guid applicationId)
        {
            var applicationList = await _applicationService.Restore(applicationId);
            return Ok(applicationList);
        }
        [Authorize(Roles = "Candidate,Admin")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(Guid applicationId)
        {
            var applicationList = await _applicationService.Delete(applicationId);
            return Ok(applicationList);
        }
    }
}
