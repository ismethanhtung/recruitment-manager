using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface ITestService
    {
        Task<TestListViewModel> GetAll(int page, int limit, bool deleted);
        Task<TestViewModel> GetById(Guid testId);
        Task<bool> Create(CreateTestModel request);
        Task<bool> Delete(Guid testId);
        Task<bool> Update(Guid testId, TestUpdateModel request);
        Task<bool> AddQuestion(Guid testId, Guid questionId);
        Task<bool> AddNewQuestion(Guid testId, CreateQuestionModel request);
        Task<bool> RemoveQuestion(Guid testId, Guid questionId);
        Task<TestQuestionListViewModel> GetDetailById(Guid testId);
        Task<bool> Restore(Guid testId);
    }
}
