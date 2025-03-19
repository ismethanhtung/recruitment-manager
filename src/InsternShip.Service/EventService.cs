using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
   
        public async Task<bool> Create(CreateEventModel request)
        {
            return await _eventRepository.Create(request);
        }

        public async Task<EventListViewModel> GetAll(string? search, int page, int limit, bool deleted)
        {
            return await _eventRepository.GetAll(search, page, limit, deleted);
        }

        public async Task<EventViewModel?> GetById(Guid EventId)
        {
            return await _eventRepository.GetById(EventId);
        }

        public async Task<bool> Update(Guid eventId, EventUpdateModel request)
        {
            return await _eventRepository.Update(eventId, request);
        }

        public async Task<bool> Delete(Guid request)
        {
            return await _eventRepository.Delete(request);
        }

        public async Task<bool> Restore(Guid request)
        {
            return await _eventRepository.Restore(request);
        }
    }
}
