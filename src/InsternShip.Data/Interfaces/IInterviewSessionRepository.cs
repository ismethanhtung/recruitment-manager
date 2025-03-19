using InsternShip.Data.Entities;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IInterviewSessionRepository
    {
        Task<InterviewSessionListViewModel> GetAllInterviewSession(Guid interviewerId);
        Task<InterviewSessionListViewModel> GetAllCandidateSession(Guid candidateId);
        Task<InterviewSessionListViewModel> GetAllInterviewSession(Guid interviewerId, int page, int limit);
        Task<InterviewSessionListViewModel> GetAllCandidateSession(Guid candidateId, int page, int limit);
        Task<InterviewSessionViewModel> GetById(Guid interviewSessionId);
        Task<bool> Create(CreateInterviewSessionModel request);
        Task<bool> Delete(Guid interviewSessionId);
        Task<bool> Update(Guid interviewSessionId, UpdatedInterviewSessionModel request);
        Task<bool> SaveScore(Guid interviewerId, Guid interviewSessionId, UpdatedInterviewSessionModel request);
        /*        Task<bool> AddTest(Guid interviewId, Guid interviewerId, Guid testId);*/
        Task<Guid> GetTestId(Guid interviewSessionId);
        Task<IEnumerable<InterviewSession>> GetAllSessionOfInterview(Guid interviewId);

        Task<InterviewSessionListViewModel> CheckDuplicate(Guid interviewerId, DateTime startTime, DateTime endTime);
        Task<InterviewSessionDetailViewModel> GetDetail(InterviewInfoViewModel interviewInfo,
           AllInfoUser candidateFullInfo, List<InterviewSessionBaseListViewModel> listInfoSessions);
    }
}
