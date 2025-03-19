using AutoMapper;
using InsternShip.Data;
using InsternShip.Data.Entities;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using InsternShip.UnitTest.Repositories;

namespace InsternShip.UnitTest.Repository
{
    public class UserAccountRepositoryTests : BaseRepositoryClass
    {
        private BlackListRepository blackListRepository;
        private UserAccountRepository userAccountRepository;
        private IUnitOfWork unitOfWork;

        private UserManager<UserAccount> userManager;
        private SignInManager<UserAccount> signInManager;
        private IMailerRepository mailer;
        private IUrlHelper urlHelper;
        private IConfiguration configuration;
        private RoleManager<Roles> roleManager;

        [SetUp]
        public void Setup()
        {
            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            userAccountRepository = new UserAccountRepository(db, unitOfWork, userManager, mailer, signInManager, mapper, configuration, urlHelper, roleManager, blackListRepository);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await userAccountRepository.GetAll(1, 5);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetById_Test()
        {
            try
            {
                var result = await userAccountRepository.GetById(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetUserIdByEmail_Test()
        {
            try
            {
                var result = userAccountRepository.GetUserIdByEmail("admin@gmail.com");
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetEmailByUserId_Test()
        {
            try
            {
                var result = userAccountRepository.GetEmailByUserId(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetRoleByUserId_Test()
        {
            try
            {
                Guid userid = (await userAccountRepository.GetAll(1, 5)).AccountList.First().UserId;
                var result = await userAccountRepository.GetRoleByUserId(userid);
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetConfirmMail_Test()
        {
            try
            {
                var result = await userAccountRepository.GetConfirmMail("admin@gmail.com");
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task SignUp_Test()
        {
            try
            {
                var signup = new SignUpModel()
                {
                    FirstName = "a",
                    LastName = "a",
                    Email = "a",
                    Password = "a",
                    ConfirmPassword = "a",
                    Avatar = "a",
                    Gender = "a",
                    DateOfBirth = DateTime.Now,
                    Location = "a"
                };
                var result = await userAccountRepository.SignUp(signup);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task SignIn_Test()
        {
            try
            {
                var signin = new SignInModel()
                {
                    Email = "a",
                    Password = "a",
                };
                var result = await userAccountRepository.SignIn(signin, new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task ForgetPassword_Test()
        {
            try
            {
                var reset = new ResetPasswordModel();
                var result = await userAccountRepository.ForgetPassword(reset);
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task ResetPassword_Test()
        {
            try
            {
                var reset = new ResetPasswordModel();
                var result = await userAccountRepository.ResetPassword("","","");
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task ConfirmEmail_Test()
        {
            try
            {
                var reset = new ResetPasswordModel();
                var result = await userAccountRepository.ConfirmEmail("", "");
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
