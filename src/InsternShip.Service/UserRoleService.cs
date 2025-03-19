using InsternShip.Data.Interfaces;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class UserRoleService : IUserRoleService
    {
        
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IInterviewerRepository _interviewerRepository;
        private readonly IRecruiterRepository _recruiterRepository;

        public UserRoleService(IUserRolesRepository userRolesRepository, IUserAccountRepository userAccountRepository,
            ICandidateRepository candidateRepository, IInterviewerRepository interviewerRepository,
            IRecruiterRepository recruiterRepository)
        {
            _userRolesRepository = userRolesRepository;
            _userAccountRepository = userAccountRepository;
            _candidateRepository = candidateRepository;
            _interviewerRepository = interviewerRepository;
            _recruiterRepository = recruiterRepository;
        }

        public async Task<UserRolesViewModel<object>> GetPermByUserId(Guid userId)
        {
            _ = await _userAccountRepository.GetById(userId);
            var can = await _candidateRepository.GetByUserId(userId);
            var inter = await _interviewerRepository.GetByUserId(userId);
            var rec = await _recruiterRepository.GetByUserId(userId);
            var role = await _userAccountRepository.GetRoleByUserId(userId);
            object currentUser;
            if (can != null) { currentUser = can; }
            else if (inter != null) { currentUser = inter; }
            else if (rec != null) { currentUser = rec; }
            else { currentUser = userId; }
            return await _userRolesRepository.GetPermByUserId(currentUser, role);
        }
    }
}
