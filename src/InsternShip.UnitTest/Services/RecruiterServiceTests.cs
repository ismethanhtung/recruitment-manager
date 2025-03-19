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
    public class RecruiterServiceTests
    {
        private RecruiterService recruiterService;
        private IUnitOfWork unitOfWork;
        private IConfiguration configuration;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {

            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);

            var recruiter = new RecruiterViewModel()
            {
                UserId = new Guid(),
                RecruiterId = new Guid(),
                Description = "Test",
                UrlContact = "Test",
            };
            var recruiterL = new List<RecruiterViewModel>();
            recruiterL.Add(recruiter);
            var recruiterlist = new RecruiterListViewModel()
            {
                TotalCount = 0,
                RecruiterList = recruiterL
            };
            var mockIRecruiterRepository = new Mock<IRecruiterRepository>();
            var mockIUserInfoRepository = new Mock<IUserInfoRepository>();
            var mockIUserAccountRepository = new Mock<IUserAccountRepository>();
            var mockIInterviewRepository = new Mock<IInterviewRepository>();
            var mockIApplicationRepository = new Mock<IApplicationRepository>();
            var mockIApplicationStatusRepository = new Mock<IApplicationStatusRepository>();
            var mockIApplicationStatusUpdateRepository = new Mock<IApplicationStatusUpdateRepository>();
            var mockIRecruiterJobPostRepository = new Mock<IRecruiterJobPostRepository>();
            mockIRecruiterRepository.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.FromResult(recruiterlist));
            mockIRecruiterRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(recruiter));
            mockIRecruiterRepository.Setup(x => x.Create(It.IsAny<CreateRecruiterModel>())).Returns(Task.FromResult(true));
            mockIRecruiterRepository.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<RecruiterUpdateModel>())).Returns(Task.FromResult(true));
            mockIRecruiterRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockIRecruiterRepository.Setup(x => x.Restore(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            recruiterService = new RecruiterService(mockIRecruiterRepository.Object, 
                                                    mockIUserInfoRepository.Object,
                                                    mockIUserAccountRepository.Object,
                                                    mockIInterviewRepository.Object,
                                                    mockIApplicationRepository.Object,
                                                    mockIApplicationStatusRepository.Object,
                                                    mockIApplicationStatusUpdateRepository.Object,
                                                    mockIRecruiterJobPostRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await recruiterService.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.RecruiterList.ToList()[0].UserId);
            Assert.IsNotNull(result.RecruiterList.ToList()[0].RecruiterId);
            Assert.AreEqual("Test", result.RecruiterList.ToList()[0].Description);
            Assert.AreEqual("Test", result.RecruiterList.ToList()[0].UrlContact);
        }

        [Test]
        public async Task GetById()
        {
            var result = await recruiterService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.UserId);
            Assert.IsNotNull(result.RecruiterId);
            Assert.AreEqual("Test", result.Description);
            Assert.AreEqual("Test", result.UrlContact);
        }


        [Test]
        public async Task Create_Test()
        {
            var createrecruiter = new CreateRecruiterModel()
            {
                UserId = new Guid()
            };
            var result = await recruiterService.Create(createrecruiter);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            try
            {
                var recruiterupdate = new RecruiterUserInfoUpdateModel();
                var result = await recruiterService.Update(It.IsAny<Guid>(), recruiterupdate);
                Assert.IsNotNull(result);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await recruiterService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Restore_Test()
        {
            var result = await recruiterService.Restore(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
