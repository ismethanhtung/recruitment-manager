using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionListViewModel> GetAll(string? search, string? tag, int? level, int page, int limit);
        Task<bool> Create(CreateQuestionModel request);
        Task<bool> Delete(Guid questionId);
        Task<bool> Update(Guid questionId, QuestionUpdateModel request);
        Task<QuestionViewModel> GetById(Guid questionId);
    }
}
