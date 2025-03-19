using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IRecruiterService
    {
        Task<RecruiterListViewModel> GetAll(string? search, int page, int limit, bool deleted);
        Task<RecruiterViewModel> GetById(Guid recId);
        Task<bool> Create(CreateRecruiterModel request);
        Task<bool> Update(Guid userId, RecruiterUserInfoUpdateModel request);
        Task<bool> Delete(Guid request);
        Task<bool> Restore(Guid request);
        Task<bool> AcceptApplication(Guid recId,Guid applicationId);
        Task<bool> RejectApplication(Guid recId,Guid applicationId);
        Task<int> GetScheduleInterview(Guid recId);
        
    }
}
