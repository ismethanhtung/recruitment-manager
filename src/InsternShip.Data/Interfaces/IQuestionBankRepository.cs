using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IQuestionBankRepository
    {
        Task<bool> Create(CreateQuestionBankModel request);
        Task<bool> Delete(string request);
        Task<bool> Update(QuestionBankModel request);
        Task<QuestionBankViewModel> GetById(string request);
        Task<IEnumerable<QuestionBankViewModel>> GetAll(string? request);
        Task<QuestionBankDetailModel?> GetAll(Guid request);
        Task<QuestionBankModel?> Get(string test_id,string question_id);
        Task<int> GetScore(Guid testId);
        Task<IEnumerable<TestViewModel>> GetTest(Guid questionId);
    }
}
