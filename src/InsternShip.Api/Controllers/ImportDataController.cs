using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ImportDataController : BaseAPIController
    {
        private readonly IImportDataService _importDataService;
        public ImportDataController(IImportDataService importDataService)
        {
            _importDataService = importDataService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ImportDataUser([FromForm] CreateFileCVModel request)
        {
            var result = await _importDataService.ImportDataUser(request.File);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ImportDataJobPost(IFormFile file)
        {
            var result = await _importDataService.ImportDataJobPost(file);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ImportDataEventPost(IFormFile file)
        {
            var result = await _importDataService.ImportDataEventPost(file);
            return Ok(result);
        }
    }
}
