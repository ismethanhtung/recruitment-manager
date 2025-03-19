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
    public class RecruiterRepository: Repository<Recruiter>, IRecruiterRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RecruiterRepository(RecruitmentDB context, IUnitOfWork uow, IMapper mapper) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<RecruiterListViewModel> GetAll(string? search, int page, int limit, bool deleted)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                var listData = new List<RecruiterViewModel>();
                var query = Entities
                                    .Include(c => c.UserAccount).ThenInclude(uc => uc.UserInfo);
                var data = await query.Where(x => (x.Description.Contains(search)) && (x.IsDeleted == deleted) && x.UserAccount.EmailConfirmed == true).ToListAsync();
                int count = data.Count();
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<RecruiterViewModel>(item);
                    listData.Add(obj);
                };
                return new RecruiterListViewModel
                {
                    RecruiterList = listData,
                    TotalCount = count

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<RecruiterViewModel> GetById(Guid? recId)
        {
            var data = await Entities.Include(r => r.UserAccount).ThenInclude(uc => uc.UserInfo).FirstOrDefaultAsync(x => x.RecruiterId == recId && x.IsDeleted == false && x.UserAccount.EmailConfirmed == true) 
                ?? throw new KeyNotFoundException(ExceptionMessages.RecruiterNotFound);
            var rec = _mapper.Map<RecruiterViewModel>(data);
            return await Task.FromResult<RecruiterViewModel>(rec);
        }
        public async Task<RecruiterViewModel> GetByUserId(Guid userId)
        {
            var data = await Entities.Include(r => r.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(rec => rec.UserId == userId && rec.IsDeleted == false && rec.UserAccount.EmailConfirmed == true);
            if (data != null)
            {
                var rec = _mapper.Map<RecruiterViewModel?>(data);
                return rec;
            }
            return null;
        }

        public async Task<Guid?> GetIdByUserId(Guid userId)
        {
            var data = await Entities.FirstOrDefaultAsync(rec => rec.UserId == userId && rec.IsDeleted == false);
            if (data != null) { return data.RecruiterId; }
            return null;
        }

        public async Task<bool> Create(CreateRecruiterModel request)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.IsDeleted == false);
            if (data != null) throw new DuplicateException(ExceptionMessages.RecruiterExisted);
            var rec = _mapper.Map<Recruiter>(request);
            Entities.Add(rec);
            _uow.SaveChanges();
            return await Task.FromResult(true);
        }

        public async Task<bool> Update(Guid? recruiterId, RecruiterUpdateModel request)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.RecruiterId == recruiterId && x.IsDeleted == false)
                ?? throw new KeyNotFoundException(ExceptionMessages.RecruiterNotFound);
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

        public async Task<bool> Delete(Guid request)
        {
            var chosen = await Entities.Include(x=>x.UserAccount).FirstOrDefaultAsync(x => x.RecruiterId == request && x.IsDeleted == false)
                ?? throw new KeyNotFoundException(ExceptionMessages.RecruiterNotFound);
            try
            {
                _uow.BeginTransaction();
                chosen.IsDeleted = true;
                chosen.UserAccount.IsDeleted = true;
                //var entry = Entities.Entry(chosen);
                //entry.CurrentValues.SetValues(request);
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
        public async Task<bool> Restore(Guid request)
        {
            var chosen = await Entities.Include(x => x.UserAccount).FirstOrDefaultAsync(x => x.RecruiterId == request) ?? throw new KeyNotFoundException(ExceptionMessages.RecruiterNotFound);
            if (chosen.IsDeleted == false) throw new AppException(ExceptionMessages.RecruiterNotDeleted);
            try
            {
                _uow.BeginTransaction();
                chosen.IsDeleted = false;
                chosen.UserAccount.IsDeleted = false;
                //var entry = Entities.Entry(chosen);
                //entry.CurrentValues.SetValues(request);
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
