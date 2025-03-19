using AutoMapper;
using CloudinaryDotNet.Actions;
using InsternShip.Common;
using InsternShip.Common.Exceptions;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InsternShip.Data.Repositories
{
    public class CandidateRepository : Repository<Candidate>, ICandidateRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CandidateRepository(RecruitmentDB context, IUnitOfWork uow, IMapper mapper) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
        }

        
        public async Task<bool> Create(CreateCandidateModel request)
        {
            var cand = await Entities.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.IsDeleted == false);
            if (cand != null) throw new DuplicateException(ExceptionMessages.CandidateExisted);
            try
            {
                var obj = _mapper.Map<Candidate>(request);
                Entities.Add(obj);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CandidateListViewModel> GetAll(string? search, int page, int limit, bool deleted)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                var query = Entities
                           .Include(candidate => candidate.UserAccount).ThenInclude(user=>user.UserInfo);

                var data = await query.Where(x => (x.UserAccount.UserInfo.FirstName.Contains(search) && x.IsDeleted == deleted && x.UserAccount.EmailConfirmed == true)).ToListAsync();

                var listData = new List<CandidateViewModel>();
                int count = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<CandidateViewModel>(item);
                    listData.Add(obj);
                };
                return new CandidateListViewModel
                {
                    CandidateList = listData,
                    TotalCount = count

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<CandidateViewModel> GetById(Guid? CandidateId, string? cv) //,role)
        {
            var data = await Entities.Include(candidate => candidate.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(x => x.CandidateId == CandidateId && x.IsDeleted == false && x.UserAccount.EmailConfirmed == true)
                ?? throw new KeyNotFoundException(ExceptionMessages.CandidateNotFound);
            try
            {
                var obj = _mapper.Map<CandidateViewModel>(data);
                obj.CV = cv;
                return await Task.FromResult(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CandidateViewModel?> GetByUserId(Guid userId)
        {
            var item = await Entities.Include(candidate => candidate.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(x => x.UserId == userId && x.UserAccount.EmailConfirmed == true);
            if (item == null) return await Task.FromResult<CandidateViewModel?>(null);
            try
            {
                var obj = _mapper.Map<CandidateViewModel>(item);
                return await Task.FromResult<CandidateViewModel?>(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid?> GetIdByUserId(Guid userId)
        {
            var data = await Entities.Include(candidate => candidate.UserAccount).ThenInclude(user => user.UserInfo).FirstOrDefaultAsync(x => x.UserId == userId && x.UserAccount.EmailConfirmed == true);
            if (data != null) { return data.CandidateId; }
            return null;
        }


        public async Task<bool> Update(Guid? candidateId, CandidateUpdateModel request)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.CandidateId == candidateId && x.IsDeleted == false) ?? throw new KeyNotFoundException(ExceptionMessages.CandidateNotFound);
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

        public async Task<bool> Delete(Guid candidateId)
        {
            var data = await Entities.Include(x=>x.UserAccount).FirstOrDefaultAsync(x => x.CandidateId == candidateId && x.IsDeleted == false) ?? throw new KeyNotFoundException(ExceptionMessages.CandidateNotFound);
            try
            {
                _uow.BeginTransaction();
                data.IsDeleted = true;
                data.UserAccount.IsDeleted = true;
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
        public async Task<bool> Restore(Guid candidateId)
        {
            var data = await Entities.Include(x => x.UserAccount).FirstOrDefaultAsync(x=>x.CandidateId==candidateId) ?? throw new KeyNotFoundException(ExceptionMessages.CandidateNotFound);
            if (data.IsDeleted == false) throw new AppException(ExceptionMessages.CandidateNotDeleted);
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

        public async Task<string?> GetSkillSetsById(Guid? candidateId)
        {
            var data = await Entities.FindAsync(candidateId) ?? throw new KeyNotFoundException(ExceptionMessages.CandidateNotFound);
            return data.Skillsets;
        }

        public async Task<CandidateUpdateModel> GetMyCV(Guid? candidateId)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.CandidateId == candidateId && x.IsDeleted == false )
                ?? throw new KeyNotFoundException(ExceptionMessages.CandidateNotFound);
            try
            {
                var obj = _mapper.Map<CandidateUpdateModel>(data);
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}


