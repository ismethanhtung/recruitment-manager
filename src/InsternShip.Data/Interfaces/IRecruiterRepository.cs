using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IRecruiterRepository
    {
        Task<RecruiterListViewModel> GetAll(string? search, int page, int limit, bool deleted);
        Task<RecruiterViewModel> GetById(Guid? recId);
        Task<RecruiterViewModel> GetByUserId(Guid recId);
        Task<bool> Create(CreateRecruiterModel request);
        Task<bool> Update(Guid? recruiterId, RecruiterUpdateModel request);
        Task<bool> Delete(Guid request);
        Task<Guid?> GetIdByUserId(Guid userId);
        Task<bool> Restore(Guid request);
    }
}
