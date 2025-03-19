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
    public class BlackListRepository : Repository<BlackList>, IBlackListRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public BlackListRepository(RecruitmentDB context, IUnitOfWork uow, IMapper mapper) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
        }
      
        public async Task<BlackListEntriesViewModel> GetAll(string? search, int page, int limit, bool isOn)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                int sign = -1;
                if (isOn) { sign = 1; }
                var query = Entities
                           .Include(bl => bl.UserAccount).ThenInclude(user => user.UserInfo);

                var data = await query.Where(x => (x.UserAccount.UserInfo.FirstName.Contains(search) || x.UserAccount.UserInfo.LastName.Contains(search)) 
                && x.IsCurrentlyActive == isOn
                && ((DateTime)x.EntryDate).AddDays((int)x.Duration) > DateTime.Now).ToListAsync();

                var listData = new List<BlackListViewModel>();
                int count = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<BlackListViewModel>(item);
                    listData.Add(obj);
                };
                
                return new BlackListEntriesViewModel 
                {
                    EntryList = listData,
                    TotalCount = count

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BlackListViewModel> GetById(Guid blacklistId)
        {
            var chosen = await Entities.Include(bl => bl.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(x => x.BlackListId == blacklistId && x.IsCurrentlyActive == true
                && ((DateTime)x.EntryDate).AddDays((int)x.Duration) > DateTime.Now) ?? throw new KeyNotFoundException(ExceptionMessages.BlackListNotFound);
            try
            {
                var obj = _mapper.Map<BlackListViewModel>(chosen);
                return await Task.FromResult<BlackListViewModel>(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<BlackListViewModel> GetByUserId(Guid userId)
        {
            var chosen = await Entities.Include(bl => bl.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(x => x.UserId == userId && x.IsCurrentlyActive == true && ((DateTime)x.EntryDate).AddDays((int)x.Duration) > DateTime.Now) ?? throw new KeyNotFoundException(ExceptionMessages.BlackListNotFound);
            try
            {
                var obj = _mapper.Map<BlackListViewModel>(chosen);
                return await Task.FromResult<BlackListViewModel>(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> Check(Guid userId)
        {
            var chosen = await Entities.Include(bl => bl.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(x => x.UserId == userId && x.IsCurrentlyActive == true && ((DateTime)x.EntryDate).AddDays((int)x.Duration) > DateTime.Now);
            if (chosen == null) return await Task.FromResult(false);
            return await Task.FromResult(true);
        }
        public async Task<bool> Create(CreateBlackListModel request)
        {
            var chosen = await Entities.Include(bl => bl.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(x => x.UserAccount.Id == request.UserId && x.IsCurrentlyActive == true && ((DateTime)x.EntryDate).AddDays((int)x.Duration) > DateTime.Now);
            if (chosen != null) throw new DuplicateException(ExceptionMessages.UserAlrBanned);
            else try
                {
                    var oneBlocked = _mapper.Map<BlackList>(request);
                    oneBlocked.IsCurrentlyActive = true;
                    oneBlocked.EntryDate = DateTime.Now;
                    Entities.Add(oneBlocked);
                    _uow.SaveChanges();
                    return await Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

        }
        public async Task<bool> Update(Guid userId, BlackListUpdateModel request)
        {

            var chosen = await Entities.Include(bl => bl.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(x => x.UserAccount.Id == userId && x.IsCurrentlyActive == true) 
                ?? throw new KeyNotFoundException(ExceptionMessages.UserNotBanned);

            try
            {
                _uow.BeginTransaction();
                chosen.Reason = request.Reason;
                if (request.Duration > 0) chosen.Duration = request.Duration;
                Entities.Update(chosen);
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
        public async Task<bool> Delete(Guid userId)
        {
            var chosen = await Entities.Include(bl => bl.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(x => x.UserAccount.Id == userId && x.IsCurrentlyActive == true) ?? throw new KeyNotFoundException(ExceptionMessages.UserNotBanned);
            //if (chosen == null) throw new KeyNotFoundException(ExceptionMessages.UserNotBanned);
            try
            {
                _uow.BeginTransaction();

                chosen.IsCurrentlyActive = false;

                Entities.Update(chosen);

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
        public async Task<bool> Restore(Guid userId)
        {
            var chosen = await Entities.Include(bl => bl.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(x => x.UserAccount.Id == userId) ?? throw new KeyNotFoundException(ExceptionMessages.UserNotBanned);
            if (chosen.IsCurrentlyActive == true) throw new KeyNotFoundException(ExceptionMessages.UserAlrBanned);
            try
            {
                _uow.BeginTransaction();
                chosen.IsCurrentlyActive = true;

                Entities.Update(chosen);

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
}
