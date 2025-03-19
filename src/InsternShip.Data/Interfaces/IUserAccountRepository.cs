using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InsternShip.Data.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<UserAccountListViewModel> GetAll(int page, int limit);
        Task<UserAccountViewModel> GetById(Guid userId);
        Task<IdentityResult> SignUp(SignUpModel request);
        Task<SignInViewModel> SignIn(SignInModel request, Guid userClaimId);
        Task<LinkedMailModel> GetConfirmMail(string email);
        Guid GetUserIdByEmail(string? email);
        // Task<IdentityResult> SendConfirmationMail(string email);
        Task<IdentityResult> ChangePassword(Guid userId, string oldPassword, string newPassword);
        Task<string> GetRoleByUserId(Guid? userId);
        Task<string> GetEmailByUserId(Guid? userId);
        Task<LinkedMailModel> ForgetPassword(ResetPasswordModel request);
        Task<bool> ResetPassword(string token, string email, string pass);
        Task<bool> ConfirmEmail(string token, string email);
        /*Task<bool> Create(BlackListCreateModel request);*/
        /*Task<bool> UpdateBlackList(int BlackListId, BlackListModel request);
        Task<bool> DeleteBlackList(int BlackListId);*/
        // Task<IdentityResult> ReSendConfirmationMail(string email);

    }
}
