using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IEventRepository
    {
        Task<bool> Create(CreateEventModel request);
        Task<Guid> CreateGUID(CreateEventModel request);
        Task<EventListViewModel> GetAll(string? search, int page, int limit, bool deleted);
        Task<EventViewModel> GetById(Guid EventId);
        Task<bool> Update(Guid eventId, EventUpdateModel request);
        Task<bool> Delete(Guid request);
        Task<bool> Restore(Guid request);
    }
}
