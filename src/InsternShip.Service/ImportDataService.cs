using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.Service
{
    public class ImportDataService : IImportDataService
    {
        private readonly IImportDataRepository _importDataRepository;
        private readonly ISeedRepository _seedRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IInterviewerRepository _interviewerRepository;
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IRecruiterJobPostRepository _recruiterJobPostRepository;
        private readonly IRecruiterEventPostRepository _recruiterEventPostRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        public ImportDataService(IImportDataRepository importDataRepository, ISeedRepository seedRepository,
            ICandidateRepository candidateRepository, IInterviewerRepository interviewerRepository,
            IRecruiterRepository recruiterRepository, IJobRepository jobRepository, 
            IEventRepository eventRepository, IRecruiterJobPostRepository recruiterJobPostRepository, 
            IRecruiterEventPostRepository recruiterEventPostRepository, IUserAccountRepository userAccountRepository)
        {
            _importDataRepository = importDataRepository;
            _seedRepository = seedRepository;
            _candidateRepository = candidateRepository;
            _interviewerRepository = interviewerRepository;
            _recruiterRepository = recruiterRepository;
            _jobRepository = jobRepository;
            _eventRepository = eventRepository;
            _recruiterJobPostRepository = recruiterJobPostRepository;
            _recruiterEventPostRepository = recruiterEventPostRepository;
            _userAccountRepository = userAccountRepository;
        }

        public async Task<bool> ImportDataUser(IFormFile file)
        {
            bool result = false;
            bool createdUser = false;
            var listData = await _importDataRepository.GetDataUser(file);
            int lengthListCan = 0;
            int lengthListInter = 0;
            int lengthListRec = 0;
            for (int i = 0; i < listData.ListUser.Count; i++)
            {
                var userData = listData.ListUser[i];
                var user = await _seedRepository.CreateUser(userData);
                var userId = await _seedRepository.AddUserToRole(userData.Email, userData.Role);
                if (userData.Role.ToUpper().Equals("CANDIDATE"))
                {
                    var candidateData = listData.ListCan[lengthListCan++];
                    candidateData.UserId = userId;
                    createdUser = await _candidateRepository.Create(candidateData);
                }
                else if (userData.Role.ToUpper().Equals("INTERVIEWER"))
                {
                    var interviewerData = listData.ListInter[lengthListInter++];
                    interviewerData.UserId = userId;
                    createdUser = await _interviewerRepository.Create(interviewerData);
                }
                else if (userData.Role.ToUpper().Equals("RECRUITER"))
                {
                    var recData = listData.listRec[lengthListRec++];
                    recData.UserId = userId;
                    createdUser = await _recruiterRepository.Create(recData);
                }
                else { createdUser = true; }

                if (user.Succeeded && userId != Guid.Empty && createdUser == true)
                {
                    result = true;
                }
            }
            return await Task.FromResult(result);
        }

        public async Task<bool> ImportDataJobPost(IFormFile file)
        {
            var listData = await _importDataRepository.GetDataJobPost(file);
            bool result = false;
            foreach(var data in listData)
            {
                var jobId = await _jobRepository.CreateGUID(data.JobData);
                var userId = _userAccountRepository.GetUserIdByEmail(data.RecEmail);
                var recId = await _recruiterRepository.GetIdByUserId(userId);
                var rjp = new RecruiterJobPostModel()
                {
                    RecruiterId = (Guid)recId,
                    JobId = jobId
                };
                var createRjp = await _recruiterJobPostRepository.Create(rjp);
                if(jobId != Guid.Empty && createRjp == true) { result = true; }
            }
            return await Task.FromResult(result);
        }

        public async Task<bool> ImportDataEventPost(IFormFile file)
        {
            var listData = await _importDataRepository.GetDataEventPost(file);
            bool result = false;
            foreach (var data in listData)
            {
                var eventId = await _eventRepository.CreateGUID(data.EventData);
                var userId = _userAccountRepository.GetUserIdByEmail(data.RecEmail);
                var recId = await _recruiterRepository.GetIdByUserId(userId);
                var rep = new RecruiterEventPostModel()
                {
                    RecruiterId = (Guid)recId,
                    EventId = eventId
                };
                var createRep = await _recruiterEventPostRepository.Create(rep);
                if (eventId != Guid.Empty && createRep == true) { result = true; }
            }
            return await Task.FromResult(result);
        }
    }
}
