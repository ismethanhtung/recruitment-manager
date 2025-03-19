using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IUserRoleService
    {
        Task<UserRolesViewModel<object>> GetPermByUserId(Guid userId);
    }
}
