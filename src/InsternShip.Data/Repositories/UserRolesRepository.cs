using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InsternShip.Data.Repositories
{
    public class UserRolesRepository: Repository<IdentityUserRole<Guid>>, IUserRolesRepository
    {
        public UserRolesRepository(RecruitmentDB context) : base(context) {}
        public async Task<UserRolesViewModel<object>> GetPermByUserId(object currentUser, string role)
        {
            try
            {
                var user = new UserRolesViewModel<object>
                {
                    CurrentUserClaim = currentUser,
                    Role = role
                };
                return await Task.FromResult(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
