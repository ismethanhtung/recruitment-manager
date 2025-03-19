using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class RecruiterEventPostService: IRecruiterEventPostService
    {
        private readonly IRecruiterEventPostRepository _recruiterEventPostRepository;
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventParticipationRepository _participationRepository;

        public RecruiterEventPostService(IRecruiterEventPostRepository recruiterEventPostRepository, IRecruiterRepository recruiterRepository, IEventRepository eventRepository, IEventParticipationRepository participationRepository)
        {
            _recruiterEventPostRepository = recruiterEventPostRepository;
            _recruiterRepository = recruiterRepository;
            _eventRepository = eventRepository;
            _participationRepository = participationRepository;
        }

        public async Task<bool> Create(Guid recId, CreateEventModel request)
        {
            _ = await _recruiterRepository.GetById(recId);
            var eventId = await _eventRepository.CreateGUID(request);
            var ep = new RecruiterEventPostModel() {
                RecruiterId = recId,
                EventId = eventId
            };
            return await _recruiterEventPostRepository.Create(ep);
        }

        public async Task<RecruiterEventPostListViewModel> GetAll(Guid? recId, int page, int limit, string? search, bool deleted)
        {
            return await _recruiterEventPostRepository.GetAll(recId, page, limit, search, deleted);
        }

        public async Task<bool> Update(Guid postId, EventUpdateModel request)
        {
            var eventId = await _recruiterEventPostRepository.GetEventId(postId);

            return await _eventRepository.Update(eventId, request);
        }

        public async Task<bool> Delete(Guid postId)
        {
            return await _recruiterEventPostRepository.Delete(postId);
        }

        public async Task<IEnumerable<RecruiterPostedViewModel>> GetAllRecPosted()
        {
            return await _recruiterEventPostRepository.GetAllRecPosted();
        }
        public async Task<RecruiterEventPostViewModel> GetById(Guid recEId)
        {
            return await _recruiterEventPostRepository.GetById(recEId);
        }
        public async Task<EventParticipationListViewModel> GetParticipant(Guid? eventPostId, int page, int limit, string? search)
        {
            return await _participationRepository.GetAll(search, page, limit,null, eventPostId);
        }
        public async Task<bool> Restore(Guid postId)
        {
            return await _recruiterEventPostRepository.Restore(postId);
        }
        public async Task<bool> CandidateApprovedEvent(Guid RecruiterId, Guid CandidateId, Guid EventPostId)
        {
            await _recruiterEventPostRepository.GetById(EventPostId);

            return await _participationRepository.UpdateByCEId(CandidateId, EventPostId, RecruiterId);
        }
    }
}
