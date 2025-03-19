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
    public class InterviewSessionRepository : Repository<InterviewSession>, IInterviewSessionRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public InterviewSessionRepository(RecruitmentDB dbContext, IUnitOfWork uow, IMapper mapper) : base(dbContext)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<InterviewSessionListViewModel> GetAllInterviewSession(Guid interviewerId, int page, int limit)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                var listData = new List<InterviewSessionViewModel>();
                var query = Entities.Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.RecruiterJobPosts)
                                                .ThenInclude(rejp => rejp.Recruiter)
                                                    .ThenInclude(re => re.UserAccount)
                                                        .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app=>app.Candidate)
                                                .ThenInclude(can => can.UserAccount)
                                                    .ThenInclude(can => can.UserInfo)
                                    .Include(iss => iss.Interviewer)
                                        .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Test);
                var data = await query.Where(x=>x.InterviewerId==interviewerId 
                                    && x.IsDeleted == false
                                    && x.Interview.IsDeleted == false
                                    && x.Interview.Application.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                    && x.Interview.Application.Candidate.IsDeleted == false
                                    && x.Interviewer.IsDeleted == false
                                    && x.Test.IsDeleted == false
                ).ToListAsync();
                int count = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item  in data)
                {
                    var session = _mapper.Map<InterviewSessionViewModel>(item);
                    session.StartTime = item.Interview.StartTime;
                    session.EndTime = item.Interview.EndTime;
                    session.IsInterviewDeleted = item.Interview.IsDeleted;
                    listData.Add(session);
                }
                return new InterviewSessionListViewModel
                {
                    TotalCount = count,
                    InterviewSessionList = listData
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<InterviewSessionListViewModel> GetAllInterviewSession(Guid interviewerId)
        {
            try
            {
                var listData = new List<InterviewSessionViewModel>();
                var query = Entities.Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.RecruiterJobPosts)
                                                .ThenInclude(rejp => rejp.Recruiter)
                                                    .ThenInclude(re => re.UserAccount)
                                                        .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.Candidate)
                                                .ThenInclude(can => can.UserAccount)
                                                    .ThenInclude(can => can.UserInfo)
                                    .Include(iss => iss.Interviewer)
                                        .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Test);
                var data = await query.Where(x => x.InterviewerId == interviewerId
                                    && x.IsDeleted == false
                                    && x.Interview.IsDeleted == false
                                    && x.Interview.Application.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                    && x.Interview.Application.Candidate.IsDeleted == false
                                    && x.Interviewer.IsDeleted == false
                                    && x.Test.IsDeleted == false).ToListAsync();
                int count = data.Count;
                foreach (var item in data)
                {
                    var session = _mapper.Map<InterviewSessionViewModel>(item);
                    session.StartTime = item.Interview.StartTime;
                    session.EndTime = item.Interview.EndTime;
                    session.IsInterviewDeleted = item.Interview.IsDeleted;
                    listData.Add(session);
                }
                return new InterviewSessionListViewModel
                {
                    TotalCount = count,
                    InterviewSessionList = listData
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<InterviewSessionListViewModel> GetAllCandidateSession(Guid candidateId, int page, int limit)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                var listData = new List<InterviewSessionViewModel>();
                var query = Entities.Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.RecruiterJobPosts)
                                                .ThenInclude(rejp => rejp.Recruiter)
                                                    .ThenInclude(re => re.UserAccount)
                                                        .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.Candidate)
                                                .ThenInclude(can => can.UserAccount)
                                                    .ThenInclude(can => can.UserInfo)
                                    .Include(iss => iss.Interviewer)
                                        .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Test);
                var data = await query.Where(x => x.Interview.Application.CandidateId == candidateId
                                    && x.IsDeleted == false
                                    && x.Interview.IsDeleted == false
                                    && x.Interview.Application.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                    && x.Interview.Application.Candidate.IsDeleted == false
                                    && x.Interviewer.IsDeleted == false
                                    && x.Test.IsDeleted == false
                ).ToListAsync();
                int count = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var session = _mapper.Map<InterviewSessionViewModel>(item);
                    session.StartTime = item.Interview.StartTime;
                    session.EndTime = item.Interview.EndTime;
                    session.IsInterviewDeleted = item.Interview.IsDeleted;
                    listData.Add(session);
                }
                return new InterviewSessionListViewModel
                {
                    TotalCount = count,
                    InterviewSessionList = listData
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<InterviewSessionListViewModel> GetAllCandidateSession(Guid candidateId)
        {
            try
            {
                var listData = new List<InterviewSessionViewModel>();
                var query = Entities.Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.RecruiterJobPosts)
                                                .ThenInclude(rejp => rejp.Recruiter)
                                                    .ThenInclude(re => re.UserAccount)
                                                        .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.Candidate)
                                                .ThenInclude(can => can.UserAccount)
                                                    .ThenInclude(can => can.UserInfo)
                                    .Include(iss => iss.Interviewer)
                                        .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Test);
                var data = await query.Where(x => x.Interview.Application.CandidateId == candidateId
                                    && x.IsDeleted == false
                                    && x.Interview.IsDeleted == false
                                    && x.Interview.Application.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                    && x.Interview.Application.Candidate.IsDeleted == false
                                    && x.Interviewer.IsDeleted == false
                                    && x.Test.IsDeleted == false).ToListAsync();
                int count = data.Count;
                foreach (var item in data)
                {
                    var session = _mapper.Map<InterviewSessionViewModel>(item);
                    session.StartTime = item.Interview.StartTime;
                    session.EndTime = item.Interview.EndTime;
                    session.IsInterviewDeleted = item.Interview.IsDeleted;
                    listData.Add(session);
                }
                return new InterviewSessionListViewModel
                {
                    TotalCount = count,
                    InterviewSessionList = listData
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<InterviewSessionViewModel> GetById(Guid interviewSessionId)
        {
            try
            {
                var query = Entities.Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.RecruiterJobPosts)
                                                .ThenInclude(rejp => rejp.Recruiter)
                                                    .ThenInclude(re => re.UserAccount)
                                                        .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.Candidate)
                                                .ThenInclude(can => can.UserAccount)
                                                    .ThenInclude(can => can.UserInfo)
                                    .Include(iss => iss.Interviewer)
                                        .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Test);
                var chosen = await query.FirstOrDefaultAsync(x => x.InterviewSessionId == interviewSessionId
                                    && x.IsDeleted == false
                                    && x.Interview.IsDeleted == false
                                    && x.Interview.Application.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                    && x.Interview.Application.Candidate.IsDeleted == false
                                    && x.Interviewer.IsDeleted == false
                                    && x.Test.IsDeleted == false)
                    ?? throw new KeyNotFoundException(ExceptionMessages.InterviewSessionNotFound);
                var session = _mapper.Map<InterviewSessionViewModel>(chosen);
                session.StartTime = chosen.Interview.StartTime;
                session.EndTime = chosen.Interview.EndTime;
                session.IsInterviewDeleted = chosen.Interview.IsDeleted;
                return await Task.FromResult<InterviewSessionViewModel>(session);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<bool> Create(CreateInterviewSessionModel request)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.InterviewerId == request.InterviewerId && x.InterviewId == request.InterviewId);
            if (chosen != null) throw new DuplicateException(ExceptionMessages.DuplicateSession);
            try
            {
                var obj = new InterviewSession(){
                    InterviewId = request.InterviewId,
                    InterviewerId = request.InterviewerId,
                    TestId = request.TestId
                };
                Entities.Add(obj);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Delete(Guid interviewSessionId)
        {
            var chosen = await Entities.Include(x => x.Interview).Include(x=> x.Interviewer).Include(x => x.Test).FirstOrDefaultAsync(x=>x.InterviewSessionId == interviewSessionId)
                ?? throw new KeyNotFoundException(ExceptionMessages.InterviewSessionNotFound);
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

        public async Task<bool> Update(Guid interviewSessionId, UpdatedInterviewSessionModel request)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.InterviewSessionId == interviewSessionId)
                ?? throw new KeyNotFoundException(ExceptionMessages.InterviewSessionNotFound);
            try
            {
                _uow.BeginTransaction();
                chosen.Note = request.Note;
                chosen.GivenScore = request.GivenScore;
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
        public async Task<bool> SaveScore(Guid interviewerId, Guid interviewSessionId, UpdatedInterviewSessionModel request)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.InterviewSessionId == interviewSessionId)
                ?? throw new KeyNotFoundException(ExceptionMessages.InterviewSessionNotFound);
            if (chosen.InterviewerId != interviewerId) throw new AppException(ExceptionMessages.NotOwnInterview);
            try
            {
                _uow.BeginTransaction();
                chosen.GivenScore = request.GivenScore;
                chosen.Note = request.Note;
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
        public async Task<IEnumerable<InterviewSession>> GetAllSessionOfInterview(Guid interviewId)
        {
            try
            {
                var listSession = await Entities.Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.RecruiterJobPosts)
                                                .ThenInclude(rejp => rejp.Recruiter)
                                                    .ThenInclude(re => re.UserAccount)
                                                        .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.Candidate)
                                                .ThenInclude(can => can.UserAccount)
                                                    .ThenInclude(can => can.UserInfo)
                                    .Include(iss => iss.Interviewer)
                                        .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Test).Where(x=>x.InterviewId == interviewId 
                                    && x.IsDeleted == false
                                    && x.Interview.IsDeleted == false
                                    && x.Interview.Application.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.Recruiter.UserAccount.EmailConfirmed == true
                                    && x.Interview.Application.Candidate.UserAccount.EmailConfirmed == true
                                    && x.Interview.Application.Candidate.IsDeleted == false
                                    && x.Interviewer.IsDeleted == false
                                    && x.Interviewer.UserAccount.EmailConfirmed == true
                                    && x.Test.IsDeleted == false).ToListAsync();
                return listSession;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Guid> GetTestId(Guid interviewssId)
        {
            var res = await Entities.Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.RecruiterJobPosts)
                                                .ThenInclude(rejp => rejp.Recruiter)
                                                    .ThenInclude(re => re.UserAccount)
                                                        .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Interview)
                                        .ThenInclude(itv => itv.Application)
                                            .ThenInclude(app => app.Candidate)
                                                .ThenInclude(can => can.UserAccount)
                                                    .ThenInclude(can => can.UserInfo)
                                    .Include(iss => iss.Interviewer)
                                        .ThenInclude(inv => inv.UserAccount)
                                                    .ThenInclude(uc => uc.UserInfo)
                                    .Include(iss => iss.Test).FirstOrDefaultAsync(x => x.InterviewSessionId == interviewssId && x.TestId != null
                                    && x.IsDeleted == false
                                    && x.Interview.IsDeleted == false
                                    && x.Interview.Application.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.IsDeleted == false
                                    && x.Interview.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                                    && x.Interview.Application.Candidate.IsDeleted == false
                                    && x.Interviewer.IsDeleted == false
                                    && x.Test.IsDeleted == false)
                ?? throw new KeyNotFoundException(ExceptionMessages.TestNotFound);

            try
            {
                return await Task.FromResult((Guid)res.TestId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<InterviewSessionListViewModel> CheckDuplicate(Guid interviewerId, DateTime startTime, DateTime endTime)
        {
            try
            {
                var listData = await GetAllInterviewSession(interviewerId);
                var result = new List<InterviewSessionViewModel>();
                foreach (var item in listData.InterviewSessionList)
                {
                    if ((startTime >= item.StartTime && startTime <= item.EndTime) || endTime >= item.StartTime && endTime <= item.EndTime)
                    {
                        result.Add(item);
                    }
                }
                return new InterviewSessionListViewModel
                {
                    TotalCount = result.Count,
                    InterviewSessionList = result
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<InterviewSessionDetailViewModel> GetDetail(InterviewInfoViewModel interviewInfo, 
           AllInfoUser candidateFullInfo, List<InterviewSessionBaseListViewModel> listInfoSessions)
        {
            try
            {
                var interviewSessionInfo = new InterviewSessionInfoViewModel
                {
                    TotalCount = listInfoSessions.Count,
                    ListInfoSessions = listInfoSessions
                };
                var interviewSessionDetail = new InterviewSessionDetailViewModel
                {
                    InterviewInfo = interviewInfo,
                    CandidateInfo = candidateFullInfo,
                    InterviewSessions = interviewSessionInfo
                };
                return await Task.FromResult(interviewSessionDetail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
