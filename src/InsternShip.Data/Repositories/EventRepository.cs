using AutoMapper;
using InsternShip.Common;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InsternShip.Data.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public EventRepository(RecruitmentDB context, IUnitOfWork uow, IMapper mapper) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> Create(CreateEventModel request)
        {
            try
            {
                var obj = _mapper.Map<Event>(request);
                obj.PostDate = DateTime.Now;

                
                Entities.Add(obj);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Guid> CreateGUID(CreateEventModel request)
        {
            try
            {
                var obj = _mapper.Map<Event>(request);
                obj.PostDate = DateTime.Now;
                Entities.Add(obj);
                _uow.SaveChanges();
                return await Task.FromResult(obj.EventId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventListViewModel> GetAll(string? search, int page, int limit, bool deleted)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                var listData = new List<EventViewModel>();
                var data = await Entities.Where(x => (x.Name.Contains(search)) && (x.IsDeleted == deleted)).ToListAsync();
                int count = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<EventViewModel>(item);
                    listData.Add(obj);
                };
                return new EventListViewModel
                {
                    EventList = listData,
                    TotalCount = count

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<EventViewModel> GetById(Guid EventId)
        {
            var item = await Entities.FirstOrDefaultAsync(x => x.EventId == EventId && x.IsDeleted == false) 
                ?? throw new KeyNotFoundException(ExceptionMessages.EventNotFound);
            var obj = _mapper.Map<EventViewModel>(item);
            return await Task.FromResult(obj);

        }

        public async Task<bool> Update(Guid eventId, EventUpdateModel request)
        {
            var chosen = await Entities.FindAsync(eventId)
                ?? throw new KeyNotFoundException(ExceptionMessages.EventNotFound);
            if (chosen.IsDeleted == true)
            {
                throw new KeyNotFoundException(ExceptionMessages.EventNotFound);
            }
            else
            {
                try
                {
                    _uow.BeginTransaction();
                    var entry = Entities.Entry(chosen);
                    entry.CurrentValues.SetValues(request);
                    _uow.SaveChanges();
                    _uow.CommitTransaction();
                    return await Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    _uow.RollbackTransaction();
                    throw new Exception(ex.Message);
                }
            }
        }

            

        public async Task<bool> Delete(Guid request)
        {
            var chosen = await Entities.FindAsync(request)
                ?? throw new KeyNotFoundException(ExceptionMessages.EventNotFound);
            if (chosen.IsDeleted == true)
            {
                throw new KeyNotFoundException(ExceptionMessages.EventNotFound);
            }
            else
            {
                try
                {
                    _uow.BeginTransaction();
                    chosen.IsDeleted = true;
                    _uow.SaveChanges();
                    _uow.CommitTransaction();
                    return await Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    _uow.RollbackTransaction();
                    throw new Exception(ex.Message);
                }
            }

        }

        public async Task<bool> Restore(Guid request)
        {
            var chosen = await Entities.FindAsync(request)
                ?? throw new KeyNotFoundException(ExceptionMessages.EventNotFound);
            if (chosen.IsDeleted == false)
            {
                throw new KeyNotFoundException(ExceptionMessages.EventNotFound);
            }
            else
            {
                try
                {
                    chosen.IsDeleted = false;
                    _uow.SaveChanges();
                    return await Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        }
    }
}
