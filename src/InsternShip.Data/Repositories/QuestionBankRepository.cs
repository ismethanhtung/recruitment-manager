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
    public class QuestionBankRepository : Repository<QuestionBank>, IQuestionBankRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public QuestionBankRepository(RecruitmentDB dbContext, IUnitOfWork uow,IMapper mapper) : base(dbContext)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> Create(CreateQuestionBankModel request)
        {
            var qlist = await GetAll(request.TestId);
            if (qlist.Questions.Any(ql => ql.QuestionId == request.QuestionId)) throw new DuplicateException(ExceptionMessages.DuplicateQuestion);
            try
            {
                var newLink = new QuestionBank()
                {
                    TestId = request.TestId,
                    QuestionId = request.QuestionId,
                };
                Entities.Add(newLink);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(QuestionBankModel request)//by testId
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.QuestionBankId == request.QuestionBankId)
                ?? throw new KeyNotFoundException(ExceptionMessages.QBankNotFound);
            try
            {
                _uow.BeginTransaction();

                chosen.TestId = request.TestId;
                chosen.QuestionId = request.QuestionId;
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

        public async Task<IEnumerable<QuestionBankViewModel>> GetAll(string? request)
        {
            try
            {
                var listData = new List<QuestionBankViewModel>();
                var data = await Entities.Include(b => b.Test).Where(x=>x.Test.IsDeleted == false).ToListAsync();
                foreach (var item in data)
                {
                    var obj = new QuestionBankViewModel
                    {
                        TestId = item.TestId,
                        QuestionId = item.QuestionId,

                    };
                    listData.Add(obj);
                }
                return listData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> Delete(string request)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.QuestionBankId == new Guid(request)) 
                ?? throw new KeyNotFoundException(ExceptionMessages.QBankNotFound);
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

        public async Task<QuestionBankViewModel> GetById(string request)
        {
            var chosen = await Entities.Include(b => b.Test).FirstOrDefaultAsync(x => x.QuestionBankId == new Guid(request) && x.Test.IsDeleted) 
                ?? throw new KeyNotFoundException(ExceptionMessages.QBankNotFound);
            try {
                var obj = new QuestionBankViewModel
                {
                    QuestionId = chosen.QuestionId,
                    TestId = chosen.TestId,
                };
                return await Task.FromResult(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<QuestionBankDetailModel?> GetAll(Guid testid)
        {
            try
            {
                var data = await
                    (
                        Entities
                        .Include(qbank => qbank.Question)
                        .Include(qbank => qbank.Test)
                        .Where(x => x.TestId == testid && x.Test.IsDeleted == false)
                    ).Select
                    (x => new
                    {
                        x.Test,
                        x.Question,
                    }
                )
                .ToListAsync();
                var obj = new QuestionBankDetailModel
                {
                };
                foreach (var item in data)
                {
                    obj.Test = item.Test;

                    if (item.Question != null) obj.Questions.Add(_mapper.Map<QuestionViewModel>(item.Question));
                }
                return await Task.FromResult<QuestionBankDetailModel?>(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<QuestionBankModel?> Get(string test_id, string question_id)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => (x.TestId == new Guid(test_id))&& x.Test.IsDeleted == false && (x.QuestionId == new Guid(question_id)))
                ?? throw new KeyNotFoundException(ExceptionMessages.QBankNotFound);
            try
            {
                var obj = new QuestionBankModel
                {
                    QuestionBankId = chosen.QuestionBankId,
                    QuestionId = chosen.QuestionId,
                    TestId = chosen.TestId,
                };
                return await Task.FromResult<QuestionBankModel?>(obj);
            }
            
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> GetScore(Guid testId)
        {
            int total = 0;
            try
            {
                var data = await
 
                        Entities
                        .Include(qbank => qbank.Question)
                        .Include(qbank => qbank.Test)
                        .Where(x=> x.TestId == testId /*&& x.Test.IsDeleted == false*/).ToListAsync();

                foreach (var item in data)
                {
                    if(item.Question.MaxScore!=null) total = (int)item.Question.MaxScore + total;
                }
                return await Task.FromResult(total);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<IEnumerable<TestViewModel>> GetTest(Guid questionId)
        { 
            List<TestViewModel> tests = new List<TestViewModel>();
            try
            {
                var data = await

                        Entities
                        .Include(qbank => qbank.Question)
                        .Include(qbank => qbank.Test)
                        .Where(x => x.QuestionId == questionId && x.Test.IsDeleted == false).ToListAsync();

                foreach (var item in data)
                {
                    if (item.Question.MaxScore != null) {
                        tests.Add(_mapper.Map<TestViewModel>(item.Test));
                    }
                }
                return await Task.FromResult(tests);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
