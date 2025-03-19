using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IQuestionBankService
    {
        Task<bool> Create(CreateQuestionBankModel request);
        Task<bool> Delete(string request);
        Task<bool> Update(QuestionBankModel request);

        Task<IEnumerable<QuestionBankViewModel>> GetAll(string? request);
        Task<QuestionBankDetailModel?> GetAll(Guid request);
        Task<QuestionBankViewModel?> GetById(string request);
        Task<QuestionBankModel?> Get(string test_id, string question_id);
    }
}
