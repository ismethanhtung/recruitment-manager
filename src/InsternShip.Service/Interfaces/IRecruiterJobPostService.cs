using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IRecruiterJobPostService
    {
        Task<bool> Create(Guid recId, CreateJobModel request);
        Task<RecruiterJobPostListViewModel> GetAll(Guid? recId, int page, int limit, string? search, bool deleted);
        
        Task<bool> Update(Guid postId, JobUpdateModel request);
        Task<bool> Delete(Guid postId);
        Task<bool> Restore(Guid postId);
        Task<RecruiterJobPostViewModel> GetById(Guid recId);
        Task<IEnumerable<RecruiterPostedViewModel>> GetAllRecPosted();
        Task<AppliedListViewModel> GetAllAppliedOfCandidate(Guid candidateId, int page, int limit, string? status);
        Task<OneRecruiterJobPostListViewModel> GetAllJobPostOfRecruiter(Guid recruiterId, int page, int limit);
    }
}
