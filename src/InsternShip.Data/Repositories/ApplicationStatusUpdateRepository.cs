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
    public class ApplicationStatusUpdateRepository : Repository<ApplicationStatusUpdate>, IApplicationStatusUpdateRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ApplicationStatusUpdateRepository(RecruitmentDB dbContext, IUnitOfWork uow, IMapper mapper) : base(dbContext)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationStatusUpdateViewModel>> GetAllApplicationStatusUpdate()
        {
            try
            {
                var listData = new List<ApplicationStatusUpdateViewModel>();
                var data = await Entities.ToListAsync();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<ApplicationStatusUpdateViewModel>(item);
                    listData.Add(obj);
                }
                return listData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ApplicationStatusUpdateViewModel>> GetAllByApplicationId(Guid request)
        {
            try
            {
                var listData = new List<ApplicationStatusUpdateViewModel>();
                var data = await Entities.Where(x => x.ApplicationId == request).ToListAsync();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<ApplicationStatusUpdateViewModel>(item);
                    listData.Add(obj);
                }
                return listData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApplicationStatusUpdateViewModel> GetByApplicationIdAndStatus(Guid applicationRequest, Guid statusRequest)
        {
            var chosen = await Entities.OrderByDescending(x => x.LatestUpdate).Where(x => x.ApplicationId == applicationRequest && x.StatusId == statusRequest).FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException(ExceptionMessages.StatusUpdateNotFound);
            try {
                var obj = _mapper.Map<ApplicationStatusUpdateViewModel>(chosen);
                return await Task.FromResult<ApplicationStatusUpdateViewModel>(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApplicationStatusUpdateViewModel> GetById(Guid request)
        {
            var chosen = await Entities.Include(asu => asu.Status).FirstOrDefaultAsync(x => x.ApplicationStatusUpdateId == request)
                   ?? throw new KeyNotFoundException(ExceptionMessages.StatusUpdateNotFound);
            try
            {
                var obj = _mapper.Map<ApplicationStatusUpdateViewModel>(chosen);
                return await Task.FromResult(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApplicationStatusUpdateModel> GetByApplicationId(Guid request)
        {

            //var chosen = Entities.First(x => x.ApplicationId == request && x.LatestUpdate == ));
            var chosen = await Entities.Include(asu => asu.Status).OrderByDescending(x => x.LatestUpdate).FirstOrDefaultAsync(x => x.ApplicationId == request)
                ?? throw new KeyNotFoundException(ExceptionMessages.StatusUpdateNotFound);
            try
            {
                var obj = _mapper.Map<ApplicationStatusUpdateModel>(chosen);
                return await Task.FromResult(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> Delete(Guid request)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.ApplicationStatusUpdateId == request)
                ?? throw new KeyNotFoundException(ExceptionMessages.StatusUpdateNotFound);
            try
            {
                
                Entities.Remove(chosen);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Create(ApplicationStatusUpdateCreateModel request)
        {
            try
            {
                var newApplication = _mapper.Map<ApplicationStatusUpdate>(request);
                newApplication.LatestUpdate = DateTime.Now;
                Entities.Add(newApplication);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(ApplicationStatusUpdateModel request)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.ApplicationId == request.ApplicationId)
                ?? throw new KeyNotFoundException(ExceptionMessages.StatusUpdateNotFound);
            try
            {
                _uow.BeginTransaction();

                chosen.StatusId = request.StatusId;
                //chosen.ApplicationId = request.ApplicationId;
                chosen.LatestUpdate = DateTime.Now;
                Entities.Update(chosen);

                _uow.SaveChanges();
                _uow.CommitTransaction();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> AcceptApplication(Guid appid, Guid recId, Guid statusId)
        {
            var query = Entities.Include(asu => asu.Application)
                                    .ThenInclude(app => app.Candidate)
                                        .ThenInclude(can => can.UserAccount)
                                            .ThenInclude(uc => uc.UserInfo)
                                .Include(asu => asu.Application)
                                    .ThenInclude(jp => jp.RecruiterJobPosts)
                                        .ThenInclude(rec => rec.Recruiter)
                                            .ThenInclude(can => can.UserAccount)
                                                .ThenInclude(uc => uc.UserInfo)
                                .Include(asu => asu.Status);

            var chosen = await query.OrderByDescending(x=> x.LatestUpdate).FirstOrDefaultAsync(x => x.ApplicationId == appid
                                && x.Application.IsDeleted == false
                                && x.Application.Candidate.IsDeleted == false
                                && x.Application.RecruiterJobPosts.IsDeleted == false
                                && x.Application.RecruiterJobPosts.Recruiter.IsDeleted == false) ?? throw new KeyNotFoundException(ExceptionMessages.ApplicationNotFound);

            if (!(chosen.Status.Description.ToLower().Contains("interviewed") || chosen.Status.Description.ToLower().Contains("rejected"))) throw new AppException(ExceptionMessages.NotInterviewed);

            if (chosen.Application.RecruiterJobPosts.RecruiterId != recId) throw new AppException(ExceptionMessages.NotOwnPost);

            return await Create(new ApplicationStatusUpdateCreateModel
            {
                ApplicationId = appid,
                StatusId = statusId
            });

        }
        public async Task<bool> RejectApplication(Guid appid, Guid recId, Guid statusId)
        {
            var query = Entities.Include(asu => asu.Application)
                                    .ThenInclude(app => app.Candidate)
                                        .ThenInclude(can => can.UserAccount)
                                            .ThenInclude(uc => uc.UserInfo)
                                .Include(asu => asu.Application)
                                    .ThenInclude(jp => jp.RecruiterJobPosts)
                                        .ThenInclude(rec => rec.Recruiter)
                                            .ThenInclude(can => can.UserAccount)
                                                .ThenInclude(uc => uc.UserInfo)
                                .Include(asu => asu.Status);

            var chosen = await query.OrderByDescending(x => x.LatestUpdate).FirstOrDefaultAsync(x => x.ApplicationId == appid
                                && x.Application.IsDeleted == false
                                && x.Application.Candidate.IsDeleted == false
                                && x.Application.RecruiterJobPosts.IsDeleted == false
                                && x.Application.RecruiterJobPosts.Recruiter.IsDeleted == false) ?? throw new KeyNotFoundException(ExceptionMessages.ApplicationNotFound);
            
            
            if (chosen.Status.Description.ToLower().Contains("rejected")) throw new AppException(ExceptionMessages.NotInterviewed);

            if (chosen.Application.RecruiterJobPosts.RecruiterId != recId) throw new AppException(ExceptionMessages.NotOwnPost);


            return await Create(new ApplicationStatusUpdateCreateModel
            {
                ApplicationId = appid,
                StatusId = statusId
            });

        }
        public async Task<IEnumerable<CandidateReportModel>> GetCandidateReport()
        {
            try
            {
                var query = Entities.Include(asu => asu.Application)
                                        .ThenInclude(app => app.Candidate)
                                            .ThenInclude(can => can.UserAccount)
                                                .ThenInclude(usr => usr.UserInfo)
                                    .Include(asu => asu.Status);

                var data = await query.ToListAsync();

                var listData = new List<CandidateReportModel>();
                foreach (var item in data)
                {
                    listData.Add(new CandidateReportModel
                    {
                        ApplicationId = item.ApplicationId,
                        JobPostId = item.Application.JobPostId,
                        CandidateId = item.Application.CandidateId,
                        CandidateName = item.Application.Candidate.UserAccount.UserInfo.FirstName + " " + item.Application.Candidate.UserAccount.UserInfo.LastName,
                        Description = item.Application.Candidate.Description,
                        Education = item.Application.Candidate.Education,
                        Experience = item.Application.Candidate.Experience,
                        Language = item.Application.Candidate.Language,
                        Skillsets = item.Application.Candidate.Skillsets,
                        Status = item.Status.Description,
                        IsApplicationDeleted = item.Application.IsDeleted

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
