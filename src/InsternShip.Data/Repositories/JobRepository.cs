using AutoMapper;
using InsternShip.Common;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InsternShip.Data.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public JobRepository(RecruitmentDB context, IUnitOfWork uow, IMapper mapper) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<JobListViewModel> GetAll(int page, int limit, string? search)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                var listData = new List<JobViewModel>();
                var data = await Entities.Where(x => x.Name.Contains(search)).ToListAsync();
                int count = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<JobViewModel>(item);
                    listData.Add(obj);
                };
                return new JobListViewModel
                {
                    JobList = listData,
                    TotalCount = count
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<bool> Create(CreateJobModel job)
        {
            try
            {
                var obj = _mapper.Map<Job>(job);
                obj.UpdateDate = DateTime.Now;
                obj.CreateDate = DateTime.Now;
                Entities.Add(obj);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Guid> CreateGUID(CreateJobModel job)
        {
            try
            {
                var obj = _mapper.Map<Job>(job);
                obj.UpdateDate = DateTime.Now;
                obj.CreateDate = DateTime.Now;
                Entities.Add(obj);
                _uow.SaveChanges();
                return await Task.FromResult(obj.JobId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Update(Guid id, JobUpdateModel job)
        {
            var data = await Entities.FindAsync(id)
                ?? throw new KeyNotFoundException(ExceptionMessages.JobNotFound);
            try
            {
                _uow.BeginTransaction();
                var entry = Entities.Entry(data);
                entry.CurrentValues.SetValues(job);
                data.UpdateDate = DateTime.Now;
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

        public async Task<bool> Delete(Guid id)
        {

            return await Task.FromResult(true);

        }
        public async Task<JobViewModel> GetById(Guid jobid)
        {   
            var chosen = await Entities.FirstOrDefaultAsync(x => x.JobId == jobid);
            if (chosen != null)
            {
                var obj = _mapper.Map<JobViewModel>(chosen);
                return await Task.FromResult<JobViewModel>(obj);
            }
            else
            {
                throw new KeyNotFoundException(ExceptionMessages.JobNotFound);
            }
        }
    }
}
