using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IRecruiterJobPostRepository
    {
        Task<bool> Create(RecruiterJobPostModel request);
        Task<RecruiterJobPostListViewModel> GetAll(Guid? recId, int page, int limit, string? search, bool deleted);
        Task<bool> Restore(Guid postId);
        Task<RecruiterJobPostViewModel> GetById(Guid request);
        Task<Guid> GetJobId(Guid request);
        Task<bool> Update(Guid postId, RecruiterJobPostModel request);
        Task<bool> Delete(Guid postId);
        Task<IEnumerable<RecruiterPostedViewModel>> GetAllRecPosted();
        Task<RecruiterJobPostListViewModel> GetAllSuggestion(int page, int limit, string? skillSets);
    }
}
