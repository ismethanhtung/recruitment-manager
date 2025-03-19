using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IEventParticipationService
    {
        Task<int> CountByEventId(Guid EventPostId);
        //Task<bool> Create(EventParticipationCreateModel request);
        Task<EventParticipationListViewModel> GetAll(string? search, int page, int limit, Guid? candidateId, Guid? eventPostId);
        Task<EventPostParticipationListViewModel?> GetAllEventOfCandidate(Guid CandidateId, string? search, int page, int limit);
        Task<EventParticipationViewModel> GetById(Guid ParticipationId);
        Task<bool> Update(Guid ParticipationId);
        Task<bool> UpdateByCEId(Guid CandidateId, Guid EventPostId);
        Task<bool> Delete(Guid ParticipationId);
        Task<bool> DeleteByCEid(Guid CandidateId, Guid EventId);
    }
}
