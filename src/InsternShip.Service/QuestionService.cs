using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionBankRepository _questionBankRepository;
        private readonly ITestRepository _testRepository;
        public QuestionService(IQuestionRepository questionRepository,IQuestionBankRepository questionBankRepository, ITestRepository testRepository)
        {
            _questionRepository = questionRepository;
            _questionBankRepository = questionBankRepository;
            _testRepository = testRepository;
        }

        public async Task<bool> Create(CreateQuestionModel request)
        {
            return await _questionRepository.Create(request);
        }

        public async Task<bool> Delete(Guid questionId)
        {
            var question = await _questionRepository.GetById(questionId);
            var questionbank = await _questionBankRepository.GetTest(questionId);
            foreach (var test in questionbank)
            {
                if (test.TotalScore != null && question.MaxScore!= null)
                {
                    int maxpoint = (int)test.TotalScore - (int)question.MaxScore;
                    await _testRepository.Update(test.TestId, maxpoint);
                }
            }
            return await _questionRepository.Delete(questionId);
        }

        public async Task<QuestionListViewModel> GetAll(string? search, string? tag, int? level, int page, int limit)
        {
            return await _questionRepository.GetAll(search,tag,level,page,limit);
        }
        public async Task<QuestionViewModel> GetById(Guid questionId)
        {
            return await _questionRepository.GetById(questionId);
        }

        public async Task<bool> Update(Guid questionId, QuestionUpdateModel request)
        {
            var question = await _questionRepository.GetById(questionId);
            await _questionRepository.Update(questionId, request);
            
            var questionbank = await _questionBankRepository.GetTest(questionId);
            foreach (var test in questionbank)
            {
                if (test.TotalScore != null && question.MaxScore != null && request.MaxScore != null)
                {
                    int maxpoint = (int)test.TotalScore - (int)question.MaxScore + (int)request.MaxScore;
                    await _testRepository.Update(test.TestId, maxpoint);
                }
            }
            return true;
        }
    }
}
