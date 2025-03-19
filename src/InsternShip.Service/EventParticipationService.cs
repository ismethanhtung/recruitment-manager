using InsternShip.Data.Interfaces;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class EventParticipationService : IEventParticipationService
    {
        private readonly IEventParticipationRepository _eventParticipationRepository;
        private readonly IRecruiterEventPostRepository _eventPostRepository;

        public EventParticipationService(IEventParticipationRepository eventParticipationRepository, IRecruiterEventPostRepository eventPostRepository)
        {
            _eventParticipationRepository = eventParticipationRepository;
            _eventPostRepository = eventPostRepository;
        }

        public async Task<int> CountByEventId(Guid EventPostId) 
        {
            await _eventPostRepository.GetEventId(EventPostId);
            return await _eventParticipationRepository.CountByEventId(EventPostId);
        }

        public async Task<EventParticipationListViewModel> GetAll(string? search, int page, int limit, Guid? candidateId, Guid? eventPostId)
        {
            return await _eventParticipationRepository.GetAll(search, page, limit, candidateId, eventPostId);
        }

        public async Task<EventPostParticipationListViewModel?> GetAllEventOfCandidate(Guid CandidateId, string? search, int page, int limit)
        {
            return await _eventParticipationRepository.GetAllEventOfCandidate(CandidateId, search, page, limit);
        }

        public async Task<EventParticipationViewModel> GetById(Guid ParticipationId)
        {
            return await _eventParticipationRepository.GetById(ParticipationId);
        }

        public async Task<bool> Update(Guid ParticipationId)
        {
            return await _eventParticipationRepository.Update(ParticipationId);
        }

        public async Task<bool> UpdateByCEId(Guid CandidateId, Guid EventPostId)
        {
            return await _eventParticipationRepository.UpdateByCEId(CandidateId, EventPostId, Guid.Empty);
        }

        public async Task<bool> Delete(Guid ParticipationId)
        {
            return await _eventParticipationRepository.Delete(ParticipationId);
        }

        public async Task<bool> DeleteByCEid(Guid CandidateId, Guid EventPostId)
        {
            return await _eventParticipationRepository.DeleteByCEid(CandidateId, EventPostId);
        }
    }
}
