using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.UnitTest.Services
{
    public class CandidateServiceTests
    {
        private CandidateService candidateService;

        [SetUp]
        public void Setup()
        {

            var candidateview = new CandidateViewModel()
            {
                UserId = new Guid(),
                CandidateId = new Guid(),
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                PhoneNumber = "Test",
                Description = "Test",
                Education = "Test",
                Experience = "Test",
                Language = "Test",
                Skillsets = "Test",
                Avatar = "Test",
                CV = "Test"
            };

            var candidateL = new List<CandidateViewModel>();
            candidateL.Add(candidateview);
            var candidatelist = new CandidateListViewModel()
            {
                TotalCount = 0,
                CandidateList = candidateL
            };
            
            var mockICandidateRepository = new Mock<ICandidateRepository>();
            var mockIUserAccountRepository = new Mock<IUserAccountRepository>();
            var mockICVRepository = new Mock<ICVRepository>();
            var mockIEventParticipationRepository = new Mock<IEventParticipationRepository>();
            var mockIRecruiterEventPostRepository = new Mock<IRecruiterEventPostRepository>();
            var mockIEventRepository = new Mock<IEventRepository>();
            var mockIUserInfoRepository = new Mock<IUserInfoRepository>();
            var mockIRecruiterJobPostRepository = new Mock<IRecruiterJobPostRepository>();
            var mockIInterviewSessionRepository = new Mock<IInterviewSessionRepository>();
            mockICandidateRepository.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.FromResult(candidatelist));
            mockICandidateRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(candidateview));
            mockICandidateRepository.Setup(x => x.Create(It.IsAny<CreateCandidateModel>())).Returns(Task.FromResult(true));
            mockICandidateRepository.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<CandidateUpdateModel>())).Returns(Task.FromResult(true));
            mockICandidateRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockICandidateRepository.Setup(x => x.Restore(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            candidateService = new CandidateService(mockICandidateRepository.Object, 
                                                    mockIUserAccountRepository.Object,
                                                    mockICVRepository.Object,
                                                    mockIEventParticipationRepository.Object,
                                                    mockIRecruiterEventPostRepository.Object,
                                                    mockIEventRepository.Object,
                                                    mockIUserInfoRepository.Object,
                                                    mockIRecruiterJobPostRepository.Object,
                                                    mockIInterviewSessionRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await candidateService.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());
            Assert.IsNotNull(result.CandidateList.ToList()[0].UserId);
            Assert.IsNotNull(result.CandidateList.ToList()[0].CandidateId);
            Assert.AreEqual("Test", result.CandidateList.ToList()[0].FirstName);
            Assert.AreEqual("Test", result.CandidateList.ToList()[0].LastName);
            Assert.AreEqual("Test", result.CandidateList.ToList()[0].Email);
            Assert.AreEqual("Test", result.CandidateList.ToList()[0].PhoneNumber);
            Assert.AreEqual("Test", result.CandidateList.ToList()[0].Description);
            Assert.AreEqual("Test", result.CandidateList.ToList()[0].Education);
            Assert.AreEqual("Test", result.CandidateList.ToList()[0].Experience);
            Assert.AreEqual("Test", result.CandidateList.ToList()[0].Language);
            Assert.AreEqual("Test", result.CandidateList.ToList()[0].Skillsets);
            Assert.AreEqual("Test", result.CandidateList.ToList()[0].Avatar);
            Assert.AreEqual("Test", result.CandidateList.ToList()[0].CV);
        }

        [Test]
        public async Task GetById_Test()
        {
            var result = await candidateService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result.UserId);
            Assert.IsNotNull(result.CandidateId);
            Assert.AreEqual("Test", result.FirstName);
            Assert.AreEqual("Test", result.LastName);
            Assert.AreEqual("Test", result.Email);
            Assert.AreEqual("Test", result.PhoneNumber);
            Assert.AreEqual("Test", result.Description);
            Assert.AreEqual("Test", result.Education);
            Assert.AreEqual("Test", result.Experience);
            Assert.AreEqual("Test", result.Language);
            Assert.AreEqual("Test", result.Skillsets);
            Assert.AreEqual("Test", result.Avatar);
            Assert.AreEqual("Test", result.CV);
        }

        [Test]
        public async Task Create_Test()
        {
            var candidate = new CreateCandidateModel()
            {
                UserId = new Guid()
            };
            var result = await candidateService.Create(candidate);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var candidate = new CandidateUserInfoUpdateModel()
            {
                UserInfoUpdate = new UserInfoUpdateModel(),
                CandidateUpdate = new CandidateUpdateModel()
            };
            var result = await candidateService.Update(It.IsAny<Guid>(), candidate);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await candidateService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Restore_Test()
        {
            var result = await candidateService.Restore(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
