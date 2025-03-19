
using InsternShip.Common;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly ICVRepository _cvRepository;
        private readonly IRecruiterEventPostRepository _eventPostRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventParticipationRepository _participationRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IRecruiterJobPostRepository _recruiterJobPostRepository;
        private readonly IInterviewSessionRepository _interviewSessionRepository;

        public CandidateService(ICandidateRepository candidateRepository , 
            IUserAccountRepository userAccountRepository, ICVRepository cvRepository,
            IEventParticipationRepository participationRepository, IRecruiterEventPostRepository eventPostRepository,
            IEventRepository eventRepository, IUserInfoRepository userInfoRepository,
            IRecruiterJobPostRepository recruiterJobPostRepository,
            IInterviewSessionRepository interviewSessionRepository)
        {
            _candidateRepository = candidateRepository;
            _userAccountRepository = userAccountRepository;
            _cvRepository = cvRepository;
            _eventPostRepository = eventPostRepository;
            _participationRepository = participationRepository;
            _eventRepository = eventRepository;
            _userInfoRepository = userInfoRepository;
            _recruiterJobPostRepository = recruiterJobPostRepository;
            _interviewSessionRepository = interviewSessionRepository;
        }

        public async Task<bool> Create(CreateCandidateModel request)
        {
            _ = await _userAccountRepository.GetById(request.UserId);
            return await _candidateRepository.Create(request);

        }

        public async Task<CandidateListViewModel> GetAll(string? search, int page, int limit, bool deleted)
        {
            return await _candidateRepository.GetAll(search,page,limit, deleted);
        }

        public async Task<CandidateViewModel> GetById(Guid CandidateId)
        {
            var cv = await _cvRepository.GetByCanId(CandidateId);
            var urlCV = cv != null ? cv.UrlFile : "";
            return await _candidateRepository.GetById(CandidateId, urlCV);
        }
        public async Task<bool> JoinEvent(Guid candidateId, Guid eventpostId)
        {
            await _eventPostRepository.GetById(eventpostId);
            var cv = await _cvRepository.GetByCanId(candidateId);
            var urlCV = cv != null ? cv.UrlFile : "";
            await _candidateRepository.GetById(candidateId, urlCV);
            var evente = await _eventRepository.GetById(await _eventPostRepository.GetEventId(eventpostId));
            if (evente.DeadlineDate < DateTime.Now)
            {
                throw new KeyNotFoundException(ExceptionMessages.EventPostDeadlinePassed);
            }
            //var count = _participationRepository.CountByEventId(eventpostId);
            var rq = new EventParticipationCreateModel() { 
                EventPostId = eventpostId,
                CandidateId = candidateId,
            };
            return await _participationRepository.Create(rq, evente.MaxCandidate);
        }
        public async Task<InterviewSessionListViewModel> GetAllSessions(Guid candidateId, int page, int limit)
        {
            var res = await _interviewSessionRepository.GetAllCandidateSession(candidateId, page, limit);
            return await Task.FromResult(res);

        }

        public async Task<bool> CancleEvent(Guid CandidateId, Guid EventPostId)
        {
            return await _participationRepository.DeleteByCEid(CandidateId, EventPostId);
        }
        public async Task<EventPostParticipationListViewModel> GetAllEventOfCandidate(Guid CandidateId, string? search, int page, int limit)
        {
            return await _participationRepository.GetAllEventOfCandidate(CandidateId, search, page, limit);
        }
        public async Task<bool> Update(Guid userId, CandidateUserInfoUpdateModel request)
        {
            var user = await _userAccountRepository.GetById(userId);
            var can = await _candidateRepository.GetByUserId(userId);
            if (user == null || can == null)
                return false;
            
            var candidateUpdate = request.CandidateUpdate;
            var userInfoUpdate = request.UserInfoUpdate;
            _ = await _userInfoRepository.UpdateInfo(user.UserId, userInfoUpdate);
            return await _candidateRepository.Update(can.CandidateId, candidateUpdate);

        }

        public async Task<bool> Delete(Guid CandidateId)
        {
            return await _candidateRepository.Delete(CandidateId);
        }
        public async Task<bool> Restore(Guid CandidateId)
        {
            return await _candidateRepository.Restore(CandidateId);
        }

        public async Task<RecruiterJobPostListViewModel> SuggestionJobPost(Guid? candidateId, int page, int limit)
        {
            var skillSets = await _candidateRepository.GetSkillSetsById(candidateId);
            return await _recruiterJobPostRepository.GetAllSuggestion(page, limit, skillSets);
        }

        public async Task<bool> UpdateMyCV(Guid candidateId, CandidateUpdateModel request)
        {
            return await _candidateRepository.Update(candidateId, request);
        }

        public async Task<CandidateUpdateModel> GetMyCV(Guid candidateId)
        {
            return await _candidateRepository.GetMyCV(candidateId);
        }
    }
}
