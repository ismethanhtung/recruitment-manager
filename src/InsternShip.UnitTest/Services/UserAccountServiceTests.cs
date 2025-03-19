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
    public class UserAccountServiceTests
    {
        private UserAccountService useraccountService;
        private IUnitOfWork unitOfWork;
        private IConfiguration configuration;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {
            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);

            var useraccount = new UserAccountViewModel()
            {
                UserId = new Guid(),
                InfoId = new Guid(),
                Email = "Test",
                UserName = "Test",
                RegistrationDate = DateTime.Now,
                ActiveStatus = true,
                Role = "Test",
            };
            var useraccountl = new List<UserAccountViewModel>();
            useraccountl.Add(useraccount);
            var useraccountlist = new UserAccountListViewModel()
            {
                TotalCount = 0,
                AccountList = useraccountl
            };
            var signinview = new SignInViewModel()
            {
                Token = "Test",
                Roles = "Test"
            };

            var mockIUserAccountRepository = new Mock<IUserAccountRepository>();
            var mockIUserInfoRepository = new Mock<IUserInfoRepository>();
            var mockICandidateRepository = new Mock<ICandidateRepository>();
            var mockIInterviewerRepository = new Mock<IInterviewerRepository>();
            var mockIRecruiterRepository = new Mock<IRecruiterRepository>();
            mockIUserAccountRepository.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(useraccountlist));
            mockIUserAccountRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(useraccount));
            mockIUserAccountRepository.Setup(x => x.ChangePassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            mockIUserAccountRepository.Setup(x => x.SignUp(It.IsAny<SignUpModel>())).Returns(Task.FromResult(IdentityResult.Success));
            mockIUserAccountRepository.Setup(x => x.SignIn(It.IsAny<SignInModel>(), It.IsAny<Guid>())).Returns(Task.FromResult(signinview));
            mockIUserAccountRepository.Setup(x => x.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            mockIUserAccountRepository.Setup(x => x.ResetPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            useraccountService = new UserAccountService(mockIUserAccountRepository.Object,
                                                    mockIUserInfoRepository.Object,
                                                    mockICandidateRepository.Object,
                                                    mockIInterviewerRepository.Object,
                                                    mockIRecruiterRepository.Object);
        }


        [Test]
        public async Task GetAll_Test()
        {
            var result = await useraccountService.GetAll(It.IsAny<int>(), It.IsAny<int>());
            Assert.AreEqual(0, result.TotalCount);
            Assert.IsNotNull(result.AccountList.ToList()[0].UserId);
            Assert.IsNotNull(result.AccountList.ToList()[0].InfoId);
            Assert.AreEqual("Test", result.AccountList.ToList()[0].Email);
            Assert.AreEqual("Test", result.AccountList.ToList()[0].UserName);
            Assert.IsNotNull(result.AccountList.ToList()[0].RegistrationDate);
            Assert.AreEqual(true, result.AccountList.ToList()[0].ActiveStatus);
            Assert.AreEqual("Test", result.AccountList.ToList()[0].Email);
            Assert.AreEqual("Test", result.AccountList.ToList()[0].Role);
        }

        [Test]
        public async Task GetById_Test()
        {
            var result = await useraccountService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result.UserId);
            Assert.IsNotNull(result.InfoId);
            Assert.AreEqual("Test", result.Email);
            Assert.AreEqual("Test", result.UserName);
            Assert.IsNotNull(result.RegistrationDate);
            Assert.AreEqual(true, result.ActiveStatus);
            Assert.AreEqual("Test", result.Email);
            Assert.AreEqual("Test", result.Role);
        }

        [Test]
        public async Task SignUp_Test()
        {
            var result = await useraccountService.SignUp(It.IsAny<SignUpModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(IdentityResult.Success, result);
        }

        [Test]
        public async Task SignIn_Test()
        {
            var signin = new SignInModel();
            var result = await useraccountService.SignIn(signin);
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Token);
            Assert.AreEqual("Test", result.Roles);
        }

        [Test]
        public async Task ChangePassword_Test()
        {
            var result = await useraccountService.ChangePassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.AreEqual(IdentityResult.Success, result);
        }

        [Test]
        public async Task ConfirmEmail_Test()
        {
            var result = await useraccountService.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task ResetPassword_Test()
        {
            var result = await useraccountService.ResetPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
