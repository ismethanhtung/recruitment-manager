using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IInterviewService
    {
        Task<InterviewListViewModel> GetAll(int page, int limit, bool deleted);
        Task<int[]> GetAllInterviewInWeek();
        Task<InterviewViewModel> GetById(Guid request);
        Task<MyInterviewListViewModel> GetByInterviewerId(Guid interviewerId, int page, int limit);
        Task<MyInterviewListViewModel> GetByCandidateId(Guid candidateId, int page, int limit);
        Task<bool> Create(CreateInterviewModel request);
        Task<bool> Delete(Guid request);
        Task<bool> Restore(Guid request);
        Task<bool> Update(Guid id, InterviewUpdateModel request);
        //Task<bool> CreateSession(Guid interviewId, Guid interviewerId);
        Task<bool> CreateSession(Guid interviewId, Guid interviewerId);
        //Task<bool> AddTest(Guid interviewId, Guid testId, Guid interviewerId);
        Task<int[]> GetAllInWeekByRecId(Guid recId);

        Task<InterviewDetailViewModel> GetDetail(Guid interviewId);
    }
}
