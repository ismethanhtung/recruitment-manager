using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    public class TestController : BaseAPIController
    {
        private readonly ITestService _testService;
        public TestController(ITestService testService)
        {
            _testService = testService;
        }
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(int page, int limit)
        {
            var testList = await _testService.GetAll(page, limit,false);
            return Ok(testList);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDeleted(int page, int limit)
        {
            var testList = await _testService.GetAll(page, limit,true);
            return Ok(testList);
        }
/*        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpGet("[action]/{test_id}")]
        public async Task<IActionResult> GetById(Guid test_id)
        {
            var test = await _testService.GetById(test_id);
            return Ok(test);
        }*/
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpGet("[action]/{test_id}")]
        public async Task<IActionResult> GetById(Guid test_id)
        {
            var addquestionReq = await _testService.GetDetailById(test_id);
            return Ok(addquestionReq);
        }
        /*[Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateTestModel request)
        {
            var createReq = await _testService.Create(request);
            return Ok(createReq);
        }*/
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddQuestion(Guid test_id, Guid question_id)
        {
            var addquestionReq = await _testService.AddQuestion(test_id, question_id);
            return Ok(addquestionReq);
        }
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddNewQuestion(Guid test_id, CreateQuestionModel request)
        {
            var addquestionReq = await _testService.AddNewQuestion(test_id, request);
            return Ok(addquestionReq);
        }
/*        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpPut("[action]/{test_id}")]
        public async Task<IActionResult> Update(Guid test_id, TestUpdateModel request)
        {
            var updateReq = await _testService.Update(test_id, request);
            return Ok(updateReq);
        }*/
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpPut("[action]")]
        public async Task<IActionResult> RemoveQuestion(Guid test_id, Guid question_id)
        {
            var delquestionReq = await _testService.RemoveQuestion(test_id, question_id);
            return Ok(delquestionReq);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]/{test_id}")]
        public async Task<IActionResult> Restore(Guid test_id)
        {
            var updateReq = await _testService.Restore(test_id);
            return Ok(updateReq);
        }
        [Authorize(Roles = "Admin,Recruiter,Interviewer")]
        [HttpDelete("[action]/{test_id}")]
        public async Task<IActionResult> Delete(Guid test_id)
        {
            var deleteReq = await _testService.Delete(test_id);
            return Ok(deleteReq);
        }

    }
}
