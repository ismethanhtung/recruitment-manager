using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    public class QuestionController :BaseAPIController
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        [Authorize(Roles = "Admin,Interviewer,Recruiter")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(string? search, string? tag, int? level,int page, int limit)
        {
            var questionList = await _questionService.GetAll(search,tag,level,page,limit);
            return Ok(questionList);
        }
        [Authorize(Roles = "Admin,Interviewer,Recruiter")]
        [HttpGet("[action]/{questionId}")]
        public async Task<IActionResult> GetById(Guid questionId)
        {
            var question = await _questionService.GetById(questionId);
            return Ok(question);
        }
        [Authorize(Roles = "Admin,Interviewer,Recruiter")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateQuestionModel request)
        {
            var createReq = await _questionService.Create(request);
            return Ok(createReq);
        }
        
        [Authorize(Roles = "Admin,Interviewer,Recruiter")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(Guid questionId, QuestionUpdateModel request)
        {
            var updateReq = await _questionService.Update(questionId, request);
            return Ok(updateReq);
        }

        [Authorize(Roles = "Admin,Interviewer,Recruiter")]
        [HttpDelete("[action]/{questionId}")]
        public async Task<IActionResult> Delete(Guid questionId)
        {

            var deleteReq = await _questionService.Delete(questionId);
            return Ok(deleteReq);
        }
    }
}
