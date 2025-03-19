using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;
using System.Text.Json;

namespace InsternShip.Service
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IInterviewerRepository _interviewerRepository;
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly ICVRepository _cvRepository;
        private readonly IUserAccountRepository _userAccountRepository;

        public UserInfoService(IUserInfoRepository userInfoRepository, ICandidateRepository candidateRepository,
            IInterviewerRepository interviewerRepository, IRecruiterRepository recruiterRepository,
            ICVRepository cvRepository, IUserAccountRepository userAccountRepository)
        {
            _userInfoRepository = userInfoRepository;
            _candidateRepository = candidateRepository;
            _interviewerRepository = interviewerRepository;
            _recruiterRepository = recruiterRepository;
            _cvRepository = cvRepository;
            _userAccountRepository = userAccountRepository;
        }
        public async Task<AllInfoUser> GetMyInfo(Guid userId, Guid currentUserId)
        {
            var checkCan = await _candidateRepository.GetByUserId(userId);
            var checkInter = await _interviewerRepository.GetByUserId(userId);
            var checkRec = await _recruiterRepository.GetByUserId(userId);
            var role = await _userAccountRepository.GetRoleByUserId(userId);
            object infoCurrentUser = new object();

            if (checkCan != null)
            {
                var cv = await _cvRepository.GetByCanId(currentUserId);
                var urlCV = cv != null ? cv.UrlFile : "";
                var can = await _candidateRepository.GetById(currentUserId, urlCV);
                infoCurrentUser = can; //await _canrepo.GetInfoRole(role,curr);
            }
            else if (checkInter != null)
            {
                var inter = await _interviewerRepository.GetById(currentUserId);
                infoCurrentUser = inter;
            }
            else if (checkRec != null)
            {
                var rec = await _recruiterRepository.GetById(currentUserId);
                infoCurrentUser = rec;
            }
            else 
            { 
                var email = await _userAccountRepository.GetEmailByUserId(userId);
                var id = _userAccountRepository.GetUserIdByEmail(email);
                infoCurrentUser = new { userId = id, email };
            }

            object infoUser = await _userInfoRepository.GetById(userId);
            var allInfoUser = new AllInfoUser
            {
                Role = role,
                InfoCurrentUser = infoCurrentUser,
                InfoUser = infoUser
            };
            return allInfoUser;
        }

        public async Task<AllInfoUser> GetInfoByUserId(Guid userId)
        {
            _ = await _userAccountRepository.GetById(userId);
            var canId = await _candidateRepository.GetIdByUserId(userId);
            var interId = await _interviewerRepository.GetIdByUserId(userId);
            var recId = await _recruiterRepository.GetIdByUserId(userId);
            var role = await _userAccountRepository.GetRoleByUserId(userId);

            object infoCurrentUser = new object();

            if (canId != null)
            {
                var cv = await _cvRepository.GetByCanId(canId);
                var urlCV = cv != null ? cv.UrlFile : "";
                var can = await _candidateRepository.GetById(canId, urlCV);
                infoCurrentUser = can;
            }
            else if (interId != null)
            {
                var inter = await _interviewerRepository.GetById(interId);
                infoCurrentUser = inter;
            }
            else if (recId != null)
            {
                var rec = await _recruiterRepository.GetById(recId);
                infoCurrentUser = rec;
            }
            else 
            { 
                var email = await _userAccountRepository.GetEmailByUserId(userId);
                var id = _userAccountRepository.GetUserIdByEmail(email);
                infoCurrentUser = new { userId = id, email };
            }

            object infoUser = await _userInfoRepository.GetById(userId);
            var allInfoUser = new AllInfoUser
            {
                Role = role,
                InfoCurrentUser = infoCurrentUser,
                InfoUser = infoUser
            };
            return allInfoUser;
        }


        public async Task<bool> UpdateInfo(Guid userId, UserInfoUpdateModel info)
        {
            return await _userInfoRepository.UpdateInfo(userId, info);
        }

        public async Task<ResultUpdateInfo> UpdateInfoByUserId(Guid userId, AllInfoUserUpdate infoUpdate)
        {
            _ = await _userAccountRepository.GetById(userId);
            var canId = await _candidateRepository.GetIdByUserId(userId);
            var interId = await _interviewerRepository.GetIdByUserId(userId);
            var recId = await _recruiterRepository.GetIdByUserId(userId);

            object resultUpdate = new object();

            if (canId != null)
            {
                var infoCurrentUserJson = infoUpdate.InfoCurrentUser.ToString();
                var infoCurrentUser = JsonSerializer.Deserialize<CandidateUpdateModel>(infoCurrentUserJson);
                CandidateUpdateModel canInfoUpdate = new CandidateUpdateModel
                {
                    Description = infoCurrentUser.Description,
                    Education = infoCurrentUser.Education,
                    Experience = infoCurrentUser.Experience,
                    Language = infoCurrentUser.Language,
                    Skillsets = infoCurrentUser.Skillsets
                };
                resultUpdate = await _candidateRepository.Update(canId, canInfoUpdate);
            }
            else if (interId != null)
            {
                var infoCurrentUserJson = infoUpdate.InfoCurrentUser.ToString();
                var infoCurrentUser = JsonSerializer.Deserialize<InterviewerUpdateModel>(infoCurrentUserJson);
                InterviewerUpdateModel interInfoUpdate = new InterviewerUpdateModel
                {
                    Description = infoCurrentUser.Description,
                    UrlContact = infoCurrentUser.UrlContact
                };
                resultUpdate = await _interviewerRepository.Update(interId, interInfoUpdate);
            }
            else if (recId != null)
            {
                var infoCurrentUserJson = infoUpdate.InfoCurrentUser.ToString();
                var infoCurrentUser = JsonSerializer.Deserialize<RecruiterUpdateModel>(infoCurrentUserJson);
                RecruiterUpdateModel recInfoUpdate = new RecruiterUpdateModel 
                {
                    Description = infoCurrentUser.Description,
                    UrlContact = infoCurrentUser.UrlContact
                };
                resultUpdate = await _recruiterRepository.Update(recId, recInfoUpdate);
            }
            var infoUserJson = infoUpdate.InfoUser.ToString();
            var infoUser = JsonSerializer.Deserialize<UserInfoUpdateModel>(infoUserJson);
            UserInfoUpdateModel infoUserUpdate = new UserInfoUpdateModel
            {
                Avatar = infoUser.Avatar,
                FirstName = infoUser.FirstName,
                LastName = infoUser.LastName,
                Gender = infoUser.Gender,
                DateOfBirth = infoUser.DateOfBirth,
                PhoneNumber = infoUser.PhoneNumber,
                Location = infoUser.Location,
            };
            var resultUserUpdate = await _userInfoRepository.UpdateInfo(userId, infoUserUpdate);
            var result = new ResultUpdateInfo
            {
                ResultUpdate = resultUpdate,
                ResultUserUpdate = resultUserUpdate
            };
            return result;
        } 

    }
}

