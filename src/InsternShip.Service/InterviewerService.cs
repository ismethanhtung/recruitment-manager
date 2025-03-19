using InsternShip.Common;
using InsternShip.Common.Exceptions;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class InterviewerService : IInterviewerService
    {
        private readonly IInterviewerRepository _interviewerRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IInterviewSessionRepository _interviewSessionRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IQuestionBankRepository _questionBankRepository;
        public InterviewerService(IInterviewerRepository interviewerRepository, IUserAccountRepository userAccountRepository, 
            IInterviewSessionRepository interviewSessionRepository, IUserInfoRepository userInfoRepository, 
             IQuestionBankRepository questionBankRepository)
        {
            _interviewerRepository = interviewerRepository;
            _userAccountRepository = userAccountRepository;
            _interviewSessionRepository = interviewSessionRepository;
            _userInfoRepository = userInfoRepository;
            _questionBankRepository = questionBankRepository;
        }

        public async Task<bool> Delete(Guid interviewerId)
        {
            return await _interviewerRepository.Delete(interviewerId);
        }

        public async Task<InterviewerListViewModel> GetAll(string? search, int page, int limit, bool deleted)
        {
            return await _interviewerRepository.GetAll(search,page,limit,deleted);
        }

        public async Task<InterviewerViewModel> GetById(Guid interviewerId)
        {
            return await _interviewerRepository.GetById(interviewerId);
        }

        public async Task<bool> Create(CreateInterviewerModel request)
        {
            _ = await _userAccountRepository.GetById(request.UserId);
            return await _interviewerRepository.Create(request);
        }

        public async Task<bool> Update(Guid userId, InterviewerUserInfoUpdateModel request)
        {
            var user = await _userAccountRepository.GetById(userId) ?? throw new KeyNotFoundException(ExceptionMessages.UserNotFound);
            var inte = await _interviewerRepository.GetByUserId(userId) ?? throw new KeyNotFoundException(ExceptionMessages.InterviewerNotFound);

            var interviewerUpdate = request.InterviewerUpdate;
            var userInfoUpdate = request.UserInfoUpdate;
            _ = await _userInfoRepository.UpdateInfo(user.UserId, userInfoUpdate);
            return await _interviewerRepository.Update(inte.InterviewerId, interviewerUpdate);

        }
        public async Task<bool> Restore(Guid interviewerId)
        {
            //_ = await _userAccountRepository.GetById(interviewerId);
            return await _interviewerRepository.Restore(interviewerId);

        }
        public async Task<InterviewSessionListViewModel> GetAllSessions(Guid interviewerId,int page, int limit)
        {
            var res = await _interviewSessionRepository.GetAllInterviewSession(interviewerId, page, limit);
            return await Task.FromResult(res);
            
        }
        public async Task<bool> SaveScore(Guid interviewerId, Guid intervieweSessionId, UpdatedInterviewSessionModel request)
        {
            await _interviewerRepository.GetById(interviewerId);
            var session = await _interviewSessionRepository.GetById(intervieweSessionId);
            var totalPoint = await _questionBankRepository.GetScore((Guid)session.TestId);
            if (request.GivenScore > totalPoint) {
                throw new AppException("Score cannot be higher than totalscore of Test.");
            } else return await _interviewSessionRepository.SaveScore(interviewerId, intervieweSessionId, request);
        }
        public async Task<InterviewSessionListViewModel> CheckDuplicate(Guid interviewerId, DateTime startTime, DateTime endTime)
        {
            return await _interviewSessionRepository.CheckDuplicate(interviewerId, startTime, endTime);
        }
    }
}
