using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IBlackListRepository
    {
        Task<BlackListEntriesViewModel> GetAll(string? search, int page, int limit, bool isOn);
        Task<BlackListViewModel> GetById(Guid blacklistId);
        Task<BlackListViewModel> GetByUserId(Guid userId);
        Task<bool> Check(Guid userId);
        Task<bool> Create(CreateBlackListModel request);
        Task<bool> Update(Guid userId, BlackListUpdateModel request);
        Task<bool> Delete(Guid userId);
        Task<bool> Restore(Guid userId);
    }
}   
