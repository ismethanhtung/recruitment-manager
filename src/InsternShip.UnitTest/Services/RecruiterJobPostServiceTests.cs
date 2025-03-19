using AutoMapper;
using Microsoft.Extensions.Configuration;
using InsternShip.Data;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.UnitTest.Services
{
    public class RecruiterJobServiceTests
    {
        private RecruiterJobPostService recruiterjobService;
        private IUnitOfWork unitOfWork;
        private IConfiguration configuration;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {

            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);

            var recruiterjob = new RecruiterJobPostViewModel()
            {
                JobPostId = new Guid(),
                RecruiterId = new Guid(),
                FirstName = "Test",
                LastName = "Test",
                Level = "Test",
                TypeName = "Test",
                JobName = "Test",
                Location = "Test",
                Description = "Test",
                Requirement = "Test",
                MinSalary = 0,
                MaxSalary = 0,
                Quantity = 0,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                EndDate = DateTime.Now,
                JobStatus = true
            };
            var recruiterl = new List<RecruiterJobPostViewModel>();
            recruiterl.Add(recruiterjob);
            var recruiterjobview = new RecruiterJobPostListViewModel()
            {
                TotalCount = 0,
                JobPostList = recruiterl
            };

            var mockIRecruiterJobPostRepository = new Mock<IRecruiterJobPostRepository>();
            var mockIRecruiterRepository = new Mock<IRecruiterRepository>();
            var mockIJobRepository = new Mock<IJobRepository>();
            var mockIApplicationRepository = new Mock<IApplicationRepository>();
            var mockIInterviewRepository = new Mock<IInterviewRepository>();
            var mockIInterviewSessionRepository = new Mock<IInterviewSessionRepository>();
            var mockIApplicationStatusRepository = new Mock<IApplicationStatusRepository>();
            var mockIApplicationStatusUpdateRepository = new Mock<IApplicationStatusUpdateRepository>();
            mockIRecruiterJobPostRepository.Setup(x => x.GetAll(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(recruiterjobview));
            mockIRecruiterJobPostRepository.Setup(x => x.GetAllSuggestion(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(recruiterjobview));
            mockIRecruiterJobPostRepository.Setup(x => x.Create(It.IsAny<RecruiterJobPostModel>())).Returns(Task.FromResult(true));
            mockIRecruiterJobPostRepository.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<RecruiterJobPostModel>())).Returns(Task.FromResult(true));
            mockIRecruiterJobPostRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockIRecruiterJobPostRepository.Setup(x => x.Restore(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            recruiterjobService = new RecruiterJobPostService(mockIRecruiterJobPostRepository.Object, 
                                                                  mockIRecruiterRepository.Object,
                                                                  mockIJobRepository.Object,
                                                                  mockIApplicationRepository.Object,
                                                                  mockIInterviewRepository.Object,
                                                                  mockIInterviewSessionRepository.Object,
                                                                  mockIApplicationStatusRepository.Object,
                                                                  mockIApplicationStatusUpdateRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await recruiterjobService.GetAll(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>());
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount);
        }

        [Test]
        public async Task Create_Test()
        {
            var result = await recruiterjobService.Create(It.IsAny<Guid>(), It.IsAny<CreateJobModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await recruiterjobService.Update(It.IsAny<Guid>(), It.IsAny<JobUpdateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await recruiterjobService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Restore_Test()
        {
            var result = await recruiterjobService.Restore(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
