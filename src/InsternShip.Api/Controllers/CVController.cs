using InsternShip.Data.Model;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    public class CVController : BaseAPIController
    {
        private readonly ICVService _cVService;
        private readonly IPermissionService _permissionService;
        private readonly ICandidateService _candidateService;
        public CVController(ICVService cVService, IPermissionService permissionService, ICandidateService candidateService)
        {
            _cVService = cVService;
            _permissionService = permissionService;
            _candidateService = candidateService;
        }

        [Authorize(Roles = "Candidate")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMy()
        {
            var inforToken = await _permissionService.GetInfoToken();
            var cv = await _candidateService.GetMyCV(inforToken.UserClaimId);
            return Ok(cv);
        }
        [HttpPost("[action]")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Upload([FromForm] CreateFileCVModel request)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var cv = new CreateCVModel
            {
                CurrentId = inforToken.UserClaimId,
                File = request.File
            };
            var result = await _cVService.Create(cv);
            return Ok(result);
        }
        [Authorize(Roles = "Candidate")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Create(CandidateUpdateModel request)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var reuslt = await _candidateService.UpdateMyCV(inforToken.UserClaimId, request);
            return Ok(reuslt);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(CandidateUpdateModel request)
        {
            var inforToken = await _permissionService.GetInfoToken();
            var reuslt = await _candidateService.UpdateMyCV(inforToken.UserClaimId, request);
            return Ok(reuslt);
        }
    }
}

