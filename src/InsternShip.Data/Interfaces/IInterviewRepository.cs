using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IInterviewRepository
    {
        Task<InterviewListViewModel> GetAll(int page, int limit, bool deleted);
        List<DateTime> GetDaysInWeek();
        Task<int[]> GetAllInterviewInWeek();
        Task<InterviewViewModel> GetById(Guid request);
        Task<InterviewViewModel?> GetByApplicationId(Guid request);
        Task<bool> Create(CreateInterviewModel request);
        Task<Guid> CreateGUID(CreateInterviewModel request);
        Task<bool> Delete(Guid request);
        Task<bool> Restore(Guid request);
        Task<bool> Update(Guid id, InterviewUpdateModel request);
        Task<int[]> GetAllInWeekByRecId(Guid recId);
        Task<int> GetScheduleInterviewByRecruiter(Guid recId);
        Task<IEnumerable<InterviewReportModel>> GetInterviewReport();

    }
}
