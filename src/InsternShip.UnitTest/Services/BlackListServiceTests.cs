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
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.UnitTest.Services
{
    public class BlackListServiceTests
    {
        private BlackListService blacklistService;
        private BlackListRepository blacklistRepository;
        private UserAccountService useraccountService;
        private UserAccountRepository useraccoutRepository;
        private IUnitOfWork unitOfWork;
        private UserManager<UserAccount> userManager;
        private MailerRepository mailer;
        private SignInManager<UserAccount> signInManager;
        private IConfiguration configuration;
        private IMapper mapper;
        private IUrlHelper urlHelper;

        [SetUp]
        public void Setup()
        {

            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);
            blacklistRepository = new BlackListRepository(db, unitOfWork, mapper);

            var blacklistlist = new BlackListEntriesViewModel()
            {
                TotalCount = 0
            };
            var blacklist = new BlackListViewModel()
            {
                BlacklistId = new Guid(),
                UserId = new Guid(),
                FirstName = "Test",
                LastName = "Test",
                Duration = 0,
                EntryDate = DateTime.Now,
                Reason = "Test",

            };
            var mockIBlackListRepository = new Mock<IBlackListRepository>();
            var mockIUserAccountRepository = new Mock<IUserAccountRepository>();
            var mockIUserInfoRepository = new Mock<IUserInfoRepository>();
            var mockICandidateRepository = new Mock<ICandidateRepository>();
            var mockIInterviewerRepository = new Mock<IInterviewerRepository>();
            var mockIRecruiterRepository = new Mock<IRecruiterRepository>();
            mockIBlackListRepository.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.FromResult(blacklistlist));
            mockIBlackListRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(blacklist));
            mockIBlackListRepository.Setup(x => x.Create(It.IsAny<CreateBlackListModel>())).Returns(Task.FromResult(true));
            mockIBlackListRepository.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<BlackListUpdateModel>())).Returns(Task.FromResult(true));
            mockIBlackListRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockIBlackListRepository.Setup(x => x.Restore(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            blacklistService = new BlackListService(mockIBlackListRepository.Object, mockIUserAccountRepository.Object);
            useraccountService = new UserAccountService(mockIUserAccountRepository.Object,
                                                        mockIUserInfoRepository.Object,
                                                        mockICandidateRepository.Object,
                                                        mockIInterviewerRepository.Object,
                                                        mockIRecruiterRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await blacklistService.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount);
        }

        [Test]
        public async Task GetById()
        {
            var result = await blacklistService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result.BlacklistId);
            Assert.IsNotNull(result.UserId);
            Assert.AreEqual("Test", result.FirstName);
            Assert.AreEqual("Test", result.LastName);
            Assert.AreEqual(0, result.Duration);
            Assert.IsNotNull(result.EntryDate);
            Assert.AreEqual("Test", result.Reason);
        }

        [Test]
        public async Task Create_Test()
        {
            var blacklistcreate = new CreateBlackListModel()
            {
                UserId = new Guid(),
                Reason = "Test"
            };
            var result = await blacklistService.Create(blacklistcreate);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await blacklistService.Update(It.IsAny<Guid>(), It.IsAny<BlackListUpdateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await blacklistService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Restore_Test()
        {
            var result = await blacklistService.Restore(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
