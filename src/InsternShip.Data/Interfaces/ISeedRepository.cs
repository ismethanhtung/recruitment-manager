using InsternShip.Data.Model;
using Microsoft.AspNetCore.Identity;

namespace InsternShip.Data.Interfaces
{
    public interface ISeedRepository
    {
        Task<bool> CreateRole();
        Task<Guid> AddUserToRole(string email, string role);
        Task<IdentityResult> CreateUser(CreateUserModel request);

    }
}
