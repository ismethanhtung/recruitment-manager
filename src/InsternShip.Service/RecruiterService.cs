using InsternShip.Common;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class RecruiterService: IRecruiterService
    {
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IRecruiterJobPostRepository _recruiterJobPostRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IApplicationStatusRepository _applicationStatusRepository;
        private readonly IApplicationStatusUpdateRepository _applicationStatusUpdateRepository;
        public RecruiterService(IRecruiterRepository recruiterRepository, IUserInfoRepository userInfoRepository, IUserAccountRepository userAccountRepository,
            IInterviewRepository interviewRepository, IApplicationRepository applicationRepository, IApplicationStatusRepository applicationStatusRepository, IApplicationStatusUpdateRepository applicationStatusUpdateRepository, IRecruiterJobPostRepository recruiterJobPostRepository)
        {
            _recruiterRepository = recruiterRepository;
            _userAccountRepository = userAccountRepository;
            _userInfoRepository = userInfoRepository;
            _interviewRepository = interviewRepository;
            _applicationRepository = applicationRepository;
            _applicationStatusRepository = applicationStatusRepository;
            _applicationStatusUpdateRepository = applicationStatusUpdateRepository;
            _recruiterJobPostRepository = recruiterJobPostRepository;
        }

        public async Task<RecruiterListViewModel> GetAll(string? search, int page, int limit, bool deleted)
        {
            return await _recruiterRepository.GetAll(search, page, limit, deleted);
        }

        public async Task<RecruiterViewModel> GetById(Guid recId)
        {
            return await _recruiterRepository.GetById(recId);
        }

        public async Task<bool> Create(CreateRecruiterModel request)
        {
            _ = await _userAccountRepository.GetById(request.UserId);
            return await _recruiterRepository.Create(request);
        }

        public async Task<bool> Update(Guid userId, RecruiterUserInfoUpdateModel request)
        {
            var user = await _userAccountRepository.GetById(userId) ?? throw new KeyNotFoundException(ExceptionMessages.UserNotFound);
            var rec = await _recruiterRepository.GetByUserId(userId) ?? throw new KeyNotFoundException(ExceptionMessages.RecruiterNotFound);

            var recruiterUpdate = request.RecruiterUpdate;
            var userInfoUpdate = request.UserInfoUpdate;
            _ = await _userInfoRepository.UpdateInfo(user.UserId, userInfoUpdate);
            return await _recruiterRepository.Update(rec.RecruiterId, recruiterUpdate);
        }

        public async Task<bool> Delete(Guid request)
        {
            return await _recruiterRepository.Delete(request);
        }
        public async Task<int> GetScheduleInterview(Guid recId)
        {
            return await _interviewRepository.GetScheduleInterviewByRecruiter(recId);
        }


        public async Task<bool> Restore(Guid request)
        {
            return await _recruiterRepository.Restore(request);
        }
        public async Task<bool> AcceptApplication(Guid recIc, Guid applicationId)
        {
            var app = await _applicationRepository.GetById(applicationId);
            var job = await _recruiterJobPostRepository.GetById(app.JobPostId);
            await _applicationStatusRepository.GetByDescription("Interviewed");
            var acpstatus = await _applicationStatusRepository.GetByDescription("Approved");
            return await _applicationStatusUpdateRepository.AcceptApplication(applicationId, job.RecruiterId, acpstatus.ApplicationStatusId);

        }
        public async Task<bool> RejectApplication(Guid recIc, Guid applicationId)
        {
            var app = await _applicationRepository.GetById(applicationId);
            var job = await _recruiterJobPostRepository.GetById(app.JobPostId);
            var acpstatus = await _applicationStatusRepository.GetByDescription("Rejected");
            return await _applicationStatusUpdateRepository.RejectApplication(applicationId, job.RecruiterId, acpstatus.ApplicationStatusId);

        }
    }
}
