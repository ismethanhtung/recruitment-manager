using AutoMapper;
using InsternShip.Common;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InsternShip.Data.Repositories
{
    public class ApplicationRepository : Repository<Application>, IApplicationRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ApplicationRepository(RecruitmentDB dbContext, IUnitOfWork uow, IMapper mapper) : base(dbContext)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<InfoToInterviewModel> GetMailInfoByAppId(Guid appId)
        {
            var query = Entities.Include(app => app.Candidate)
                                .ThenInclude(can => can.UserAccount)
                                .ThenInclude(user => user.UserInfo);
            var dataMail = await query.FirstOrDefaultAsync(app => app.ApplicationId == appId);
            var info = _mapper.Map<InfoToInterviewModel>(dataMail);
      
            return info;
        }
        public async Task<ApplicationListViewModel> GetAll(string? search, int page, int limit, string? statusDesc, Guid? candidateId, Guid? jobPostId, bool deleted)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                statusDesc = string.IsNullOrEmpty(statusDesc) ? string.Empty : statusDesc.ToLower();
                var listData = new List<ApplicationViewModel>();
                var data = await Entities
                                    .Include(a => a.Candidate)
                                        .ThenInclude(c => c.UserAccount)
                                            .ThenInclude(uc => uc.UserInfo)
                                    .Include(a => a.RecruiterJobPosts)
                                        .ThenInclude(rjp => rjp.Job)
                                    .Include(a => a.ApplicationStatusUpdates)
                                    .Where(x => (x.Candidate.UserAccount.UserInfo.FirstName.Contains(search) ||
                                            x.Candidate.UserAccount.UserInfo.LastName.Contains(search))
                                            && x.IsDeleted == deleted 
                                            && x.RecruiterJobPosts.IsDeleted == false 
                                            && x.Candidate.IsDeleted == false).ToListAsync();

                if (candidateId != null) data = data.Where(x => x.CandidateId == candidateId).ToList();
                if (jobPostId != null) data = data.Where(x => x.JobPostId == jobPostId).ToList();
                foreach (var item in data)
                {

                    var obj = _mapper.Map<ApplicationViewModel>(item);
                    var data2 = await Entities.Select(x => new
                    {
                        Status = x.ApplicationStatusUpdates.Where(y => y.ApplicationId == x.ApplicationId
                                                                                    && y.LatestUpdate == x.ApplicationStatusUpdates.Max(z => z.LatestUpdate)
                                                                                    && y.Status.Description.ToLower().Contains(statusDesc))
                                                                    .Select(y => y.Status.Description).First(),

                        x.ApplicationId,

                    }).FirstOrDefaultAsync(dto => dto.ApplicationId == obj.ApplicationId);
                    obj.Status = data2.Status;
                    if (obj.Status != null)
                    {
                        listData.Add(obj);
                    }
                    
                    //int count = data2.Status.Count();

                };
                int count = listData.Count;
                listData = listData.Skip((page - 1) * limit).Take(limit).ToList();
                return new ApplicationListViewModel
                {
                    ApplicationList = listData,
                    TotalCount = count

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApplicationViewModel> GetById(Guid request)
        {
            var query = Entities
                                .Include(a => a.Candidate)
                                    .ThenInclude(c => c.UserAccount)
                                        .ThenInclude(uc => uc.UserInfo)
                                .Include(a => a.RecruiterJobPosts)
                                    .ThenInclude(rjp => rjp.Job)
                                .Include(a => a.ApplicationStatusUpdates);
            var chosen = await query.FirstOrDefaultAsync(x => x.ApplicationId == request 
                                && x.IsDeleted == false
                                && x.Candidate.IsDeleted == false
                                && x.RecruiterJobPosts.IsDeleted == false

                )
                ?? throw new KeyNotFoundException(ExceptionMessages.ApplicationNotFound);
            try
            {
                var obj = _mapper.Map<ApplicationViewModel>(chosen);
                var data2 = await Entities.Select(x => new
                {
                    Status = x.ApplicationStatusUpdates.Where(y => y.ApplicationId == x.ApplicationId && y.LatestUpdate == x.ApplicationStatusUpdates.Max(z => z.LatestUpdate))
                                                             .Select(y => y.Status.Description).First(),
                    ApplicationId = x.ApplicationId,

                }).FirstOrDefaultAsync(dto => dto.ApplicationId == obj.ApplicationId);
                obj.Status = data2.Status;
                return await Task.FromResult<ApplicationViewModel>(obj);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AlreadyAppliedForJob(ApplicationModel request)
        {
            var query = await Entities.Include(a => a.Candidate)
                                    .ThenInclude(c => c.UserAccount)
                                        .ThenInclude(uc => uc.UserInfo)
                                .Include(a => a.RecruiterJobPosts)
                                    .ThenInclude(rjp => rjp.Job)
                                .Include(a => a.ApplicationStatusUpdates)
                                .FirstOrDefaultAsync(x => x.CandidateId == request.CandidateId 
                                                    && x.JobPostId == request.JobPostId 
                                                    && x.IsDeleted == false
                                                    && x.Candidate.IsDeleted == false
                                                    && x.RecruiterJobPosts.IsDeleted == false);
            if(query == null) return await Task.FromResult(false);
            return await Task.FromResult(true);
        }

        public async Task<bool> Create(ApplicationCreateModel request)
        {
            if (await Entities.FirstOrDefaultAsync(x => x.CandidateId == request.CandidateId && x.JobPostId == request.JobPostId && x.IsDeleted == false) != null)
                throw new KeyNotFoundException(ExceptionMessages.AlreadyApplied);
            try
            {
                var newApplication = _mapper.Map<Application>(request);
                newApplication.ApplyDate = DateTime.Now;

                Entities.Add(newApplication);
                _uow.SaveChanges();
                return await Task.FromResult(true);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> CreateGuid(ApplicationCreateModel request)
        {
            if (await Entities.FirstOrDefaultAsync(x => x.CandidateId == request.CandidateId && x.JobPostId == request.JobPostId && x.IsDeleted == false) != null)
                throw new KeyNotFoundException(ExceptionMessages.AlreadyApplied);
            try
            {
                var newApplication = _mapper.Map<Application>(request);
                newApplication.ApplyDate = DateTime.Now;

                Entities.Add(newApplication);
                _uow.SaveChanges();
                return await Task.FromResult(newApplication.ApplicationId);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(Guid request)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.ApplicationId == request && x.IsDeleted == false)
                ?? throw new KeyNotFoundException(ExceptionMessages.ApplicationNotFound);
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
        public async Task<bool> Restore(Guid request)
        {
            var chosen = await Entities.FindAsync(request)
                ?? throw new KeyNotFoundException(ExceptionMessages.ApplicationNotFound);
            if (chosen.IsDeleted == false) throw new KeyNotFoundException(ExceptionMessages.ApplicationNotDeleted);
            try
            {
                _uow.BeginTransaction();
                chosen.IsDeleted = false;
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
        public async Task<IEnumerable<RecruitmentReportModel>> GetRecruitmentReport()
        {
            try
            {
                var query = Entities.Include(a => a.Candidate)
                                    .ThenInclude(c => c.UserAccount)
                                        .ThenInclude(uc => uc.UserInfo)
                                .Include(a => a.RecruiterJobPosts)
                                    .ThenInclude(rjp => rjp.Job);

                var data = await query.ToListAsync();

                //var first = data.FirstOrDefault();
                List<Guid> jobids = new List<Guid>();
                foreach (var item in data)
                {
                    if (!jobids.Contains(item.JobPostId))
                    {
                        jobids.Add(item.JobPostId);
                    }
                }
                var joblist = data.GroupBy(x => x.JobPostId).ToList();
                var listData = new List<RecruitmentReportModel>();
                foreach (var item in jobids)
                {
                    var job = data.FirstOrDefault(x => x.JobPostId == item).RecruiterJobPosts;
                    var count = data.Where(x => x.JobPostId == item).ToList().Count;
                    listData.Add(new RecruitmentReportModel
                    {
                        JobPostId = item,
                        Name = job.Job.Name,
                        Level = job.Job.Level,
                        Location = job.Job.Location,
                        Benefit = job.Job.Benefit,
                        MinSalary = job.Job.MinSalary,
                        MaxSalary = job.Job.MaxSalary,
                        Quantity = job.Job.Quantity,
                        NumOfRegistration = count,
                        StartDate = job.Job.CreateDate,
                        EndDate = job.Job.EndDate,
                        IsJobPostDeleted = job.IsDeleted
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
