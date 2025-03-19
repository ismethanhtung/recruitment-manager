using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class QuestionBankService : IQuestionBankService
    {
        private readonly IQuestionBankRepository _qbankRepository;
        public QuestionBankService(IQuestionBankRepository qbankRepository)
        {
            _qbankRepository = qbankRepository;
        }

        public async Task<bool> Create(CreateQuestionBankModel request)
        {
            return await _qbankRepository.Create(request);
        }

        public async Task<bool> Update(QuestionBankModel request)
        {
            return await _qbankRepository.Update(request);
        }

        public async Task<IEnumerable<QuestionBankViewModel>> GetAll(string? request)
        {
            return await _qbankRepository.GetAll(request);
        }

        public async Task<bool> Delete(string request)
        {
            return await _qbankRepository.Delete(request);
        }

        public async Task<QuestionBankViewModel?> GetById(string request)
        {
            return await _qbankRepository.GetById(request);
        }
        public async Task<QuestionBankModel?> Get(string test_id, string question_id)
        {
            return await _qbankRepository.Get(test_id, question_id);
        }
        public async Task<QuestionBankDetailModel?> GetAll(Guid request)
        {
            return await _qbankRepository.GetAll(request);
        }
    }
}
