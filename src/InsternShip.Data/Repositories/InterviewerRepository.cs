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
    public class InterviewerRepository : Repository<Interviewer>, IInterviewerRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public InterviewerRepository(RecruitmentDB dbContext, IUnitOfWork uow, IMapper mapper) : base(dbContext)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<InterviewerListViewModel> GetAll(string? search, int page, int limit, bool deleted)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                var query = Entities
                           .Include(itn => itn.UserAccount).ThenInclude(usr => usr.UserInfo);

                var data = await query.Where(x => ((x.UserAccount.UserInfo.FirstName.Contains(search))
                                                || x.UserAccount.UserInfo.LastName.Contains(search)) 
                                                && x.IsDeleted == deleted
                                                && x.UserAccount.EmailConfirmed == true)
                                        .ToListAsync();

                var listData = new List<InterviewerViewModel>();
                int count = data.Count();
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<InterviewerViewModel>(item);
                    listData.Add(obj);
                };
                return new InterviewerListViewModel
                {
                    InterviewerList = listData,
                    TotalCount = count

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<InterviewerViewModel> GetById(Guid? interviewerId)
        {
            var chosen = await Entities.Include(intevr => intevr.UserAccount).ThenInclude(usr=>usr.UserInfo).FirstOrDefaultAsync(x => x.InterviewerId == interviewerId && x.IsDeleted ==false && x.UserAccount.EmailConfirmed == true)
                ?? throw new KeyNotFoundException(ExceptionMessages.InterviewerNotFound);
            try
            {
                var obj = _mapper.Map<InterviewerViewModel>(chosen);
                return await Task.FromResult<InterviewerViewModel>(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<InterviewerViewModel?> GetByUserId(Guid userId)
        {
            var data = await Entities.Include(i => i.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(inter => inter.UserId == userId && inter.UserAccount.EmailConfirmed == true);
            if (data != null)
            {
                var inter = _mapper.Map<InterviewerViewModel>(data);
                return inter;
            }
            return null;
        }

        public async Task<Guid?> GetIdByUserId(Guid userId)
        {
            var data = await Entities.FirstOrDefaultAsync(inter => inter.UserId == userId);
            if (data != null) { return data.InterviewerId; }
            return null;
        }

        public async Task<bool> Create(CreateInterviewerModel request)
        {
            var intr = await Entities.Include(intevr => intevr.UserAccount).ThenInclude(usr => usr.UserInfo).FirstOrDefaultAsync(x => x.UserId == request.UserId && x.IsDeleted == false);
            if(intr!=null) throw new DuplicateException(ExceptionMessages.InterviewerExisted);
            try
            {
                var newInterviewer = _mapper.Map<Interviewer>(request);
                Entities.Add(newInterviewer);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(Guid interviewerId)
        {
            var chosen = await Entities.Include(intevr => intevr.UserAccount).ThenInclude(usr => usr.UserInfo).FirstOrDefaultAsync(x => x.InterviewerId == interviewerId && x.IsDeleted == false)
                ?? throw new KeyNotFoundException(ExceptionMessages.InterviewerNotFound);
            try
            {
                //Entities.Remove(chosen);
                _uow.BeginTransaction();
                chosen.IsDeleted = true;
                chosen.UserAccount.IsDeleted = true;
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
        public async Task<bool> Update(Guid? interviewerId, InterviewerUpdateModel request)
        {
            var chosen = await Entities.Include(intevr => intevr.UserAccount).ThenInclude(usr => usr.UserInfo).FirstOrDefaultAsync(x => x.InterviewerId == interviewerId && x.IsDeleted == false)
                ?? throw new KeyNotFoundException(ExceptionMessages.InterviewerNotFound);
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
        public async Task<bool> Restore(Guid interviewerId)
        {
            var data = await Entities.Include(intevr => intevr.UserAccount).FirstOrDefaultAsync(x=> x.InterviewerId ==interviewerId) ?? throw new KeyNotFoundException(ExceptionMessages.InterviewerNotFound);
            if (data.IsDeleted == false) throw new AppException(ExceptionMessages.InterviewerNotDeleted);
            try
            {
                _uow.BeginTransaction();
                data.IsDeleted = false;
                data.UserAccount.IsDeleted = false;
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

