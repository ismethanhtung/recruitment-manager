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
    public class ApplicationServiceTests
    {
        private ApplicationService applicationService;

        [SetUp]
        public void Setup()
        {
            var infotointerview = new InfoToInterviewModel()
            {
                Email = "Test",
                FirstName = "Test"
            };

            var applicationview = new ApplicationViewModel()
            {
                ApplicationId = new Guid(),
                CandidateId = new Guid(),
                JobPostId = new Guid(),
                CandidateName = "Test",
                Email = "Test",
                PhoneNumber = "Test",
                Status = "Test",
                AppliedPosition = "Test",
                ApplyDate = DateTime.Now
            };

            var applicationL = new List<ApplicationViewModel>();
            applicationL.Add(applicationview);

            var applicationlistview = new ApplicationListViewModel()
            {
                TotalCount = 0,
                ApplicationList = applicationL
            };
            var mockIApplicationRepository = new Mock<IApplicationRepository>();
            var mockIApplicationStatusUpdateRepository = new Mock<IApplicationStatusUpdateRepository>();
            var mockIApplicationStatusRepository = new Mock<IApplicationStatusRepository>();
            var mockICandidateRepository = new Mock<ICandidateRepository>();
            var mockIRecruiterJobPostRepository = new Mock<IRecruiterJobPostRepository>();
            var mockICVRepository = new Mock<ICVRepository>();
            var mockIInterviewRepository = new Mock<IInterviewRepository>();
            var mockIInterviewSessionsRepository = new Mock<IInterviewSessionRepository>();
            mockIApplicationRepository.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>())).Returns(Task.FromResult(applicationlistview));
            mockIApplicationRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(applicationview));
            mockIApplicationRepository.Setup(x => x.GetMailInfoByAppId(It.IsAny<Guid>())).Returns(Task.FromResult(infotointerview));
            mockIApplicationRepository.Setup(x => x.Create(It.IsAny<ApplicationCreateModel>())).Returns(Task.FromResult(false));
            mockIApplicationRepository.Setup(x => x.CreateGuid(It.IsAny<ApplicationCreateModel>())).Returns(Task.FromResult(new Guid()));
            mockIApplicationRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockIApplicationRepository.Setup(x => x.Restore(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            applicationService = new ApplicationService(mockIApplicationRepository.Object, 
                                                        mockIApplicationStatusUpdateRepository.Object, 
                                                        mockIApplicationStatusRepository.Object, 
                                                        mockICandidateRepository.Object,
                                                        mockIRecruiterJobPostRepository.Object,
                                                        mockICVRepository.Object,
                                                        mockIInterviewRepository.Object,
                                                        mockIInterviewSessionsRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await applicationService.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>());
            Assert.IsNotNull(result.ApplicationList.ToList()[0].ApplicationId);
            Assert.IsNotNull(result.ApplicationList.ToList()[0].CandidateId);
            Assert.IsNotNull(result.ApplicationList.ToList()[0].JobPostId);
            Assert.AreEqual("Test", result.ApplicationList.ToList()[0].CandidateName);
            Assert.AreEqual("Test", result.ApplicationList.ToList()[0].Email);
            Assert.AreEqual("Test", result.ApplicationList.ToList()[0].PhoneNumber);
            Assert.AreEqual("Test", result.ApplicationList.ToList()[0].Status);
            Assert.AreEqual("Test", result.ApplicationList.ToList()[0].AppliedPosition);
            Assert.IsNotNull(result.ApplicationList.ToList()[0].ApplyDate);
        }

        [Test]
        public async Task GetById_Test()
        {
            var result = await applicationService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result.ApplicationId);
            Assert.IsNotNull(result.CandidateId);
            Assert.IsNotNull(result.JobPostId);
            Assert.AreEqual("Test", result.CandidateName);
            Assert.AreEqual("Test", result.Email);
            Assert.AreEqual("Test", result.PhoneNumber);
            Assert.AreEqual("Test", result.Status);
            Assert.AreEqual("Test", result.AppliedPosition);
            Assert.IsNotNull(result.ApplyDate);
        }

        [Test]
        public async Task Create_Test()
        {
            try
            {
                var result = await applicationService.Create(new Guid(), new Guid());
                Assert.IsNotNull(result);
                Assert.AreEqual(false, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await applicationService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Restore_Test()
        {
            var result = await applicationService.Restore(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
