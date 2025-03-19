using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class TestService : ITestService
    { 
        private readonly ITestRepository _testRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionBankRepository _qbankRepository;
        public TestService(ITestRepository testRepository, IQuestionRepository questionRepository, IQuestionBankRepository qbankRepository)
        {
            _testRepository = testRepository;
            _questionRepository = questionRepository;
            _qbankRepository = qbankRepository;
        }

        public async Task<bool> Delete(Guid testId)
        {
            return await _testRepository.Delete(testId);
        }

        public async Task<TestListViewModel> GetAll(int page, int limit, bool deleted)
        {
            var res  =  await _testRepository.GetAll(page, limit, deleted);
            foreach (var item in res.TestList)
            {
                int score = await _qbankRepository.GetScore(item.TestId);
                item.TotalScore = score;
                await _testRepository.Update(item.TestId, score);
            }
            return res;

        }

        public async Task<bool> Create(CreateTestModel request)
        {
            return await _testRepository.Create(request);
        }

        public async Task<bool> Update(Guid testId, TestUpdateModel request)
        {
            return await _testRepository.Update(testId,request);
        }


        public async Task<bool> AddQuestion(Guid testId, Guid question_id)
        {
            var question = await _questionRepository.GetById(question_id);
            var test = await _testRepository.GetById(testId);
            CreateQuestionBankModel qbank = new()
            {
                QuestionId = question.QuestionId,
                TestId = test.TestId
            };
            await _qbankRepository.Create(qbank);
            test.TotalScore += question.MaxScore;
            return await _testRepository.Update(testId, (int)test.TotalScore);
        }
        public async Task<bool> AddNewQuestion(Guid testId, CreateQuestionModel request)
        {
            var question = await _questionRepository.CreateFromTest(request);
            var test = await _testRepository.GetById(testId);
            CreateQuestionBankModel qbank = new()
            {
                QuestionId = question.QuestionId,
                TestId = test.TestId
            };
            await _qbankRepository.Create(qbank);
            test.TotalScore += question.MaxScore;
            return await _testRepository.Update(testId, (int)test.TotalScore);
        }

        public async Task<TestViewModel> GetById(Guid testId)
        {
            var test = await _testRepository.GetById(testId);
            int score = await _qbankRepository.GetScore(test.TestId);
            test.TotalScore = score;
            await _testRepository.Update(test.TestId, score);
            return test;
        }

        public async Task<TestQuestionListViewModel> GetDetailById(Guid testId)
        {
            var test = await _testRepository.GetById(testId);
            var qlist = await _qbankRepository.GetAll(testId);
            var totalPoint = await _qbankRepository.GetScore(testId);
            var obj = new TestQuestionListViewModel
            {
                TestId = test.TestId,
                TotalScore = totalPoint,
                StartTime = test.StartTime,
                EndTime = test.EndTime,
                Questions = qlist.Questions
            };
            await _testRepository.Update(testId,totalPoint);
            return obj;
        }
        public async Task<bool> RemoveQuestion(Guid testId, Guid questionId)
        {
            var question = await _questionRepository.GetById(questionId);
            var test = await _testRepository.GetById(testId);
            var qbank = await _qbankRepository.Get(test.TestId.ToString(), question.QuestionId.ToString());
            test.TotalScore -= question.MaxScore;
            return await _qbankRepository.Delete(qbank.QuestionBankId.ToString());
        }

        public async Task<bool> Restore(Guid testId)
        {
            return await _testRepository.Restore(testId);
        }
    }
}
