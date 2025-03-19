using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    public class RecruiterJobPostController : BaseAPIController
    {
        private readonly IRecruiterJobPostService _recruiterJobPostService;
        private readonly IPermissionService _permissionService;
        private readonly IApplicationService _applicationService;
        public RecruiterJobPostController(IRecruiterJobPostService recruiterJobPostService, IPermissionService permissionService, IApplicationService applicationService)
        {
            _recruiterJobPostService = recruiterJobPostService;
            _permissionService = permissionService;
            _applicationService = applicationService;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        /*link api: 
         * https://localhost:44309/api/RecruiterJobPost/GetAll
         * https://localhost:44309/api/RecruiterJobPost/GetAll?recId=&page=1&limit=3&search=
         * https://localhost:44309/api/RecruiterJobPost/GetAll?page=1&limit=1&search=
         * https://localhost:44309/api/RecruiterJobPost/GetAll?page=1&limit=1&search=IT
         * https://localhost:44309/api/RecruiterJobPost/GetAll?recId=1&page=1&limit=1&search=
         * https://localhost:44309/api/RecruiterJobPost/GetAll?recId=1&page=1&limit=1&search=IT
         */
        public async Task<IActionResult> GetAll(Guid? recId, int page, int limit, string? search)
        {
            var listJobPost = await _recruiterJobPostService.GetAll(recId, page, limit, search, false);
            return Ok(listJobPost);
            //return Ok(response);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDeleted(Guid? recId, int page, int limit, string? search)
        {
            var listJobPost = await _recruiterJobPostService.GetAll(recId, page, limit, search,true);
            return Ok(listJobPost);
            //return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetById(Guid jobPostId)
        {
            var listJobPost = await _recruiterJobPostService.GetById(jobPostId);
            return Ok(listJobPost);
            //return Ok(response);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetApplications(Guid jobPostId, int page, int limit, string? search)
        {
            var listRecruiterEventPost = await _applicationService.GetAll(search, page, limit, "", null, jobPostId, false);
            return Ok(listRecruiterEventPost);
        }
        [Authorize(Roles = "Admin,Recruiter")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllRecPosted()
        {
            var listRec = await _recruiterJobPostService.GetAllRecPosted();
            return Ok(listRec);
        }
        [Authorize(Roles = "Recruiter")]

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateJobModel request)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _recruiterJobPostService.Create(inforToken.UserClaimId, request);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Recruiter")]
        [HttpPut("[action]/{postId}")]
        public async Task<IActionResult> Update(Guid postId, JobUpdateModel request)
        {
            var result = await _recruiterJobPostService.Update(postId, request);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]/{postId}")]
        public async Task<IActionResult> Restore(Guid postId)
        {
            var result = await _recruiterJobPostService.Restore(postId);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Recruiter")]
        [HttpDelete("[action]/{postId}")]
        public async Task<IActionResult> Delete(Guid postId)
        {
            var result = await _recruiterJobPostService.Delete(postId);
            return Ok(result);
        }

    }
}
