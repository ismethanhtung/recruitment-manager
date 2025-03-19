using AutoMapper;
using InsternShip.Common;
using InsternShip.Common.Exceptions;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace InsternShip.Data.Repositories
{
    public class TestRepository: Repository<Test>, ITestRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public TestRepository(RecruitmentDB context, IUnitOfWork uow, IMapper mapper) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<TestListViewModel> GetAll(int page, int limit, bool deleted)
        {
            try
            {
                var listData = new List<TestViewModel>();
                var data = await Entities.Include(t => t.Sessions)
                                            .ThenInclude(ss => ss.Interview)
                                                .ThenInclude(intv => intv.Application)
                                                    .ThenInclude(app => app.Candidate)
                                          .Include(t => t.Sessions)
                                            .ThenInclude(ss => ss.Interview)
                                                .ThenInclude(intv => intv.Application)
                                                    .ThenInclude(app => app.RecruiterJobPosts)
                                                        .ThenInclude(rjp=>rjp.Recruiter)
                                         .Include(t => t.Sessions)
                                            .ThenInclude(ss => ss.Interviewer)
                                         
                                                


                    .Where(x => x.IsDeleted == deleted 
                             && x.Sessions.IsDeleted == false
                             && x.Sessions.Interview.IsDeleted == false
                             && x.Sessions.Interview.Application.IsDeleted == false
                             && x.Sessions.Interview.Application.Candidate.IsDeleted == false
                             && x.Sessions.Interview.Application.RecruiterJobPosts.IsDeleted == false
                             && x.Sessions.Interview.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                             && x.Sessions.Interviewer.IsDeleted == false

                    ).ToListAsync();
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                int count = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var test = _mapper.Map<TestViewModel>(item);
                    listData.Add(test);
                }

                return new TestListViewModel
                {
                    TestList = listData,
                    TotalCount = count,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Create(CreateTestModel request)
        {
            try
            {
                var test = _mapper.Map<Test>(request);
                Entities.Add(test);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Guid> CreateGUID(CreateTestModel request)
        {
            try
            {
                var test = _mapper.Map<Test>(request);
                Entities.Add(test);
                _uow.SaveChanges();
                return await Task.FromResult(test.TestId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Delete(Guid testId)
        {
            var target = await Entities.Include(test => test.Sessions).Include(test => test.QuestionBanks).FirstOrDefaultAsync(x => x.TestId == testId && x.IsDeleted == false)
                ?? throw new KeyNotFoundException(ExceptionMessages.TestNotFound);
            try
            {
                target.IsDeleted = true;
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Update(Guid testId, TestUpdateModel request)
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.TestId == testId && x.IsDeleted == false) 
                ?? throw new KeyNotFoundException(ExceptionMessages.TestNotFound);
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
        public async Task<bool> Update(Guid testId, int point) // Add point after add question
        {
            var data = await Entities.FirstOrDefaultAsync(x => x.TestId == testId)
                ?? throw new KeyNotFoundException(ExceptionMessages.TestNotFound);
            try
            {
                _uow.BeginTransaction();
                data.TotalScore = point;
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
        public async Task<TestViewModel> GetById(Guid? testId)
        {
            var chosen = await Entities.Include(t => t.Sessions)
                                            .ThenInclude(ss => ss.Interview)
                                                .ThenInclude(intv => intv.Application)
                                                    .ThenInclude(app => app.Candidate)
                                          .Include(t => t.Sessions)
                                            .ThenInclude(ss => ss.Interview)
                                                .ThenInclude(intv => intv.Application)
                                                    .ThenInclude(app => app.RecruiterJobPosts)
                                                        .ThenInclude(rjp => rjp.Recruiter)
                                         .Include(t => t.Sessions)
                                            .ThenInclude(ss => ss.Interviewer)
                        .FirstOrDefaultAsync(x => x.TestId == testId 
                             && x.IsDeleted == false
                             && x.Sessions.IsDeleted == false
                             && x.Sessions.Interview.IsDeleted == false
                             && x.Sessions.Interview.Application.IsDeleted == false
                             && x.Sessions.Interview.Application.Candidate.IsDeleted == false
                             && x.Sessions.Interview.Application.RecruiterJobPosts.IsDeleted == false
                             && x.Sessions.Interview.Application.RecruiterJobPosts.Recruiter.IsDeleted == false
                             && x.Sessions.Interviewer.IsDeleted == false) ?? throw new KeyNotFoundException(ExceptionMessages.TestNotFound);
            try
            {
                var obj = _mapper.Map<TestViewModel>(chosen);
                //obj.QuestionId = questionId;
                return await Task.FromResult(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Restore(Guid testId)
        {
            var target = await Entities.Include(test => test.Sessions).Include(test => test.QuestionBanks).FirstOrDefaultAsync(x => x.TestId == testId)
                ?? throw new KeyNotFoundException(ExceptionMessages.TestNotFound);
            if (target.IsDeleted == false) throw new AppException(ExceptionMessages.TestNotDeleted);
            try
            {
                _uow.BeginTransaction();
                target.IsDeleted = false;
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
