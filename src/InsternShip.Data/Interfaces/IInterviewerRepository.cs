using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IInterviewerRepository
    {
        Task<InterviewerListViewModel> GetAll(string? search, int page, int limit, bool deleted);
        Task<InterviewerViewModel> GetById(Guid? interviewerId);
        Task<bool> Create(CreateInterviewerModel request);
        Task<bool> Delete(Guid interviewerId);
        Task<bool> Update(Guid? interviewerId, InterviewerUpdateModel request);
        Task<bool> Restore(Guid interviewerId);
        Task<InterviewerViewModel?> GetByUserId(Guid userId);
        Task<Guid?> GetIdByUserId(Guid userId);
    }
}
