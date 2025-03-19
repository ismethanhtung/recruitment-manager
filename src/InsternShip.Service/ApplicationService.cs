using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IApplicationStatusUpdateRepository _applicationStatusUpdateRepository;
        private readonly IApplicationStatusRepository _applicationStatusRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IRecruiterJobPostRepository _recruiterJobPostRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IInterviewSessionRepository _interviewSessionRepository;
        private readonly ICVRepository _cvRepository;
        public ApplicationService(IApplicationRepository applicationRepository,
            IApplicationStatusUpdateRepository applicationStatusUpdateRepository,
            IApplicationStatusRepository applicationStatusRepository,
            ICandidateRepository candidateRepository,
            IRecruiterJobPostRepository recruiterJobPostRepository,
            ICVRepository cvRepository,
            IInterviewRepository interviewRepository,
            IInterviewSessionRepository interviewSessionRepository)
        {
            _applicationRepository = applicationRepository;
            _applicationStatusUpdateRepository = applicationStatusUpdateRepository;
            _applicationStatusRepository = applicationStatusRepository;
            _candidateRepository = candidateRepository;
            _recruiterJobPostRepository = recruiterJobPostRepository;
            _interviewRepository = interviewRepository;
            _interviewSessionRepository = interviewSessionRepository;
            _cvRepository = cvRepository;
        }

        public async Task<bool> Delete(Guid request)
        {
            return await _applicationRepository.Delete(request);
        }

        public async Task<bool> Restore(Guid request)
        {
            return await _applicationRepository.Restore(request);
        }
        public async Task<ApplicationListViewModel> GetAll(string? search, int page, int limit, string? statusDesc, Guid? candidateId, Guid? jobPostId, bool deleted)
        {
            return await _applicationRepository.GetAll(search, page, limit, statusDesc, candidateId, jobPostId, deleted);

        }

        public async Task<ApplicationViewModel> GetById(Guid request)
        {
            return await _applicationRepository.GetById(request);
        }

        public async Task<float> GetScoreByApplicationId(Guid request)
        {
            var interviewofApplication = await _interviewRepository.GetByApplicationId(request);
            if (interviewofApplication != null)
            {
                var interviewSession = await _interviewSessionRepository.GetAllSessionOfInterview(interviewofApplication.InterviewId);
                int numOfSession = interviewSession.Count();
                float totalScore = 0;
                if (numOfSession == 0) return 0;
                else
                {
                    foreach (var itv in interviewSession)
                    {
                        totalScore += itv.GivenScore;
                    }
                    return MathF.Round(totalScore / numOfSession, 2, MidpointRounding.AwayFromZero);
                }
            }
            else return 0;
        }

        public async Task<bool> Create(Guid canId, Guid jobPostId)
        {
            var cv = await _cvRepository.GetByCanId(canId);
            var urlCV = cv != null ? cv.UrlFile : "";
            await _candidateRepository.GetById(canId, urlCV);

            _ = await _recruiterJobPostRepository.GetById(jobPostId);
            ApplicationCreateModel request = new()
            {
                CandidateId = canId,
                JobPostId = jobPostId

            };
            var unprocessedStatus = await _applicationStatusRepository.GetByDescription("Unprocessed");
            var createdAppId = await _applicationRepository.CreateGuid(request);
            var newStatus = new ApplicationStatusUpdateCreateModel()
            {
                ApplicationId = createdAppId,
                StatusId = unprocessedStatus.ApplicationStatusId
            };
            
            return await _applicationStatusUpdateRepository.Create(newStatus);
        }
    }
}
