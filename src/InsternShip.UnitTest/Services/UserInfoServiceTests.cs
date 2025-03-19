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
    public class UserInfoServiceTests
    {
        private UserInfoService userinfoService;
        private IUnitOfWork unitOfWork;
        private IConfiguration configuration;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {
            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);

            var userinfo = new UserInfoViewModel()
            {
                InfoId = new Guid(),
                Avatar = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = "Test",
                DateOfBirth = DateTime.Now,
                PhoneNumber = "Test",
                Location = "Test",
            };

            var mockIUserInfoRepository = new Mock<IUserInfoRepository>();
            var mockICandidateRepository = new Mock<ICandidateRepository>();
            var mockIInterviewerRepository = new Mock<IInterviewerRepository>();
            var mockIRecruiterRepository = new Mock<IRecruiterRepository>();
            var mockICVRepository = new Mock<ICVRepository>();
            var mockIUserAccountRepository = new Mock<IUserAccountRepository>();
            mockIUserInfoRepository.Setup(x => x.UpdateInfo(It.IsAny<Guid>(), It.IsAny<UserInfoUpdateModel>())).Returns(Task.FromResult(true));
            userinfoService = new UserInfoService(mockIUserInfoRepository.Object,
                                                    mockICandidateRepository.Object,
                                                    mockIInterviewerRepository.Object,
                                                    mockIRecruiterRepository.Object,
                                                    mockICVRepository.Object,
                                                    mockIUserAccountRepository.Object);
        }

        [Test]
        public async Task UpdateInfo_Test()
        {
            var result = await userinfoService.UpdateInfo(It.IsAny<Guid>(), It.IsAny<UserInfoUpdateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
