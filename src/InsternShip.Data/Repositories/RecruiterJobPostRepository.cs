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
    public class RecruiterJobPostRepository : Repository<RecruiterJobPost>, IRecruiterJobPostRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public RecruiterJobPostRepository(RecruitmentDB context, IUnitOfWork uow, IMapper mapper) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<bool> Create(RecruiterJobPostModel request)
        {
            try
            {
                var jobPost = _mapper.Map<RecruiterJobPost>(request);
                Entities.Add(jobPost);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<RecruiterJobPostListViewModel> GetAll(Guid? recId, int page, int limit, string? search, bool deleted)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;

                var listData = new List<RecruiterJobPostViewModel>();
                var query = Entities.Include(jp => jp.Recruiter).
                            ThenInclude(rec => rec.UserAccount).
                            ThenInclude(uac => uac.UserInfo).
                            Include(jp=>jp.Job);
                var data = await (
                    recId == null
                    ? query.Where(rjp => rjp.Job.Name.Contains(search) && rjp.IsDeleted == deleted && rjp.Recruiter.IsDeleted == false)
                            .OrderByDescending(rjp => rjp.Job.UpdateDate)
                    : query.Where(rjp => rjp.RecruiterId == recId && rjp.Job.Name.Contains(search) && rjp.IsDeleted == deleted && rjp.Recruiter.IsDeleted == false)
                            .OrderByDescending(rjp => rjp.Job.UpdateDate)
                    )
                    .ToListAsync();

                var totalCount = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();

                foreach (var item in data)
                {
                    var jobpost = _mapper.Map<RecruiterJobPostViewModel>(item);
                    listData.Add(jobpost);
                }
                return new RecruiterJobPostListViewModel
                {
                    TotalCount = totalCount,
                    JobPostList = listData
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<RecruiterJobPostViewModel> GetById(Guid request)
        {
            var data = await Entities.Include(jp => jp.Recruiter)
                                        .ThenInclude(rec => rec.UserAccount)
                                        .ThenInclude(uac => uac.UserInfo)
                                     .Include(jp => jp.Job)
                .FirstOrDefaultAsync(x => x.JobPostId == request 
                                && x.IsDeleted == false 
                                && x.Recruiter.IsDeleted == false);
            if (data != null)
            {
                var obj = _mapper.Map<RecruiterJobPostViewModel>(data);
                return await Task.FromResult(obj);
            }
            else
            {
                throw new KeyNotFoundException(ExceptionMessages.RecruiterJobPostNotFound);
            }
        }
        public async Task<Guid> GetJobId(Guid request)
        {
            var data = await Entities.Include(jp => jp.Recruiter)
                                        .ThenInclude(rec => rec.UserAccount)
                                        .ThenInclude(uac => uac.UserInfo)
                                     .Include(jp => jp.Job)
                              .FirstOrDefaultAsync(x => x.JobPostId == request 
                                && x.IsDeleted == false
                                && x.Recruiter.IsDeleted == false
                           );
            if (data != null)
            {
                return await Task.FromResult(data.JobId);
            }
            else
            {
                throw new KeyNotFoundException(ExceptionMessages.RecruiterJobPostNotFound);
            }
        }
        public async Task<bool> Update(Guid postId, RecruiterJobPostModel request)
        {
            var data = await Entities.FindAsync(postId) ?? throw new KeyNotFoundException(ExceptionMessages.RecruiterJobPostNotFound); ;
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
            var data = await Entities.FirstOrDefaultAsync(x => x.JobPostId == postId && x.IsDeleted == false)
                ?? throw new KeyNotFoundException(ExceptionMessages.RecruiterJobPostNotFound);
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
                    .GroupBy(rjp => rjp.RecruiterId)
                    .Select(group => group.FirstOrDefault(x => x.IsDeleted == false && x.Recruiter.IsDeleted == false))
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
            var data = await Entities.FirstOrDefaultAsync(x => x.JobPostId == postId)
                ?? throw new KeyNotFoundException(ExceptionMessages.RecruiterJobPostNotFound);
            if (data.IsDeleted == false) throw new AppException(ExceptionMessages.JobPostNotDeleted);
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

        public double GetPercentageAppear(string requirement, string skillSets)
        {
            string[] skillToCheck = skillSets.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int totalSkill = skillToCheck.Count();
            int numberAppear = 0;
            foreach (string skill in skillToCheck)
            {
                if (requirement.Contains(skill))
                {
                    numberAppear++;
                }
            }
            double percent = ((double) numberAppear / totalSkill) * 100;
            return Math.Round(percent, 2);
        }

        public async Task<RecruiterJobPostListViewModel> GetAllSuggestion(int page, int limit, string? skillSets)
        {
            try
            {
                if(skillSets == null)
                {
                    return new RecruiterJobPostListViewModel
                    {
                        TotalCount = 0,
                        JobPostList = new List<RecruiterJobPostViewModel>()
                    };
                }
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;

                var listData = new List<RecruiterJobPostViewModel>();
                var query = Entities.Include(jp => jp.Recruiter)
                                        .ThenInclude(rec => rec.UserAccount)
                                        .ThenInclude(uac => uac.UserInfo)
                                    .Include(jp => jp.Job);
                var data = await query.Where(rjp => rjp.IsDeleted == false && rjp.Recruiter.IsDeleted == false && rjp.Job.Requirement != null).ToListAsync();

                // check % của từng job với skillSets
                List<(RecruiterJobPost data, double percent)> listDataWithPercent = new List<(RecruiterJobPost data, double percent)>();
                foreach(var item in data)
                {
                    double percent = GetPercentageAppear(item.Job.Requirement, skillSets);
                    if(percent != 0)
                    {
                        listDataWithPercent.Add((item, percent));
                    }
                }
                // sắp xếp theo thứ tự giảm dần của percent
                listDataWithPercent.Sort((x, y) => y.percent.CompareTo(x.percent));

                var totalCount = listDataWithPercent.Count;
                listDataWithPercent = listDataWithPercent.Skip((page - 1) * limit).Take(limit).ToList();

                foreach (var item in listDataWithPercent)
                {
                    var jobpost = _mapper.Map<RecruiterJobPostViewModel>(item.data);
                    listData.Add(jobpost);
                }
                return new RecruiterJobPostListViewModel
                {
                    TotalCount = totalCount,
                    JobPostList = listData
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
