using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InsternShip.Service
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IInterviewerRepository _interviewerRepository;
        private readonly IRecruiterRepository _recruiterRepository;

        public UserAccountService(IUserAccountRepository userAccountRepository,
            IUserInfoRepository userInfoRepository,
            ICandidateRepository candidateRepository, IInterviewerRepository interviewerRepository,
            IRecruiterRepository recruiterRepository)
        {
            _userAccountRepository = userAccountRepository;
            _userInfoRepository = userInfoRepository;
            _candidateRepository = candidateRepository;
            _interviewerRepository = interviewerRepository;
            _recruiterRepository = recruiterRepository;

        }
        public async Task<UserAccountListViewModel> GetAll(int page, int limit)
        {
            var res = await _userAccountRepository.GetAll(page, limit);
            foreach (var item in res.AccountList)
            {
                item.UserInfo = await _userInfoRepository.GetById(item.UserId);
            }
            return res;
        }
        public async Task<UserAccountViewModel> GetById(Guid userId)
        {
            var userAcc = await _userAccountRepository.GetById(userId);
            userAcc.UserInfo = await _userInfoRepository.GetById(userId);
            return userAcc;
        }
        public async Task<IdentityResult> SignUp(SignUpModel request)
        {
            var res = await _userAccountRepository.SignUp(request);
            return res;
        }
        public async Task<SignInViewModel> SignIn(SignInModel request)
        {
            var userId = _userAccountRepository.GetUserIdByEmail(request.Email);
            var can = await _candidateRepository.GetByUserId(userId);
            var inter = await _interviewerRepository.GetByUserId(userId);
            var rec = await _recruiterRepository.GetByUserId(userId);
            Guid userClaimId = Guid.Empty;
            if (can != null) { userClaimId = can.CandidateId; }
            else if (inter != null) { userClaimId = inter.InterviewerId; }
            else if (rec != null) { userClaimId = rec.RecruiterId; }
            return await _userAccountRepository.SignIn(request, userClaimId);
        }

        public async Task<IdentityResult> ChangePassword(Guid userId, string currentPassword, string newPassword)
        {
            return await _userAccountRepository.ChangePassword(userId, currentPassword, newPassword);
        }
        public async Task<LinkedMailModel> ForgotPassword(ResetPasswordModel request)
        {
            
            return await _userAccountRepository.ForgetPassword(request);
        }
        public async Task<LinkedMailModel> GetConfirmMail(string email)
        {

            return await _userAccountRepository.GetConfirmMail(email);
        }
        public async Task<bool> ConfirmEmail(string token, string email)
        {

            return await _userAccountRepository.ConfirmEmail(token, email);
        }
        public async Task<bool> ResetPassword(string token, string email, string pass)
        {

            return await _userAccountRepository.ResetPassword(token, email, pass);
        }
    }
}
