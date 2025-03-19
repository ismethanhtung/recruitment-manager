using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IUserInfoService
    {
        Task<AllInfoUser> GetMyInfo(Guid userId, Guid currentUserId);
        Task<AllInfoUser> GetInfoByUserId(Guid userId);
        Task<bool> UpdateInfo(Guid id, UserInfoUpdateModel info);
        Task<ResultUpdateInfo> UpdateInfoByUserId(Guid userId, AllInfoUserUpdate infoUpdate);
    }
}

