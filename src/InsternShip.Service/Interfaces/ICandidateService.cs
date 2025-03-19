using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface ICandidateService
    {
        Task<bool> Create(CreateCandidateModel request);
        Task<CandidateListViewModel> GetAll(string? search, int page, int limit, bool deleted);
        Task<CandidateViewModel> GetById(Guid CandidateId);
        Task<bool> Update(Guid userId, CandidateUserInfoUpdateModel request);
        Task<bool> Delete(Guid CandidateId);
        Task<bool> JoinEvent(Guid CandidateId, Guid EventId);
        Task<bool> CancleEvent(Guid CandidateId, Guid EventPostId);
        Task<EventPostParticipationListViewModel> GetAllEventOfCandidate(Guid CandidateId, string? search, int page, int limit);
        Task<InterviewSessionListViewModel> GetAllSessions(Guid candidateId, int page, int limit);
        Task<bool> Restore(Guid CandidateId);
        Task<RecruiterJobPostListViewModel> SuggestionJobPost(Guid? candidateId, int page, int limit);
        Task<bool> UpdateMyCV(Guid candidateId, CandidateUpdateModel request);
        Task<CandidateUpdateModel> GetMyCV(Guid candidateId);
    }
}
