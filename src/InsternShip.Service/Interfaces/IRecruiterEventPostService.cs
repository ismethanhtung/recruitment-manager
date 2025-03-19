using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IRecruiterEventPostService
    {
        Task<bool> Create(Guid recId, CreateEventModel request);
        Task<RecruiterEventPostListViewModel> GetAll(Guid? recId, int page, int limit, string? search, bool deleted);

        Task<bool> Update(Guid postId, EventUpdateModel request);
        Task<bool> Delete(Guid postId);
        Task<RecruiterEventPostViewModel> GetById(Guid recEPId);
        Task<EventParticipationListViewModel> GetParticipant(Guid? EventPostId, int page, int limit, string? search);
        Task<bool> Restore(Guid postId);
        Task<IEnumerable<RecruiterPostedViewModel>> GetAllRecPosted();
        Task<bool> CandidateApprovedEvent(Guid RecruiterId, Guid CandidateId, Guid EventPostId);
    }
}
