using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlackListController : BaseAPIController
    {
        private readonly IBlackListService _blackListService;
        public BlackListController(IBlackListService blackListService)
        {
            _blackListService = blackListService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(string? search, int page, int limit)
        {
            var blackListList = await _blackListService.GetAll(search, page, limit, true);
            return Ok(blackListList);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDeleted(string? search, int page, int limit)
        {
            var blackListList = await _blackListService.GetAll(search, page, limit,false);
            return Ok(blackListList);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetById(Guid entryId)
        {
            var blackListList = await _blackListService.GetById(entryId);
            return Ok(blackListList);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateBlackListModel request)
        {
            var blackListList = await _blackListService.Create(request);
            return Ok(blackListList);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(Guid userId, BlackListUpdateModel request)
        {
            var blackListList = await _blackListService.Update(userId,request);
            return Ok(blackListList);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Restore(Guid userId)
        {
            var blackListList = await _blackListService.Restore(userId);
            return Ok(blackListList);
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var applicationList = await _blackListService.Delete(userId);
            return Ok(applicationList);

        }
    }
}

