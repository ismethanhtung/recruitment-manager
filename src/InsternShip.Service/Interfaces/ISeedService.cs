using InsternShip.Data.Model;
using Microsoft.AspNetCore.Identity;

namespace InsternShip.Service.Interfaces
{
    public interface ISeedService
    {
        Task<bool> CreateRole();
        Task<bool> AddUserToRole(string email, string role);
        Task<IdentityResult> CreateUser(CreateUserModel request);

    }
}
