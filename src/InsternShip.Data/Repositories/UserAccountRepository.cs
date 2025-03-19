using AutoMapper;
using InsternShip.Common;
using InsternShip.Common.Exceptions;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;





namespace InsternShip.Data.Repositories
{
    public class UserAccountRepository : Repository<UserAccount>, IUserAccountRepository
    {
        //private readonly IUnitOfWork _uow;
        private readonly UserManager<UserAccount> userManager;
        private readonly SignInManager<UserAccount> signInManager;
        private readonly IUrlHelper urlHelper;
        private readonly IConfiguration configuration;
        private readonly IMapper _mapper;
        private readonly RoleManager<Roles> _roleManager;
        private readonly IBlackListRepository blackList;
        private IUnitOfWork unitOfWork;
        private UserManager<UserAccount> userManager1;
        private IMailerRepository mailer;
        private SignInManager<UserAccount> signInManager1;
        private IMapper mapper;
        private RoleManager<Roles> roleManager;
        private BlackListRepository blackListRepository;

        public UserAccountRepository(RecruitmentDB context, UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager, IMapper mapper, IConfiguration configuration, IUrlHelper urlHelper, RoleManager<Roles> roleManager, IBlackListRepository blackList
            ) : base(context)
        {
            //_uow = uow;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.urlHelper = urlHelper;
            this.blackList = blackList;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public UserAccountRepository(RecruitmentDB dbContext, IUnitOfWork unitOfWork, UserManager<UserAccount> userManager1, IMailerRepository mailer, SignInManager<UserAccount> signInManager1, IMapper mapper, IConfiguration configuration, IUrlHelper urlHelper, RoleManager<Roles> roleManager, BlackListRepository blackListRepository) : base(dbContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager1 = userManager1;
            this.mailer = mailer;
            this.signInManager1 = signInManager1;
            this.mapper = mapper;
            this.configuration = configuration;
            this.urlHelper = urlHelper;
            this.roleManager = roleManager;
            this.blackListRepository = blackListRepository;
        }

        public async Task<UserAccountListViewModel> GetAll(int page, int limit)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                var listData = new List<UserAccountViewModel>();
                var query = Entities
                                .Include(user => user.UserInfo);

                var data = await query.OrderByDescending(user => user.RegistrationDate).Where(user => user.IsDeleted == false && user.EmailConfirmed == true).ToListAsync();
                var totalCount = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var roles = await userManager.GetRolesAsync(item);
                    var role = roles.Count > 0 ? roles[0] : "";
                    var user = _mapper.Map<UserAccountViewModel>(item);
                    user.Role = role;
                    listData.Add(user);
                }
                return new UserAccountListViewModel
                {
                    TotalCount = totalCount,
                    AccountList = listData
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserAccountViewModel> GetById(Guid userId)
        {
            var query = Entities.Include(user => user.UserInfo);
            var data = query.FirstOrDefault(user => user.Id == userId)
                ?? throw new KeyNotFoundException(ExceptionMessages.UserNotFound);
            var roles = await userManager.GetRolesAsync(data);
            try
            {
                var role = roles.Count > 0 ? roles[0] : "";
                var user = _mapper.Map<UserAccountViewModel>(data);
                user.Role = role;
                return await Task.FromResult(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IdentityResult> SignUp(SignUpModel request)
        {
            if (request.ConfirmPassword != request.Password) throw new AppException(ExceptionMessages.PasswordMismatch);
            var userTest = await userManager.FindByEmailAsync(request.Email);
            if (userTest != null)
            {
                throw new DuplicateException("Account existed.");
            }
            var passwordValidator = new PasswordValidator<UserAccount>();
            var passtest = await passwordValidator.ValidateAsync(userManager, null, request.Password);
            List<string> passwordErrors = new();
            if (!passtest.Succeeded)
            {
                foreach (var error in passtest.Errors)
                {
                    string desc = error.Description;
                    if (desc.Contains("have at least"))
                        desc = desc[29..];
                    else desc = desc[27..];
                    desc = desc[..^1];
                    passwordErrors.Add(desc);
                }
                string errors = String.Join(", ", passwordErrors.ToArray());
                throw new PasswordException("Password must have at least " + errors);
                //password must be (6 characters long) 
            }
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
                UserName = request.Email
            };
            var res = await userManager.CreateAsync(user, request.Password);
            return res;
        }
        public async Task<LinkedMailModel> GetConfirmMail(string email)
        {
            var userTest = await userManager.FindByEmailAsync(email) ?? throw new KeyNotFoundException(ExceptionMessages.UserNotFound);
            if (userTest.EmailConfirmed == true) throw new DuplicateException(DuplicateMessage.AccountConfirmed);
            try
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(userTest);
                var confirmationLink = urlHelper.Action("ConfirmEmail", new { token, email });
                return await Task.FromResult(new LinkedMailModel { Email = email, Link = confirmationLink });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<SignInViewModel> SignIn(SignInModel request, Guid userClaimId)
        {
            try
            {
                var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
                var tokenJWT = "";
                var user = await userManager.FindByEmailAsync(request.Email);
                if (!result.Succeeded)
                {
                    if (user != null && !user.EmailConfirmed) throw new KeyNotFoundException(ExceptionMessages.AccountHasNotBeenConfirmed);
                    throw new KeyNotFoundException(ExceptionMessages.WrongEmailOrPassword);

                }

                
                var isBanned = await blackList.Check(user.Id);
                if (isBanned) throw new KeyNotFoundException(ExceptionMessages.AccountHasBeenBanned);
                if (user.IsDeleted) throw new KeyNotFoundException(ExceptionMessages.AccountHasBeenDeactivated);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserClaimId", userClaimId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var roles = await userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    var ro = await _roleManager.FindByNameAsync(role);
                    var claims = await _roleManager.GetClaimsAsync(ro);
                    authClaims.Add(new Claim(ClaimTypes.Role, role));

                    foreach (var claim in claims)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, claim.Value));

                    }
                }
                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(30),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                );

                tokenJWT = new JwtSecurityTokenHandler().WriteToken(token);
                return new SignInViewModel { Token = tokenJWT, Roles = roles[0] };
                //return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Guid GetUserIdByEmail(string? email)
        {
            var user = Entities.FirstOrDefault(user => user.Email == email) ?? throw new KeyNotFoundException(ExceptionMessages.UserNotFound);
            return user.Id;
        }
        public async Task<IdentityResult> ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result;
        }
        public async Task<bool> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return await Task.FromResult(true);
                }
            }
            throw new KeyNotFoundException(ExceptionMessages.EmailDoesNotExist);
        }
        public async Task<bool> ResetPassword(string token, string email,string pass)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await userManager.ResetPasswordAsync(user, token, pass);
                if (result.Succeeded)
                {
                    return await Task.FromResult(true);
                }
            }
            throw new KeyNotFoundException(ExceptionMessages.EmailDoesNotExist);
        }
        public async Task<string> GetRoleByUserId(Guid? userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString()) ??
                throw new KeyNotFoundException(ExceptionMessages.UserNotFound);
            var roles = await userManager.GetRolesAsync(user);
            return roles[0];
        }
        public async Task<string> GetEmailByUserId(Guid? userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString()) ??
                throw new KeyNotFoundException(ExceptionMessages.UserNotFound);
            var email = await userManager.GetEmailAsync(user);
            return email;
        }
        public async Task<LinkedMailModel> ForgetPassword(ResetPasswordModel request)
        {
            if (request.ConfirmPassword != request.NewPassword) throw new AppException(ExceptionMessages.PasswordMismatch);
            var user = await userManager.FindByEmailAsync(request.Email);
            var passwordValidator = new PasswordValidator<UserAccount>();
            var passtest = await passwordValidator.ValidateAsync(userManager, null, request.NewPassword);
            List<string> passwordErrors = new List<string>();
            if (user != null)
            {
                if (passtest.Succeeded)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    
                    var resetLink = urlHelper.Action("ResetPassword", new { token = token, email = request.Email, pass = request.NewPassword });
                    LinkedMailModel result = new()
                    {
                        Email = request.Email,
                        Link = resetLink
                    };
                    return await Task.FromResult(result);
                }
                else
                {
                    foreach (var error in passtest.Errors)
                    {
                        string desc = error.Description;
                        if (desc.Contains("have at least"))
                            desc = desc[29..];
                        else desc = desc[27..];
                        desc = desc[..^1];
                        passwordErrors.Add(desc);
                    }
                    string errors = String.Join(", ", passwordErrors.ToArray());
                    throw new PasswordException("Password must have at least " + errors);
                }
            }
            else
            {
                throw new KeyNotFoundException(ExceptionMessages.EmailDoesNotExist);
            }
        }
    }
}
