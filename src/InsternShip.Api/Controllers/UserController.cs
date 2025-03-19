using InsternShip.Data.Entities;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.Api.Controllers
{
    public class UserController : BaseAPIController
    {
        private readonly IUserAccountService _userAccountService;
        private readonly ISeedService _seedService;

        private readonly IUserInfoService _userInfoService;
        private readonly UserManager<UserAccount> _userManager;
        private readonly IMailerService _mailerService;
        private readonly IPermissionService _permissionService;
        private readonly IUserRoleService _roleService;

        public UserController(IUserAccountService userAccountService, UserManager<UserAccount> userManager,
                                        IMailerService mailerService, ISeedService seedService, IUserInfoService userInfoService,
                                        IPermissionService permissionService, IUserRoleService roleService)
        {
            _userAccountService = userAccountService;
            _seedService = seedService;
            _userManager = userManager;
            _mailerService = mailerService;
            _userInfoService = userInfoService;
            _permissionService = permissionService;
            _roleService = roleService; 
        }
        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAll(int page, int limit)
        {
            var userAccountList = await _userAccountService.GetAll(page,limit);
            return Ok(userAccountList);
        }
        
        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(Guid request)
        {

            var userAccountList = await _userAccountService.GetById(request);
            return Ok(userAccountList);
        }
        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            try
            {
                var result = await _userAccountService.ConfirmEmail(token, email);
                return Redirect("https://hcm23net03gr01-001-site1.htempurl.com/BodyContent/success.html");
            }
            catch {
                return Redirect("https://hcm23net03gr01-001-site1.htempurl.com/BodyContent/failed.html");
            }
            
            //return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> ResetPassword(string token, string email, string pass)
        {
            try
            {
                var result = await _userAccountService.ResetPassword(token, email, pass);
                //return Ok(result);
                return Redirect("https://hcm23net03gr01-001-site1.htempurl.com/BodyContent/success.html");
            }
            catch
            {
                return Redirect("https://hcm23net03gr01-001-site1.htempurl.com/BodyContent/failed.html");
            }

        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMyInfo()
        {
            var infoToken = await _permissionService.GetInfoToken();
            var userId = infoToken.UserId;
            var currentUserId = infoToken.UserClaimId;
            var userInfoList = await _userInfoService.GetMyInfo(userId, currentUserId);
            return Ok(userInfoList);
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetInfoByUserId(Guid userId)
        {
            var userInfoList = await _userInfoService.GetInfoByUserId(userId);
            return Ok(userInfoList);
        }
        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPermByUserId(Guid userId)
        {
            var data = await _roleService.GetPermByUserId(userId);
            return Ok(data);
        }
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(SignUpModel request)
        {
            var user = await _userAccountService.SignUp(request);
            var mail = await _userAccountService.GetConfirmMail(request.Email);
            await _mailerService.SendEmailSignUp(mail.Email, mail.Link);
            var candidateRoleDefault = await _seedService.AddUserToRole(request.Email, "Candidate");
            var result = new { user, candidateRoleDefault };
            return Ok(result);
        }
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(SignInModel request)
        {
            var result = await _userAccountService.SignIn(request);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> ForgotPassword(ResetPasswordModel request)
        {
            var mail = await _userAccountService.ForgotPassword(request);
            return Ok(await _mailerService.SendEmailReset(mail.Email, mail.Link));
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> ResendConfirmationMail(string email)
        {
            var mail = await _userAccountService.GetConfirmMail(email);
            var result = await _mailerService.SendEmailSignUp(mail.Email, mail.Link);
            return Ok(result);
        }
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(CreateUserModel request)
        {
            var user = await _seedService.CreateUser(request);
            if (user.Succeeded)
            {
                var userRole = await _seedService.AddUserToRole(request.Email, request.Role);
                await _userManager.FindByEmailAsync(request.Email);
                var mail = await _userAccountService.GetConfirmMail(request.Email);
                await _mailerService.SendEmailSignUp(mail.Email, mail.Link);
                return Ok(new { user, userRole });
            }
            return Ok(user);
        }
        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMyInfo(UserInfoUpdateModel userInfoUpdate)
        {
            var infoToken = await _permissionService.GetInfoToken();
            var userId = infoToken.UserId;
            var user = await _userAccountService.GetById(userId);
            var updateInfo = await _userInfoService.UpdateInfo(user.UserId, userInfoUpdate);
            return Ok(updateInfo);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var infoToken = await _permissionService.GetInfoToken();
            var userId = infoToken.UserId;
            await _userAccountService.GetById(userId);
            var updateInfo = await _userAccountService.ChangePassword(userId, currentPassword, newPassword);
            return Ok(updateInfo);

        
        }
        [HttpPut("[action]/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInfo(Guid userId, [FromBody] AllInfoUserUpdate infoUpdate)
        {
            var result = await _userInfoService.UpdateInfoByUserId(userId, infoUpdate);
            return Ok(result);
        }
    }
}

