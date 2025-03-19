using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IInterviewerService
    {
        Task<InterviewerListViewModel> GetAll(string? search, int page, int limit, bool deleted);
        Task<InterviewerViewModel> GetById(Guid interviewerId);
        Task<bool> Create(CreateInterviewerModel request);
        Task<bool> Delete(Guid interviewerId);
        Task<bool> Update(Guid userId, InterviewerUserInfoUpdateModel request);
        Task<bool> Restore(Guid interviewerId);
        Task<InterviewSessionListViewModel> GetAllSessions(Guid interviewerId,int page, int limit);
        Task<bool> SaveScore(Guid interviewerId, Guid interviewSession, UpdatedInterviewSessionModel request);
        Task<InterviewSessionListViewModel> CheckDuplicate(Guid interviewerId, DateTime startTime, DateTime endTime);
    }
}
