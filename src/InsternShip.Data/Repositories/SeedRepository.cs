using InsternShip.Api.Claims.System;
using InsternShip.Common;
using InsternShip.Common.Exceptions;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace InsternShip.Data.Repositories
{
    public class SeedRepository: ISeedRepository
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly RoleManager<Roles> _roleManager;

        public SeedRepository(UserManager<UserAccount> userManager, RoleManager<Roles> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRole()
        {
            List<string> listRoles = new List<string> {
                "Admin", "Recruiter", "Interviewer", "Candidate"
            };
            try
            {
                foreach (string role in listRoles) {
                    var isExist = await _roleManager.FindByNameAsync(role);
                    if(isExist != null) { throw new KeyNotFoundException(ExceptionMessages.RoleCreated); }
                    var currentRole = new Roles() {
                        Id = new Guid(),
                        Name = role
                    };
                    var result = await _roleManager.CreateAsync(currentRole);
                    if (!result.Succeeded) { return await Task.FromResult(true); }
                    await AddClaimsToRole(currentRole);
                }
                return await Task.FromResult(true);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public async Task<IdentityResult> CreateUser(CreateUserModel request)
        {
            if (!(request.Role.ToUpper().Equals("ADMIN") || request.Role.ToUpper().Equals("CANDIDATE")
                || request.Role.ToUpper().Equals("RECRUITER") || request.Role.ToUpper().Equals("INTERVIEWER")))
                throw new KeyNotFoundException(ExceptionMessages.FillOtherRole);
            try
            {
                var user = new UserAccount
                {
                    UserInfo = new UserInfo
                    {
                        Avatar = request.Avatar,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Gender = request.Gender,
                        DateOfBirth = request.DateOfBirth,
                        Location = request.Location
                    },
                    RegistrationDate = DateTime.Now,
                    Email = request.Email,
                    UserName = request.Email,
//                    EmailConfirmed = true
                };
                
                return await _userManager.CreateAsync(user, request.Password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> AddUserToRole(string email, string role)
        {
            if (!(role.ToUpper().Equals("ADMIN") || role.ToUpper().Equals("CANDIDATE")
                || role.ToUpper().Equals("RECRUITER") || role.ToUpper().Equals("INTERVIEWER")))
                throw new KeyNotFoundException(ExceptionMessages.FillOtherRole);
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new KeyNotFoundException(ExceptionMessages.UserNotFound);

            var roles = await _userManager.GetRolesAsync(user);

            // 1 user -> n role
            /*if (roles.Any(p => p.Equals(role, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateException(DuplicateMessage.DuplicateUserRole);
            }*/

            // 1 user -> 1 role
            if(roles.Count > 0) { throw new DuplicateException(DuplicateMessage.DuplicateUserRole); }
            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded) { return user.Id; }
            return Guid.Empty;
        }

        private async Task AddClaimsToRole(Roles role)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            List<string> listClaims =
                role.NormalizedName.Equals("INTERVIEWER") ? Authority.LIST_INTERVIEWER_CLAIMS :
                role.NormalizedName.Equals("CANDIDATE") ? Authority.LIST_CANDIDATE_CLAIMS :
                role.NormalizedName.Equals("RECRUITER") ? Authority.LIST_RECRUITER_CLAIMS :
                Authority.LIST_ADMIN_CLAIMS;

            foreach (string claim in listClaims)
            {
                if (false == claims.Any(p => p.Value.Equals(claim)))
                {
                    await _roleManager.AddClaimAsync(role, new Claim(ClaimTypes.AuthorizationDecision, claim));
                }
            }

        }
    }
}
