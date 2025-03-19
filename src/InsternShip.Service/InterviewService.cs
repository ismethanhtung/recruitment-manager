using InsternShip.Common.Exceptions;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IInterviewSessionRepository _interviewSessionRepository;
        private readonly IInterviewerRepository _interviewerRepository;
        private readonly ITestRepository _testRepository;
        private readonly IApplicationStatusUpdateRepository _applicationStatusUpdateRepository;
        private readonly IApplicationStatusRepository _applicationStatusRepository;
        public InterviewService(IInterviewRepository interviewRepository, 
            IApplicationRepository applicationRepository,
            IInterviewSessionRepository interviewSessionRepository,
            IInterviewerRepository interviewerRepository,
            ITestRepository testRepository,
            IApplicationStatusUpdateRepository applicationStatusUpdateRepository,
            IApplicationStatusRepository applicationStatusRepository
            )
        {
            _interviewRepository = interviewRepository;
            _applicationRepository = applicationRepository;
            _interviewSessionRepository = interviewSessionRepository;
            _interviewerRepository = interviewerRepository;
            _testRepository = testRepository;
            _applicationStatusUpdateRepository = applicationStatusUpdateRepository;
            _applicationStatusRepository = applicationStatusRepository;


        }

        public async Task<bool> Delete(Guid request)
        {
            return await _interviewRepository.Delete(request);
        }
        public async Task<bool> Restore(Guid request)
        {
            return await _interviewRepository.Restore(request);
        }

        public async Task<InterviewListViewModel> GetAll(int page, int limit, bool deleted)
        {
            return await _interviewRepository.GetAll(page, limit,deleted);
        }
        public async Task<int[]> GetAllInWeekByRecId(Guid recId)
        {
            return await _interviewRepository.GetAllInWeekByRecId(recId);
        }

        public async Task<int[]> GetAllInterviewInWeek()
        {
            return await _interviewRepository.GetAllInterviewInWeek();
        }
        public async Task<InterviewViewModel> GetById(Guid request)
        {
            return await _interviewRepository.GetById(request);
        }

        public async Task<MyInterviewListViewModel> GetByInterviewerId(Guid interviewerId, int page, int limit)
        {
            var interviewerSessions = await _interviewSessionRepository.GetAllInterviewSession(interviewerId,page,limit);
            var myInterviewList = new List<MyInterviewViewModel>();
            foreach (var session in interviewerSessions.InterviewSessionList)
            {
                var interview = await _interviewRepository.GetById(session.InterviewId);
                var allSession = await _interviewSessionRepository.GetAllSessionOfInterview(interview.InterviewId);
                var myInt = new MyInterviewViewModel
                {
                    InterviewId = interview.InterviewId,
                    StartTime = interview.StartTime,
                    EndTime = interview.EndTime,
                    Location = interview.Location,
                    NumberOfInterviewer = allSession.Count(),
                };
                myInterviewList.Add(myInt);
            }
            return new MyInterviewListViewModel { InterviewList = myInterviewList, TotalCount = interviewerSessions.TotalCount};
        }

        public async Task<MyInterviewListViewModel> GetByCandidateId(Guid candidateId, int page, int limit)
        {
            var interviewerSessions = await _interviewSessionRepository.GetAllCandidateSession(candidateId, page, limit);
            var myInterviewList = new List<MyInterviewViewModel>();
            foreach (var session in interviewerSessions.InterviewSessionList)
            {
                var interview = await _interviewRepository.GetById(session.InterviewId);
                var allSession = await _interviewSessionRepository.GetAllSessionOfInterview(interview.InterviewId);
                var myInt = new MyInterviewViewModel
                {
                    InterviewId = interview.InterviewId,
                    StartTime = interview.StartTime,
                    EndTime = interview.EndTime,
                    Location = interview.Location,
                    NumberOfInterviewer = allSession.Count(),
                };
                myInterviewList.Add(myInt);
            }
            return new MyInterviewListViewModel { InterviewList = myInterviewList, TotalCount = interviewerSessions.TotalCount };
        }

        public async Task<bool> Create(CreateInterviewModel request)
        {
            var app = await _applicationRepository.GetById(request.ApplicationId);
            await _interviewRepository.Create(request);
            var desc = await _applicationStatusRepository.GetByDescription("Interviewing");
            var newStatus = new ApplicationStatusUpdateCreateModel()
            {
                ApplicationId = app.ApplicationId,
                StatusId = desc.ApplicationStatusId
            };
            return await _applicationStatusUpdateRepository.Create(newStatus);
        }
        public async Task<bool> CreateSession(Guid interviewId, Guid interviewerId)
        {
            await _interviewerRepository.GetById(interviewerId);
            var interview = await _interviewRepository.GetById(interviewId);
            var duplicate = await _interviewSessionRepository.CheckDuplicate(interviewerId, (DateTime)interview.StartTime, (DateTime)interview.EndTime);
            if (duplicate != null && duplicate.TotalCount != 0) { throw new DuplicateException("Interviewer already have an sessions in this time."); }
            var test = new CreateTestModel()
            {
                TotalScore = 0,
                StartTime = interview.StartTime,
                EndTime = interview.EndTime,
            };
            var testId = await _testRepository.CreateGUID(test);
            var csrq = new CreateInterviewSessionModel() { 
                InterviewerId = interviewerId,
                InterviewId = interviewId,
                TestId = testId
            };
            return await _interviewSessionRepository.Create(csrq);
        }

        public async Task<bool> Update(Guid interviewId, InterviewUpdateModel request)
        {
            return await _interviewRepository.Update(interviewId, request);
        }
        public async Task<InterviewDetailViewModel> GetDetail(Guid interviewId)
        {
            var interview = await _interviewRepository.GetById(interviewId);
            var listInterviewer = new List<InterviewerViewModel>();
            var listSessionOfInterview = await _interviewSessionRepository.GetAllSessionOfInterview(interviewId);
            foreach(var item in listSessionOfInterview)
            {
                var interviewer = await _interviewerRepository.GetById(item.InterviewerId);
                listInterviewer.Add(interviewer);
            }
            var application = await _applicationRepository.GetById(interview.ApplicationId);
            var obj = new InterviewDetailViewModel
            {
                StartTime = interview.StartTime,
                EndTime = interview.EndTime,
                Location = interview.Location,
                CandidateId = application.CandidateId,
                CandidateName = application.CandidateName,
                NumberOfInterviewer = listInterviewer.Count,
                IsOnline = interview.IsOnline,
                Interviewers = listInterviewer,
            };
            return obj;
        }
    }
}
