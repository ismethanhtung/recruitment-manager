using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;

namespace InsternShip.Api.Controllers
{
    public class RecruiterEventPostController : BaseAPIController
    {
        private readonly IRecruiterEventPostService _recruiterEventPostService;
        private readonly IPermissionService _permissionService;
        public RecruiterEventPostController(IRecruiterEventPostService recruiterEventPostService, IPermissionService permissionService)
        {
            _recruiterEventPostService = recruiterEventPostService;
            _permissionService = permissionService;
        }
        
        [AllowAnonymous]
        [HttpGet("[action]")]
        /*link api: 
         * https://localhost:44309/api/RecruiterEventPost/GetAll
         * https://localhost:44309/api/RecruiterEventPost/GetAll?recId=&page=1&limit=3&search=
         * https://localhost:44309/api/RecruiterEventPost/GetAll?page=1&limit=1&search=
         * https://localhost:44309/api/RecruiterEventPost/GetAll?page=1&limit=1&search=IT
         * https://localhost:44309/api/RecruiterEventPost/GetAll?recId=1&page=1&limit=1&search=
         * https://localhost:44309/api/RecruiterEventPost/GetAll?recId=1&page=1&limit=1&search=IT
         */
        public async Task<IActionResult> GetAll(Guid? recId, int page, int limit, string? search)
        {
            var listRecruiterEventPost = await _recruiterEventPostService.GetAll(recId, page, limit, search,false);
            return Ok(listRecruiterEventPost);
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetParticipant(Guid eventPostId, int page, int limit, string? search)
        {
            var listRecruiterEventPost = await _recruiterEventPostService.GetParticipant(eventPostId, page, limit, search);
            return Ok(listRecruiterEventPost);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDeleted(Guid? recId, int page, int limit, string? search)
        {
            var listJobPost = await _recruiterEventPostService.GetAll(recId, page, limit, search, true);
            return Ok(listJobPost);
            //return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetById(Guid eventPostId)
        {
            var listJobPost = await _recruiterEventPostService.GetById(eventPostId);
            return Ok(listJobPost);
            //return Ok(response);
        }
        [Authorize(Roles = "Admin,Recruiter")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllRecPosted()
        {
            var listRec = await _recruiterEventPostService.GetAllRecPosted();
            return Ok(listRec);
        }
        [Authorize(Roles = "Recruiter")]

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateEventModel request)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _recruiterEventPostService.Create(inforToken.UserClaimId, request);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Recruiter")]
        [HttpPut("[action]")]
        public async Task<IActionResult> ApprovedParticipant(Guid CandidateId, Guid EventPostId)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _recruiterEventPostService.CandidateApprovedEvent(inforToken.UserClaimId, CandidateId, EventPostId);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]/{postId}")]
        public async Task<IActionResult> Restore(Guid postId)
        {
            var result = await _recruiterEventPostService.Restore(postId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin, Recruiter")]
        [HttpPut("[action]/{postId}")]
        public async Task<IActionResult> Update(Guid postId, EventUpdateModel request)
        {
            var result = await _recruiterEventPostService.Update(postId, request);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Recruiter")]

        [HttpDelete("[action]/{postId}")]
        public async Task<IActionResult> Delete(Guid postId)
        {
            var result = await _recruiterEventPostService.Delete(postId);
            return Ok(result);
        }

    }
}
