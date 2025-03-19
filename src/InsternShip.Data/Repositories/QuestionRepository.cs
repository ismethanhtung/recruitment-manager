using AutoMapper;
using InsternShip.Common;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace InsternShip.Data.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public QuestionRepository(RecruitmentDB context, IUnitOfWork uow, IMapper mapper) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> Create(CreateQuestionModel request)
        {
            if (request.Detail.IsNullOrEmpty()) throw new MissingFieldException(ExceptionMessages.FieldIsRequired + " (Detail)");
            try
            {
                var question = _mapper.Map<Question>(request);
                Entities.Add(question);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<QuestionViewModel> CreateFromTest(CreateQuestionModel request)
        {
            if (request.Detail.IsNullOrEmpty()) throw new MissingFieldException(ExceptionMessages.FieldIsRequired + " (Detail)");
            try
            {
                var question = _mapper.Map<Question>(request);
                Entities.Add(question);
                _uow.SaveChanges();
                var qvml = _mapper.Map<QuestionViewModel>(question); 
                return await Task.FromResult(qvml);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> Delete(Guid questionId)
        {
            var target = await Entities.Include(question => question.QuestionBanks).FirstOrDefaultAsync(x => x.QuestionId == questionId) 
                ?? throw new KeyNotFoundException(ExceptionMessages.QuestionNotFound);

            try
            {
                Entities.Remove(target);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<QuestionListViewModel> GetAll(string? search, string? tag, int? level, int page, int limit)
        {
            try
            {
                page = page != 0 ? page : 1;
                limit = limit != 0 ? limit : 10;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                var query = await Entities.Where(x => x.Detail != null && x.Detail.Contains(search)).ToListAsync();
                if (tag !=null) query = query.Where(x => x.Tag != null && x.Tag.Contains(tag)).ToList();
                if (level != null) query = query.Where(x => x.Level != null && x.Level == level).ToList();
                var data = query;
                var listData = new List<QuestionViewModel>();
                int count = data.Count;
                data = data.Skip((page - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<QuestionViewModel>(item);
                    listData.Add(obj);
                };
                return new QuestionListViewModel
                {
                    QuestionList = listData,
                    TotalCount = count

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<QuestionViewModel> GetById(Guid questionId)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.QuestionId == questionId) 
                ?? throw new KeyNotFoundException(ExceptionMessages.QuestionNotFound);
            try
            {
                var obj = _mapper.Map<QuestionViewModel>(chosen);
                //obj.QuestionId = questionId;
                return await Task.FromResult(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Update(Guid questionId, QuestionUpdateModel request)
        {
            var data = await Entities.FindAsync(questionId) 
                ?? throw new KeyNotFoundException(ExceptionMessages.QuestionNotFound);
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

    }
}
