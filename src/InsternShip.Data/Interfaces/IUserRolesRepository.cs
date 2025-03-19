using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IUserRolesRepository
    {
        Task<UserRolesViewModel<object>> GetPermByUserId(object currentUser, string role);
    }
}
