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

    public class InterviewRepository : Repository<Interview>, IInterviewRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public InterviewRepository(RecruitmentDB dbContext, IUnitOfWork uow, IMapper mapper) : base(dbContext)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public List<DateTime> GetDaysInWeek()
        {
            DateTime today = DateTime.Today;
            int currentDayOfWeek = (int)today.DayOfWeek;
            DateTime sunday = today.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);
            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            return Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
        }

        public async Task<InterviewListViewModel> GetAll(int page, int limit, bool deleted)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                //search = string.IsNullOrEmpty(search) ? string.Empty : search;
                var query = Entities.Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Job)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Recruiter)
                                                .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.Candidate)
                                            .ThenInclude(can => can.UserAccount)
                                                .ThenInclude(acc => acc.UserInfo);

                var data = await query.Where(x => x.IsDeleted == deleted
                                               && x.Application.IsDeleted == false
                                               && x.Application.RecruiterJobPosts.IsDeleted == false
                                               && x.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                               && x.Application.Candidate.IsDeleted == false
                ).ToListAsync();

                var listData = new List<InterviewViewModel>();
                int count = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<InterviewViewModel>(item);
                    listData.Add(obj);
                };
                return new InterviewListViewModel
                {
                    InterviewList = listData,
                    TotalCount = count

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int[]> GetAllInWeekByRecId(Guid recId)
        {
            int[] numOfInterview = new int[7];
            try
            {
                var query = Entities.Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Recruiter)
                                                .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Job)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.Candidate)
                                            .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo);

                var daysOfWeek = GetDaysInWeek();
                foreach (var day in daysOfWeek)
                {
                    var data = await query.Where(intv => intv.Application.RecruiterJobPosts.RecruiterId == recId
                                && ((DateTime)intv.StartTime).Date == day.Date 
                                && intv.IsDeleted == false
                                && intv.Application.IsDeleted == false
                                && intv.Application.RecruiterJobPosts.IsDeleted == false
                                && intv.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                && intv.Application.Candidate.IsDeleted == false).ToListAsync();

                    if((int)day.DayOfWeek == 0)
                        numOfInterview[6] = data.Count;
                    else numOfInterview[(int)day.DayOfWeek -1] = data.Count;
                }
                return numOfInterview;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<InterviewViewModel> GetById(Guid request)
        {
            var chosen = await Entities.Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Job)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Recruiter)
                                                .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.Candidate)
                                            .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                              .FirstOrDefaultAsync(x => x.InterviewId == request 
                                               && x.IsDeleted == false
                                               && x.Application.IsDeleted == false
                                               && x.Application.RecruiterJobPosts.IsDeleted == false
                                               && x.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                               && x.Application.Candidate.IsDeleted == false)
                ?? throw new KeyNotFoundException(ExceptionMessages.InterviewNotFound);
            try
            {
                var obj = _mapper.Map<InterviewViewModel>(chosen);
                return await Task.FromResult<InterviewViewModel>(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<InterviewViewModel?> GetByApplicationId(Guid request)
        {
            var chosen = await Entities.Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Job)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Recruiter)
                                                .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.Candidate)
                                            .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                              .FirstOrDefaultAsync(x => x.ApplicationId == request
                                               && x.IsDeleted == false
                                               && x.Application.IsDeleted == false
                                               && x.Application.RecruiterJobPosts.IsDeleted == false
                                               && x.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                               && x.Application.Candidate.IsDeleted == false);
            if (chosen == null) return await Task.FromResult<InterviewViewModel?>(null);
            try
            {
                var obj = _mapper.Map<InterviewViewModel>(chosen);
                return await Task.FromResult<InterviewViewModel?>(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Create(CreateInterviewModel request)
        {
            var chose = await Entities.FirstOrDefaultAsync(x => x.ApplicationId == request.ApplicationId && x.IsDeleted == false);
            if (chose != null) throw new DuplicateException(ExceptionMessages.AlreadyInInterview);
            try
            {
                var newInterview = _mapper.Map<Interview>(request);
                Entities.Add(newInterview);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Guid> CreateGUID(CreateInterviewModel request)
        {
            var chose = await Entities.FirstOrDefaultAsync(x => x.ApplicationId == request.ApplicationId && x.IsDeleted == false);
            if (chose != null) throw new DuplicateException(ExceptionMessages.AlreadyInInterview);
            try
            {
                var newInterview = _mapper.Map<Interview>(request);
                Entities.Add(newInterview);
                _uow.SaveChanges();
                return await Task.FromResult(newInterview.InterviewId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Delete(Guid request)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.InterviewId == request && x.IsDeleted == false)
            ?? throw new DuplicateException(ExceptionMessages.InterviewNotFound);
            try
            {
                _uow.BeginTransaction();
                chosen.IsDeleted = true;
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
        public async Task<bool> Restore(Guid request)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.InterviewId == request) ?? throw new KeyNotFoundException(ExceptionMessages.InterviewNotFound);
            if (data.IsDeleted == false) throw new AppException(ExceptionMessages.CandidateNotDeleted);
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
        public async Task<bool> Update(Guid interviewId, InterviewUpdateModel request)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.InterviewId == interviewId && x.IsDeleted == false)
            ?? throw new DuplicateException(ExceptionMessages.InterviewNotFound);
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
        public async Task<int[]> GetAllInterviewInWeek()
        {
            int[] numOfInterview = new int[7];
            try
            {
                var query = Entities.Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Recruiter)
                                                .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Job)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.Candidate)
                                            .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo);

                var daysOfWeek = GetDaysInWeek();
                foreach (var day in daysOfWeek)
                {
                    var data = await query.Where(x => ((DateTime)x.StartTime).Date == day.Date 
                                               && x.IsDeleted == false
                                               && x.Application.IsDeleted == false
                                               && x.Application.RecruiterJobPosts.IsDeleted == false
                                               && x.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                               && x.Application.Candidate.IsDeleted == false).ToListAsync();
                    if ((int)day.DayOfWeek == 0)
                        numOfInterview[6] = data.Count;
                    else numOfInterview[(int)day.DayOfWeek - 1] = data.Count;
                }
                return numOfInterview;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> GetScheduleInterviewByRecruiter(Guid recId)
        {
            try
            {
                int count = 0;
                var data = await Entities.Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Recruiter)
                                                .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.RecruiterJobPosts)
                                            .ThenInclude(inap => inap.Job)
                                    .Include(intv => intv.Application)
                                        .ThenInclude(ina => ina.Candidate)
                                            .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                                .Where(x => x.Application.RecruiterJobPosts.Recruiter.RecruiterId == recId
                                               && x.IsDeleted == false
                                               && x.Application.IsDeleted == false
                                               && x.Application.RecruiterJobPosts.IsDeleted == false
                                               && x.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                               && x.Application.Candidate.IsDeleted == false).ToListAsync();
                count = data.Count;
                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<InterviewReportModel>> GetInterviewReport()
        {
            try
            {
                var query = Entities.Include(itv => itv.Application)
                                        .ThenInclude(app => app.RecruiterJobPosts)
                                            .ThenInclude(rejp => rejp.Recruiter)
                                                .ThenInclude(re => re.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(itv => itv.Application)
                                        .ThenInclude(app => app.RecruiterJobPosts)
                                            .ThenInclude(rejp => rejp.Job)
                                    .Include(itv => itv.Application)
                                        .ThenInclude(app => app.Candidate)
                                            .ThenInclude(can => can.UserAccount)
                                                .ThenInclude(can => can.UserInfo)
                                    .Include(itv => itv.Sessions);
                                    /*.Include(iss => iss.Interviewer)
                                        .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo);*/
                var data = await query.ToListAsync();
                //var data = await query.GroupBy(x => x.InterviewId).Select(x => x.First()).ToListAsync();
                //List<Guid> interviewIds = new List<Guid>();

                var listData = new List<InterviewReportModel>();
                foreach (var item in data)
                {
                    //var count = data.Sessions.Where(x => x.InterviewId == item.InterviewId).ToList().Count;
                    //var count = item.Select(x => x.Sessions.Where(y => y.InterviewId == x.InterviewId)).ToList().Count;
                    var count = item.Sessions.Count;
                    if(count != 0)
                    {
                        string type = "";
                        if (item.IsOnline)
                        {
                            type = "online";
                        }
                        else type = "offline";
                        listData.Add(new InterviewReportModel
                        {
                            InterviewId = item.InterviewId,
                            JobPostId = item.Application.JobPostId,
                            NumberOfInterviewer = count,
                            JobPosition = item.Application.RecruiterJobPosts.Job.Name,
                            CandidateName = item.Application.Candidate.UserAccount.UserInfo.LastName + " " + item.Application.Candidate.UserAccount.UserInfo.FirstName,
                            CandidateEmail = item.Application.Candidate.UserAccount.Email,
                            StartDate = item.StartTime,
                            EndDate = item.EndTime,
                            Method = type
                        });
                    }

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
