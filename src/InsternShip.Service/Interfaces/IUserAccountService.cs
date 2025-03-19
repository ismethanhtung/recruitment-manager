using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InsternShip.Service.Interfaces
{
    public interface IUserAccountService
    {
        Task<UserAccountViewModel> GetById(Guid userId);
        Task<SignInViewModel> SignIn(SignInModel request);
        Task<IdentityResult> SignUp(SignUpModel request);
        Task<UserAccountListViewModel> GetAll(int page, int limit);
        Task<IdentityResult> ChangePassword(Guid userId, string oldPassword, string newPassword);
        Task<LinkedMailModel> ForgotPassword(ResetPasswordModel request);
        Task<LinkedMailModel> GetConfirmMail(string email);
        Task<bool> ConfirmEmail(string token, string email);
        Task<bool> ResetPassword(string token, string email, string pass);
    }
}
