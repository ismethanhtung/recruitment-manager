using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IQuestionRepository
    {
        Task<QuestionListViewModel> GetAll(string? search, string? tag, int? level, int page, int limit);
        Task<bool> Create(CreateQuestionModel request);
        Task<QuestionViewModel> CreateFromTest(CreateQuestionModel request);
        Task<bool> Delete(Guid questionId);
        Task<bool> Update(Guid questionId, QuestionUpdateModel request);
        Task<QuestionViewModel> GetById(Guid questionId);
    }
}
