using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IRecruiterEventPostRepository
    {
        Task<bool> Create(RecruiterEventPostModel request);
        Task<RecruiterEventPostListViewModel> GetAll(Guid? recId, int page, int limit, string? search, bool deleted);
        Task<RecruiterEventPostViewModel> GetById(Guid recEPId);
        Task<bool> Update(Guid postId, RecruiterEventPostModel request);
        Task<bool> Delete(Guid postId);
        Task<IEnumerable<RecruiterPostedViewModel>> GetAllRecPosted();
        Task<bool> Restore(Guid postId);
        Task<Guid> GetEventId(Guid request);
        //Task<IEnumerable<EventReportModel>> GetEventReport();
    }
}
