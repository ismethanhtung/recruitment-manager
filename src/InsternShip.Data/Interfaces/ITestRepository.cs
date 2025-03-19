using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface ITestRepository
    {
        //Task<IEnumerable<TestViewModel>> GetAll(int? page, int? limit);
        Task<TestListViewModel> GetAll(int page, int limit, bool deleted);
        Task<bool> Update(Guid testId, int point);
        Task<TestViewModel> GetById(Guid? testId);
        Task<bool> Create(CreateTestModel request);
        Task<Guid> CreateGUID(CreateTestModel request);
        Task<bool> Delete(Guid testId);
        Task<bool> Update(Guid testId, TestUpdateModel request);
        Task<bool> Restore(Guid testId);
    }
}
