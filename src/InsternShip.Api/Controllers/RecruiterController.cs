using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    public class RecruiterController : BaseAPIController
    {
        private readonly IRecruiterService _recruiterService;

        private readonly IRecruiterJobPostService _recruiterJobPostService;
        private readonly IRecruiterEventPostService _recruiterEventPostService;
        private readonly IPermissionService _permissionService;
        private readonly IMailerService _mailerService;
        public RecruiterController(IRecruiterService service, IRecruiterJobPostService recruiterJobPostService, IPermissionService permissionService, IRecruiterEventPostService recruiterEventPostService, IMailerService mailerService)
        {
            _recruiterService = service;
            _recruiterJobPostService = recruiterJobPostService;
            _permissionService = permissionService;
            _recruiterEventPostService = recruiterEventPostService;
            _mailerService = mailerService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(string? search, int page, int limit)
        {
            var listData = await _recruiterService.GetAll(search, page, limit, false);
            return Ok(listData);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDeleted(string? search, int page, int limit)
        {
            var listData = await _recruiterService.GetAll(search, page, limit, true);
            return Ok(listData);
        }
        [Authorize(Roles = "Admin,Recruiter")]

        [HttpGet("[action]/{recruiterId}")]
        public async Task<IActionResult> GetById(Guid recruiterId)
        {
            var data = await _recruiterService.GetById(recruiterId);
            return Ok(data);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetScheduleInterview(Guid recId)
        {
            var result = await _recruiterService.GetScheduleInterview(recId);
            return Ok(result);
        }
        [Authorize(Roles = "Recruiter")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMyJobPosts(int page, int limit, string? search)
        {
            var infoToken = await _permissionService.GetInfoToken();
            var recId = infoToken.UserClaimId;
            var listJobPost = await _recruiterJobPostService.GetAll(recId, page, limit, search, false);
            return Ok(listJobPost);
        }
        [Authorize(Roles = "Recruiter")]
        [HttpGet("[action]")]

        public async Task<IActionResult> GetMyEventPosts(int page, int limit, string? search)
        {
            var infoToken = await _permissionService.GetInfoToken();
            var recId = infoToken.UserClaimId;
            var listRecruiterEventPost = await _recruiterEventPostService.GetAll(recId, page, limit, search, false);
            return Ok(listRecruiterEventPost);
        }
        [Authorize(Roles = "Admin,Recruiter")]

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(RecruiterUserInfoUpdateModel request)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var result = await _recruiterService.Update(inforToken.UserId, request);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("[action]")]
        public async Task<IActionResult> Restore(Guid recruiterId)
        {
            var result = await _recruiterService.Restore(recruiterId);
            return Ok(result);
        }


        [Authorize(Roles = "Recruiter")]
        [HttpPut("[action]")]

        public async Task<IActionResult> AcceptApplication(Guid applicationId)
        {
            var infoToken = await _permissionService.GetInfoToken();
            var recId = infoToken.UserClaimId;
            await _recruiterService.AcceptApplication(recId, applicationId);
            var result = await _mailerService.SendEmailAccept(applicationId);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]")]

        public async Task<IActionResult> AcceptOtherApplication(Guid recruiterId, Guid applicationId)
        {
            await _recruiterService.AcceptApplication(recruiterId, applicationId);
            var result = await _mailerService.SendEmailAccept(applicationId);
            return Ok(result);
        }
        [Authorize(Roles = "Recruiter")]
        [HttpPut("[action]")]

        public async Task<IActionResult> RejectApplication(Guid applicationId)
        {
            var infoToken = await _permissionService.GetInfoToken();
            var recId = infoToken.UserClaimId;
            await _recruiterService.RejectApplication(recId, applicationId);
            var result = await _mailerService.SendEmailReject(applicationId);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]")]

        public async Task<IActionResult> RejectOtherApplication(Guid recruiterId, Guid applicationId)
        {
            await _recruiterService.RejectApplication(recruiterId, applicationId);
            var result = await _mailerService.SendEmailReject(applicationId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(Guid recruiterId)
        {
            var result = await _recruiterService.Delete(recruiterId);
            return Ok(result);
        }
        
    }
}
