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
    public class RecruiterEventPostRepository: Repository<RecruiterEventPost>, IRecruiterEventPostRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public RecruiterEventPostRepository(RecruitmentDB context, IUnitOfWork uow, IMapper mapper) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> Create(RecruiterEventPostModel request)
        {
            try
            {
                var eventPost = _mapper.Map<RecruiterEventPost>(request);
                Entities.Add(eventPost);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RecruiterEventPostListViewModel> GetAll(Guid? recId, int page, int limit, string? search, bool deleted)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;

                var listData = new List<RecruiterEventPostViewModel>();
                var query = Entities
                            .Include(rjp => rjp.Recruiter)
                                .ThenInclude(rec => rec.UserAccount)
                                    .ThenInclude(acc => acc.UserInfo)
                            .Include(ev => ev.Event);
                var data = await (
                recId == null
                    ? query.Where(rep => rep.Event.Name.Contains(search) 
                            && rep.IsDeleted == deleted
                            && rep.Recruiter.IsDeleted == false)
                            .OrderByDescending(rep => rep.Event.PostDate)
                    : query.Where(rep => rep.RecruiterId == recId && rep.Event.Name.Contains(search) 
                            && rep.IsDeleted == deleted
                            && rep.Recruiter.IsDeleted == false)
                            .OrderByDescending(rep => rep.Event.PostDate)
                    )
                    .ToListAsync();

                var totalCount = data.Count();
                data = data.Skip((page - 1) * limit).Take(limit).ToList();

                foreach (var item in data)
                {
                    var eventPost = _mapper.Map<RecruiterEventPostViewModel>(item);
                    listData.Add(eventPost);
                }
                return new RecruiterEventPostListViewModel
                {
                    TotalCount = totalCount,
                    EventPostList = listData
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(Guid postId, RecruiterEventPostModel request)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.EventPostId == postId && x.IsDeleted == false) ??
               throw new KeyNotFoundException(ExceptionMessages.RecruiterEventPostNotFound);
            try
            {
                _uow.BeginTransaction();
                var entry = Entities.Entry(data);
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

        public async Task<bool> Delete(Guid postId)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.EventPostId == postId && x.IsDeleted == false) ??
                throw new KeyNotFoundException(ExceptionMessages.RecruiterEventPostNotFound);
            try 
            {
                _uow.BeginTransaction();
                data.IsDeleted = true;
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

        public async Task<IEnumerable<RecruiterPostedViewModel>> GetAllRecPosted()
        {
            try
            {
                var listData = new List<RecruiterPostedViewModel>();
                var query = Entities
                            .Include(rjp => rjp.Recruiter)
                                .ThenInclude(rec => rec.UserAccount)
                                    .ThenInclude(acc => acc.UserInfo);
                var data = await query
                    .Where(x=>x.Recruiter.IsDeleted == false && x.IsDeleted == false)
                    .GroupBy(rjp => rjp.RecruiterId)
                    .Select(group => group.FirstOrDefault())
                    .ToListAsync();
                foreach (var item in data)
                {
                    var recPosted = _mapper.Map<RecruiterPostedViewModel>(item);
                    listData.Add(recPosted);
                }
                return listData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Restore(Guid postId)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.EventPostId == postId)
                ?? throw new KeyNotFoundException(ExceptionMessages.RecruiterEventPostNotFound);
            if (data.IsDeleted == false) throw new AppException(ExceptionMessages.EventPostNotDeleted);
            try
            {
                _uow.BeginTransaction();
                data.IsDeleted = false;
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
        public async Task<Guid> GetEventId(Guid request)
        {
            var data = await Entities.Include(rjp => rjp.Recruiter)
                                .FirstOrDefaultAsync(x => x.EventPostId == request 
                                && x.IsDeleted == false
                                && x.Recruiter.IsDeleted == false
                                );
            if (data != null)
            {
                return await Task.FromResult(data.EventId);
            }
            else
            {
                throw new KeyNotFoundException(ExceptionMessages.RecruiterEventPostNotFound);
            }
        }
        public async Task<RecruiterEventPostViewModel> GetById(Guid request)
        {
            var data = await Entities
                            .Include(rjp => rjp.Recruiter)
                                .ThenInclude(rec => rec.UserAccount)
                                    .ThenInclude(acc => acc.UserInfo)
                            .Include(ev => ev.Event).FirstOrDefaultAsync(x => x.EventPostId == request && x.IsDeleted == false && x.Recruiter.IsDeleted == false);
            if (data != null)
            {
                var obj = _mapper.Map<RecruiterEventPostViewModel>(data);
                return await Task.FromResult(obj);
            }
            else
            {
                throw new KeyNotFoundException(ExceptionMessages.RecruiterEventPostNotFound);
            }
        }
        
    }
}
