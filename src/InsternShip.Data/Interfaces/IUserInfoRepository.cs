using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IUserInfoRepository
    {
        Task<UserInfoViewModel> GetById(Guid infoId);
        Task<bool> UpdateInfo(Guid userId, UserInfoUpdateModel newInfo);
        /*Task<bool> Create(BlackListCreateModel request);*/
        /*Task<bool> UpdateBlackList(int BlackListId, BlackListModel request);
        Task<bool> DeleteBlackList(int BlackListId);*/
    }
}
