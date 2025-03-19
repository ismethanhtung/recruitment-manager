using InsternShip.Data.Interfaces;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class InterviewSessionService: IInterviewSessionService
    {
        private readonly IInterviewSessionRepository _interviewSessionRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IRecruiterJobPostRepository _recruiterJobPostRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICVRepository _cvRepository;
        private readonly IUserInfoService _userInfoService;
        private readonly IInterviewerRepository _interviewerRepository;
        private readonly ITestRepository _testRepository;
        public InterviewSessionService(IInterviewSessionRepository interviewSessionRepository, 
            IInterviewRepository interviewRepository, IApplicationRepository applicationRepository,
            IRecruiterJobPostRepository recruiterJobPostRepository, ICandidateRepository candidateRepository,
            ICVRepository cvRepository, IUserInfoService userInfoService, 
            IInterviewerRepository interviewerRepository, ITestRepository testRepository) 
        {
            _interviewSessionRepository = interviewSessionRepository;
            _interviewRepository = interviewRepository;
            _applicationRepository = applicationRepository;
            _recruiterJobPostRepository = recruiterJobPostRepository;
            _candidateRepository = candidateRepository;
            _cvRepository = cvRepository;
            _userInfoService = userInfoService;
            _interviewerRepository = interviewerRepository;
            _testRepository = testRepository;
        }
        public async Task<InterviewSessionDetailViewModel> GetDetail(Guid interviewId)
        {
            // get interview info
            var interview = await _interviewRepository.GetById(interviewId);
            var application = await _applicationRepository.GetById(interview.ApplicationId);
            var jobPost = await _recruiterJobPostRepository.GetById(application.JobPostId);
            var appliBaseInfo = new ApplicationBaseViewModel
            {
                ApplicationId = application.ApplicationId,
                CandidateId = application.CandidateId,
                JobPostId = application.JobPostId,
                ApplyDate = application.ApplyDate
            };
            var applicationInfo = new ApplicationInfoViewModel
            {
                CurrentAppliInfo = appliBaseInfo,
                JobPostInfo = jobPost
            };
            var interviewInfo = new InterviewInfoViewModel
            {
                CurrentInterviewInfo = interview,
                ApplicationInfo = applicationInfo
            };

            // get candidate info
            var cv = await _cvRepository.GetByCanId(application.CandidateId);
            var urlCV = cv != null ? cv.UrlFile : "";
            var candidate = await _candidateRepository.GetById(application.CandidateId, urlCV);
            var candidateFullInfo = await _userInfoService.GetMyInfo(candidate.UserId, candidate.CandidateId);

            // get list interview session info
            var listInterviewSession = await _interviewSessionRepository.GetAllSessionOfInterview(interviewId);
            var listInfoSessions = new List<InterviewSessionBaseListViewModel>();
            foreach (var session in listInterviewSession)
            {
                var currentSession = await _interviewSessionRepository.GetById(session.InterviewSessionId);
                var interviewer = await _interviewerRepository.GetById(session.InterviewerId);
                var interviewerFullInfo = await _userInfoService.GetMyInfo(interviewer.UserId, interviewer.InterviewerId);
                var test = await _testRepository.GetById(session.TestId);
                var sessionBaseInfo = new InterviewSessionBaseViewModel
                {
                    SessionId = currentSession.SessionId,
                    InterviewId = currentSession.InterviewId,
                    InterviewerId = currentSession.InterviewerId,
                    TestId = currentSession.TestId,
                    GivenScore = currentSession.GivenScore,
                    Note = currentSession.Note
                };
                var InfoSessions = new InterviewSessionBaseListViewModel
                {
                    CurrentSessionInfo = sessionBaseInfo,
                    InterviewerInfo = interviewerFullInfo,
                    TestInfo = test
                };
                listInfoSessions.Add(InfoSessions);
            }
            return await _interviewSessionRepository.GetDetail(interviewInfo, candidateFullInfo, listInfoSessions);
        }
    }
}
