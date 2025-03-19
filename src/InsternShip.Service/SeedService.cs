using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InsternShip.Service
{
    public class SeedService: ISeedService
    {
        private readonly ISeedRepository _seedRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IInterviewerRepository _interviewerRepository;
        private readonly IRecruiterRepository _recruiterRepository;
        public SeedService(ISeedRepository seedRepository, ICandidateRepository candidateRepository,
            IInterviewerRepository interviewerRepository, IRecruiterRepository recruiterRepository)
        {
            _seedRepository = seedRepository;
            _candidateRepository = candidateRepository;
            _interviewerRepository = interviewerRepository;
            _recruiterRepository = recruiterRepository;
        }

        public async Task<bool> CreateRole()
        {
            return await _seedRepository.CreateRole();
        }
        public async Task<IdentityResult> CreateUser(CreateUserModel request)
        {
            return await _seedRepository.CreateUser(request);
        }
        public async Task<bool> AddUserToRole(string email, string role)
        {
            var userId =  await _seedRepository.AddUserToRole(email, role);
            if (role.ToUpper().Equals("CANDIDATE"))
            {
                var can = new CreateCandidateModel { UserId = userId };
                return await _candidateRepository.Create(can);
            }
            else if (role.ToUpper().Equals("INTERVIEWER"))
            {
                var inter = new CreateInterviewerModel { UserId = userId };
                return await _interviewerRepository.Create(inter);
            }
            else if(role.ToUpper().Equals("RECRUITER"))
            {
                var rec = new CreateRecruiterModel { UserId = userId };
                return await _recruiterRepository.Create(rec);
            }
            else { return await Task.FromResult(true); }
                
        }
    }
}
