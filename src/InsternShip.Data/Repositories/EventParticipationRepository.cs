using AutoMapper;
using InsternShip.Common;
using InsternShip.Common.Exceptions;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InsternShip.Data.Repositories
{
    public class EventParticipationRepository : Repository<EventParticipation>, IEventParticipationRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public EventParticipationRepository(RecruitmentDB context, IUnitOfWork uow, IMapper mapper) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> CountByEventId(Guid EventPostId)
        {
            try
            {
                return await Entities.Include(p => p.Candidate)
                    .Include(p => p.RecruiterEventPost)
                    .ThenInclude(ep => ep.Recruiter).
                    CountAsync(x => x.EventPostId == EventPostId 
                    && x.Candidate.IsDeleted == false 
                    && x.RecruiterEventPost.IsDeleted==false
                    && x.RecruiterEventPost.Recruiter.IsDeleted == false);
            }
            catch
            {
                throw new Exception(ExceptionMessages.UnexpectedException);
            }
        }

        public async Task<bool> Create(EventParticipationCreateModel request, int? max)
        {
            max ??= 0;
            var item = await Entities.Include(x => x.RecruiterEventPost).ThenInclude(y => y.Event).FirstOrDefaultAsync(x => x.EventPostId == request.EventPostId && x.CandidateId == request.CandidateId);
            if (item != null) {
                throw new KeyNotFoundException(ExceptionMessages.EventParticipationFound);
            }
            
            if (!(await CountByEventId(request.EventPostId) + 1 <= max)) throw new AppException(ExceptionMessages.EventMaxCandidate);
            try
            {
                var obj = _mapper.Map<EventParticipation>(request);
                obj.Status = false;
                Entities.Add(obj);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<EventParticipationListViewModel> GetAll(string? search, int page, int limit, Guid? candidateId, Guid? eventPostId)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                var listData = new List<EventParticipationViewModel>();
                var data = await Entities
                                    .Include(e => e.RecruiterEventPost)
                                        .ThenInclude(rt => rt.Recruiter)
                                            .ThenInclude(c => c.UserAccount)
                                                .ThenInclude(uc => uc.UserInfo)
                                    .Include(e => e.RecruiterEventPost)
                                        .ThenInclude(ev => ev.Event)
                                    .Include(e => e.Candidate)
                                        .ThenInclude(c => c.UserAccount)
                                                .ThenInclude(uc => uc.UserInfo)
                    .Where(x => (
                                    (x.Candidate.UserAccount.UserInfo.FirstName.Contains(search) ||
                                    x.Candidate.UserAccount.UserInfo.LastName.Contains(search))
                                    && x.RecruiterEventPost.IsDeleted == false
                                    && x.RecruiterEventPost.Recruiter.IsDeleted ==false
                                    && x.Candidate.IsDeleted == false

                                )
                           ).ToListAsync();

                if (candidateId != null) data = data.Where(x => x.CandidateId == candidateId).ToList();
                if (eventPostId != null) data = data.Where(x => x.EventPostId == eventPostId).ToList();

                int count = data.Count();
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<EventParticipationViewModel>(item);
                    listData.Add(obj);
                };
                return new EventParticipationListViewModel
                {
                    EventParticipationList = listData,
                    TotalCount = count

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<EventParticipationViewModel> GetById(Guid ParticipationId)
        {
            var item = await Entities.Include(e => e.RecruiterEventPost)
                                        .ThenInclude(rt => rt.Recruiter)
                                            .ThenInclude(c => c.UserAccount)
                                                .ThenInclude(uc => uc.UserInfo)
                                    .Include(e => e.RecruiterEventPost)
                                        .ThenInclude(ev => ev.Event)
                                    .Include(e => e.Candidate)
                                        .ThenInclude(c => c.UserAccount)
                                                .ThenInclude(uc => uc.UserInfo)
               .FirstOrDefaultAsync(x => x.ParticipationId == ParticipationId
                                    && x.RecruiterEventPost.IsDeleted == false
                                    && x.RecruiterEventPost.Recruiter.IsDeleted == false
                                    && x.Candidate.IsDeleted == false
               )
                ?? throw new KeyNotFoundException(ExceptionMessages.EventParticipationNotFound);
            try
            {
                var obj = _mapper.Map<EventParticipationViewModel>(item);
                return await Task.FromResult(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(Guid ParticipationId)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.ParticipationId == ParticipationId)
                ?? throw new KeyNotFoundException(ExceptionMessages.EventParticipationNotFound);
            try
            {
                var entry = Entities.Entry(data);
                entry.Property(x => x.Status).CurrentValue = true;
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateByCEId(Guid CandidateId, Guid EventPostId, Guid RecruiterId)
        {
            var data = await Entities.Include(x => x.RecruiterEventPost).FirstOrDefaultAsync(x => x.CandidateId == CandidateId && x.EventPostId == EventPostId)
                ?? throw new KeyNotFoundException(ExceptionMessages.EventParticipationNotFound);
            if (data.RecruiterEventPost.RecruiterId != RecruiterId) throw new KeyNotFoundException(ExceptionMessages.RecruiterNotPermission);
            try
            {
                var entry = Entities.Entry(data);
                entry.Property(x => x.Status).CurrentValue = true;
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(Guid ParticipationId)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.ParticipationId == ParticipationId)
               ?? throw new KeyNotFoundException(ExceptionMessages.EventParticipationNotFound);
            try
            {
                Entities.Remove(data);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByCEid(Guid CandidateId, Guid EventPostId)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.CandidateId == CandidateId && x.EventPostId == EventPostId)
               ?? throw new KeyNotFoundException(ExceptionMessages.EventParticipationNotFound);
            try
            {
                Entities.Remove(data);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        

        public async Task<EventPostParticipationListViewModel> GetAllEventOfCandidate (Guid CandidateId, string? search, int page, int limit)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                var listData = new List<EventPostParticipationViewModel>();
                var query = Entities
                                    .Include(e => e.RecruiterEventPost)
                                        .ThenInclude(rt => rt.Recruiter)
                                            .ThenInclude(c => c.UserAccount)
                                                .ThenInclude(uc => uc.UserInfo)
                                    .Include(e => e.RecruiterEventPost)
                                        .ThenInclude(ev => ev.Event);
                var data = await query.Where(x => x.CandidateId == CandidateId && (x.RecruiterEventPost.Event.Name.Contains(search)
                                               && x.RecruiterEventPost.IsDeleted == false
                                               && x.RecruiterEventPost.Recruiter.IsDeleted ==false
                                               && x.Candidate.IsDeleted == false
                ))
                                    .ToListAsync();

                int count = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<EventPostParticipationViewModel>(item);
                    listData.Add(obj);
                };
                return new EventPostParticipationListViewModel
                {
                    EventPostParticipationList = listData,
                    TotalCount = count

                };
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<EventReportModel>> GetEventReport()
        {
            try
            {
                var query = Entities.Include(p => p.RecruiterEventPost)
                                        .ThenInclude(rep => rep.Recruiter)
                                            .ThenInclude(rec => rec.UserAccount)
                                                .ThenInclude(uc => uc.UserInfo)
                                    .Include(p => p.RecruiterEventPost)
                                        .ThenInclude(rep => rep.Event)
                                    .Include(p => p.Candidate);

                var data = await query.ToListAsync();

                List<Guid> eventIds = new List<Guid>();
                foreach (var item in data)
                {
                    if (!eventIds.Contains(item.RecruiterEventPost.EventPostId))
                    {
                        eventIds.Add(item.RecruiterEventPost.EventPostId);
                    }
                }
                var eventList = data.GroupBy(x => x.EventPostId).ToList();
                var listData = new List<EventReportModel>();
                foreach (var item in eventIds)
                {
                    var post = data.FirstOrDefault(x => x.EventPostId == item).RecruiterEventPost;
                    var count = data.Where(x => x.EventPostId == item).ToList().Count;
                    string type = "";
                    if (post.Event.Status == true)
                    {
                        type = "online";
                    }
                    else type = "offline";
                    listData.Add(new EventReportModel
                    {
                        EventPostId = post.EventPostId,
                        Name = post.Event.Name,
                        Quantity = post.Event.MaxCandidate,
                        NumOfRegistration = count,
                        StartDate = post.Event.StartDate,
                        EndDate = post.Event.EndDate,
                        Method = type,
                        IsEventDeleted = post.IsDeleted
                    });

                };
                return await Task.FromResult(listData);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }


}
